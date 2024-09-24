using Microsoft.AspNetCore.Mvc;
using eBooksAPI.Database;
using eBooksAPI.Database.Models;
using eBooksAPI.Helpers;

namespace eBooksAPI.Controllers.AuthControllers.Register
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly eBooksContext _db;
        public AuthController(eBooksContext context)
        {
            _db = context;
        }

        [HttpPost("register")]
        public ActionResult Action([FromBody] RegisterReq req)
        {
            if (_db.Users.FirstOrDefault(x => x.Email == req.Email) != null)
            {
                return StatusCode(400, "User with this email already exists");
            }

            if (!RegexValidator.IsValidEmail(req.Email))
            {
                return StatusCode(400, "Email is not valid");
            }

            if (!RegexValidator.IsValidPassword(req.Password))
            {
                return StatusCode(400, "Password must contain 8 characters, 1 uppercase letter, 1 lowercase letter, 1 digit, and 1 special character");
            }

            _db.Users.Add(new User
            {
                Email = req.Email,
                PasswordHash = Hashing.Sha256Hash(req.Password),
                FirstName = req.FirstName,
                LastName = req.LastName,
                UserName = req.UserName,
                RegistrationDate = DateTime.UtcNow,
                RoleId = _db.Roles.Any(x => x.RoleId == req.RoleId) ? req.RoleId : 1,
            });
            _db.SaveChanges();

            return StatusCode(200);
        }
    }
}
