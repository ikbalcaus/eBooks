using Microsoft.AspNetCore.Http;

namespace eBooks.Models.Requests
{
    public class UsersPostReq
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
