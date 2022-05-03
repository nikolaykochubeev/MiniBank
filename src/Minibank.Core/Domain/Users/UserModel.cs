using System;

namespace Minibank.Core.Domain.Users
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
    }
}