using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Minibank.Core.Domain.Transactions;

namespace Minibank.Core.Domain.BankAccounts.Services
{
    public interface IBankAccountService
    {
        Task<BankAccountModel> GetById(Guid id);
        Task<IEnumerable<BankAccountModel>> GetAll();
        Task<IEnumerable<TransactionModel>> GetAllTransactions();
        Task<Guid> Create(BankAccountModel bankAccountModel);
        Task<decimal> CalculateCommission(TransactionModel transactionModel);
        Task<Guid> Transfer(TransactionModel transactionModel);
        Task Close(Guid id);
    }
}