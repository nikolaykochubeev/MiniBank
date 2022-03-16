using System;

namespace Minibank.Web.Users
{
    public class UserDbModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; } 
    }
}