﻿using eBooks.Models.Roles;

namespace eBooks.Models.User
{
    public class UsersRes
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public RolesRes Role { get; set; }
    }
}
