using System;
using System.Collections.Generic;
using Minibank.Core.Domains.Transactions;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public interface IBankAccountService
    {
        BankAccountModel GetById(Guid id);
        IEnumerable<BankAccountModel> GetAll();
        IEnumerable<TransactionModel> GetAllTransactions();
        Guid Create(BankAccountModel bankAccountModel);
        public decimal CalculateCommission(TransactionModel transactionModel);
        public Guid Transfer(TransactionModel transactionModel);
        void Close(Guid id);
    }
}