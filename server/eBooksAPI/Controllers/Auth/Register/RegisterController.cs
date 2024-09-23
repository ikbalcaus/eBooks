using Microsoft.AspNetCore.Mvc;
using eBooksAPI.Database;
using eBooksAPI.Database.Models;
using eBooksAPI.Controllers.Auth.Register;

namespace BaseCureAPI.Endpoints.Auth.Register
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
        public ActionResult Register([FromBody] RegisterReq req)
        {
            _db.Users.Add(new User
            {
                Email = req.Email,
                PasswordHash = req.Password,
                FirstName = req.FirstName,
                LastName = req.LastName,
                UserName = req.UserName,
                RegistrationDate = DateTime.UtcNow
            });
            _db.SaveChanges();
            return StatusCode(200);
        }
    }
}
