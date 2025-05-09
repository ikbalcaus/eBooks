﻿using eBooks.Database;
using eBooks.Database.Models;
using eBooks.Interfaces;
using eBooks.Models.Exceptions;
using eBooks.Models.User;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace eBooks.Services;

public class UsersService : BaseService<User, UsersSearch, UsersCreateReq, UsersUpdateReq, UsersRes>, IUsersService
{
    protected ILogger<UsersService> _logger;

    public UsersService(EBooksContext db, IMapper mapper, ILogger<UsersService> logger)
        : base(db, mapper)
    {
        _logger = logger;
    }

    public override async Task<UsersRes> Delete(int id)
    {
        var set = _db.Set<User>();
        var entity = await set.FindAsync(id);
        if (entity == null)
            throw new ExceptionNotFound();
        entity.IsDeleted = true;
        await _db.SaveChangesAsync();
        return null;
    }

    public async Task<UsersRes> UndoDelete(int id)
    {
        var set = _db.Set<User>();
        var entity = await set.FindAsync(id);
        if (entity == null)
            throw new ExceptionNotFound();
        entity.IsDeleted = false;
        await _db.SaveChangesAsync();
        return _mapper.Map<UsersRes>(entity);
    }

    public async Task<UsersRes> ChangeRole(int id, int roleId)
    {
        var entity = await _db.Set<User>().Include(x => x.Role).FirstOrDefaultAsync(x => x.UserId == id);
        if (entity == null)
            throw new ExceptionNotFound();
        var role = await _db.Set<Role>().FirstOrDefaultAsync(x => x.RoleId == roleId);
        if (role == null)
            throw new ExceptionNotFound();
        entity.Role = role;
        await _db.SaveChangesAsync();
        return _mapper.Map<UsersRes>(entity);
    }

    public async Task<UsersRes> Login(string email, string password)
    {
        var entity = await _db.Set<User>().Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == email);
        if (entity == null)
        {
            _logger.LogInformation($"User failed to log in {email}.");
            return null;
        }
        if (Helpers.GenerateHash(entity.PasswordSalt, password) != entity.PasswordHash)
        {
            _logger.LogInformation($"User with email {email} failed to log in.");
            return null;
        }
        _logger.LogInformation($"User with email {email} logged in.");
        return _mapper.Map<UsersRes>(entity);
    }

    public override async Task BeforeCreate(User entity, UsersCreateReq req)
    {
        if (!Helpers.IsEmailValid(req.Email))
            throw new ExceptionBadRequest("Email is not valid");
        if (await _db.Users.AnyAsync(x => x.Email == req.Email))
            throw new ExceptionBadRequest("Email already exist");
        if (await _db.Users.AnyAsync(x => x.UserName == req.UserName))
            throw new ExceptionBadRequest("Username already exists");
        if (!Helpers.IsPasswordValid(req.Password))
            throw new ExceptionBadRequest("Password is not valid");
        entity.PasswordSalt = Helpers.GenerateSalt();
        entity.PasswordHash = Helpers.GenerateHash(entity.PasswordSalt, req.Password);
        entity.RoleId = (await _db.Set<Role>().FirstOrDefaultAsync(x => x.Name == "User")).RoleId;
        _logger.LogInformation($"User with email {req.Email} created.");
    }

    public override async Task BeforeUpdate(User entity, UsersUpdateReq req)
    {
        if (!string.IsNullOrWhiteSpace(req.UserName) && await _db.Set<User>().AnyAsync(x => x.UserName == req.UserName))
            throw new ExceptionBadRequest("Username already exist");
        if (!string.IsNullOrWhiteSpace(req.Password))
        {
            if (!Helpers.IsPasswordValid(req.Password))
                throw new ExceptionBadRequest("Password is not valid");
            entity.PasswordSalt = Helpers.GenerateSalt();
            entity.PasswordHash = Helpers.GenerateHash(entity.PasswordSalt, req.Password);
        }
        _logger.LogInformation($"User with email {entity.Email} updated.");
    }
}
