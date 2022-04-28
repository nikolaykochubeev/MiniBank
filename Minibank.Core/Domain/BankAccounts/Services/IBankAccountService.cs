using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Minibank.Core.Domain.Transactions;

namespace Minibank.Core.Domain.BankAccounts.Services
{
    public interface IBankAccountService
    {
        Task<BankAccountModel> GetByIdAsync(Guid id);
        Task<IEnumerable<BankAccountModel>> GetAllAsync();
        Task<IEnumerable<TransactionModel>> GetAllTransactionsAsync();
        Task<Guid> CreateAsync(BankAccountModel bankAccountModel);
        Task<decimal> CalculateCommissionAsync(TransactionModel transactionModel);
        Task<Guid> TransferAsync(TransactionModel transactionModel);
        Task CloseAsync(Guid id);
    }
}