using System;
using System.Collections.Generic;
using Minibank.Core.Domains.BankAccount.Repositories;

namespace Minibank.Core.Domains.BankAccount.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;

        public BankAccountService(IBankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }
        public BankAccountModel Get(Guid id)
        {
            return _bankAccountRepository.Get(id);
        }

        public IEnumerable<BankAccountModel> GetAll()
        {
            return _bankAccountRepository.GetAll();
        }

        public void Create(BankAccountModel bankAccountModel)
        {
            _bankAccountRepository.Create(bankAccountModel);
        }

        public void Update(BankAccountModel bankAccountModel)
        {
            _bankAccountRepository.Update(bankAccountModel);
        }

        public void Delete(Guid id)
        {
            _bankAccountRepository.Delete(id);
        }
    }
}