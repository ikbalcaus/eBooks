using Microsoft.AspNetCore.Mvc;
using eBooksAPI.Database;
using eBooksAPI.Database.Models;
using eBooksAPI.Controllers.Auth.Login;
using BaseCureAPI.Helpers;

namespace BaseCureAPI.Endpoints.Auth.Login
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
        public ActionResult Login([FromBody] LoginReq req)
        {
            var data = _db.Users.FirstOrDefault(x => x.Email == req.Email);
            var token = TokenGenerator.GenerateToken();

            if (data == null) {
                return StatusCode(401, "User not found");
            }

            _db.AuthTokens.Add(new AuthToken
            {
                UserId = data.UserId,
                Token = token,
                LoggedInDate = DateTime.UtcNow
            });
            _db.SaveChanges();

            return StatusCode(200, token);
        }
    }
}
