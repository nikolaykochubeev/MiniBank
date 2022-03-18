using System;
using System.Collections.Generic;
using Minibank.Core.Domains.Transactions;

namespace Minibank.Core.Domains.BankAccount.Services
{
    public interface IBankAccountService
    {
        BankAccountModel Get(Guid id);
        IEnumerable<BankAccountModel> GetAll();
        IEnumerable<TransactionModel> GetAllTransactions();
        Guid Create(BankAccountModel bankAccountModel);
        public decimal CalculateCommission(TransactionModel transactionModel);
        public Guid Transfer(TransactionModel transactionModel);
        void Close(Guid id);
    }
}