using Microsoft.AspNetCore.Mvc;
using eBooksAPI.Database;
using eBooksAPI.Database.Models;
using eBooksAPI.Helpers;

namespace eBooksAPI.Controllers.AuthControllers.Login
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

        [HttpPost("login")]
        public ActionResult Action([FromBody] LoginReq req)
        {
            var data = _db.Users.FirstOrDefault(x => x.Email == req.Email);

            if (data == null) {
                return StatusCode(401, "User not found");
            }

            if (data.PasswordHash != Hashing.Sha256Hash(req.Password))
            {
                return StatusCode(401, "Password is not correct");
            }

            var token = TokenGenerator.GenerateUniqueToken(_db);

            _db.AuthTokens.Add(new AuthToken
            {
                UserId = data.UserId,
                Token = token,
                LoggedInDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddDays(7),
            });
            _db.SaveChanges();

            return StatusCode(200, token);
        }
    }
}
