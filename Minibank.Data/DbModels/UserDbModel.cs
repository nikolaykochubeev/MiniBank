using System;

namespace Minibank.Data.DbModels
{
    public class UserDbModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; } 
        public uint AmountOfBankAccounts { get; set; }
    }
}