using System;
using Minibank.Core;
using Minibank.Core.Domain.BankAccounts;
using Minibank.Core.Domain.BankAccounts.Repositories;
using Minibank.Core.Domain.BankAccounts.Services;
using Minibank.Core.Domain.BankAccounts.Validators;
using Minibank.Core.Domain.Currency;
using Minibank.Core.Domain.Currency.Services;
using Minibank.Core.Domain.Transactions;
using Minibank.Core.Domain.Transactions.Repositories;
using Minibank.Core.Domain.Users;
using Minibank.Core.Domain.Users.Repositories;
using Minibank.Core.Exceptions;
using Moq;
using Xunit;

namespace Minibank.Core.Tests;

public class BankAccountServiceTests
{
    private readonly IBankAccountService _bankAccountService;
    private readonly Mock<IUserRepository> _fakeUserRepository;
    private readonly Mock<IUnitOfWork> _fakeUnitOfWork;
    private readonly BankAccountValidator _fakeBankAccountValidator;
    private readonly Mock<IBankAccountRepository> _fakeBankAccountRepository;
    private readonly Mock<ITransactionRepository> _fakeTransactionRepository;
    private readonly CurrencyService _currencyService;
    private readonly Mock<ICurrencyCourseProvider> _fakeCurrencyCourseProvider;

    public BankAccountServiceTests()
    {
        _fakeUserRepository = new Mock<IUserRepository>();
        _fakeBankAccountRepository = new Mock<IBankAccountRepository>();
        _fakeTransactionRepository = new Mock<ITransactionRepository>();
        _fakeUnitOfWork = new Mock<IUnitOfWork>();
        _fakeBankAccountValidator = new BankAccountValidator();
        _fakeCurrencyCourseProvider = new Mock<ICurrencyCourseProvider>();
        _currencyService = new CurrencyService(_fakeCurrencyCourseProvider.Object);
        _bankAccountService = new BankAccountService(_fakeUnitOfWork.Object, _fakeBankAccountRepository.Object,
            _fakeUserRepository.Object,
            _fakeTransactionRepository.Object, _currencyService, _fakeBankAccountValidator);
    }

    [Fact]
    public void CreateBankAccount_Success_ShouldCreateBankAccount()
    {
        var userGuid = Guid.NewGuid();
        var bankAccountGuid = Guid.NewGuid();
        var user = new UserModel() { Id = userGuid, Email = "email", Login = "Login" };
        var bankAccount = new BankAccountModel()
        {
            Id = bankAccountGuid, UserId = user.Id, AmountOfMoney = 10, Currency = CurrencyModel.USD, IsActive = true
        };

        _fakeUserRepository.Setup(repo => repo.GetById(userGuid).Result).Returns(user);
        _fakeBankAccountRepository.Setup(repo => repo.Create(bankAccount).Result).Returns(bankAccountGuid);

        Assert.Equal(bankAccountGuid, _bankAccountService.CreateAsync(bankAccount).Result);
        _fakeUnitOfWork.Verify(work => work.SaveChanges(), Times.Once);
    }

    [Fact]
    public void CreateBankAccount_WithAmountOfMoneyLessThanZero_ShouldThrowException()
    {
        var userGuid = Guid.NewGuid();
        var bankAccountGuid = Guid.NewGuid();
        var user = new UserModel() { Id = userGuid, Email = "email", Login = "Login" };
        var bankAccount = new BankAccountModel()
        {
            Id = bankAccountGuid, UserId = user.Id, AmountOfMoney = 0, Currency = CurrencyModel.USD, IsActive = true
        };
        _fakeUserRepository.Setup(repo => repo.GetById(userGuid).Result).Returns(user);
        _fakeBankAccountRepository.Setup(repo => repo.Create(bankAccount).Result).Returns(bankAccountGuid);

        var exception =
            Assert.ThrowsAsync<Minibank.Core.Exceptions.ValidationException>(() => _bankAccountService
                .CreateAsync(bankAccount));

        Assert.Contains("Unable to create an account, the amount of money cannot be negative",
            exception.Result.Message);
        _fakeUnitOfWork.Verify(work => work.SaveChanges(), Times.Never);
    }

    [Fact]
    public void CreateBankAccount_WithNotExistingUser_ShouldThrowException()
    {
        var userGuid = Guid.NewGuid();
        var bankAccountGuid = Guid.NewGuid();
        var bankAccount = new BankAccountModel()
        {
            Id = bankAccountGuid, UserId = userGuid, AmountOfMoney = 0, Currency = CurrencyModel.USD, IsActive = true
        };
        _fakeUserRepository.Setup(repo => repo.GetById(userGuid).Result).Returns((UserModel)null!);
        _fakeBankAccountRepository.Setup(repo => repo.Create(bankAccount).Result).Returns(bankAccountGuid);

        var exception =
            Assert.ThrowsAsync<ObjectNotFoundException>(() => _bankAccountService
                .CreateAsync(bankAccount));

        Assert.Contains($"User with id = {userGuid} does not exist", exception.Result.Message);
        _fakeUnitOfWork.Verify(work => work.SaveChanges(), Times.Never);
    }

    [Fact]
    public void GetBankAccountById_Success_ShouldReturnBankAccount()
    {
        var bankAccountGuid = Guid.NewGuid();
        var bankAccount = new BankAccountModel()
        {
            Id = bankAccountGuid, UserId = Guid.NewGuid(), AmountOfMoney = 0, Currency = CurrencyModel.USD,
            IsActive = true
        };

        _fakeBankAccountRepository.Setup(repo => repo.GetById(bankAccountGuid).Result).Returns(bankAccount);
        var returnedBankAccount = _bankAccountService.GetByIdAsync(bankAccountGuid);

        Assert.Equal(returnedBankAccount.Result, bankAccount);
        _fakeUnitOfWork.Verify(work => work.SaveChanges(), Times.Never);
    }

    [Fact]
    public void GetBankAccountById_WithNotExistingBankAccount_ShouldThrowException()
    {
        Assert.ThrowsAsync<ObjectNotFoundException>(() => _bankAccountService.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public void CalculateCommission_FromAccountAndTransactionHaveDifferentCurrencies_ShouldThrowException()
    {
        var bankAccountGuid = Guid.NewGuid();
        var bankAccountGuid2 = Guid.NewGuid();
        var transactionId = Guid.NewGuid();

        var bankAccount = new BankAccountModel()
        {
            Id = bankAccountGuid, UserId = Guid.NewGuid(), AmountOfMoney = 10, Currency = CurrencyModel.EUR,
            IsActive = true
        };
        var bankAccount2 = new BankAccountModel()
        {
            Id = bankAccountGuid2, UserId = Guid.NewGuid(), AmountOfMoney = 10, Currency = CurrencyModel.USD,
            IsActive = true
        };
        var transaction = new TransactionModel()
        {
            AmountOfMoney = 5, Currency = CurrencyModel.RUB, FromAccountId = bankAccountGuid,
            ToAccountId = bankAccountGuid2, Id = transactionId
        };

        _fakeBankAccountRepository.Setup(repo => repo.GetById(bankAccountGuid).Result).Returns(bankAccount);
        _fakeBankAccountRepository.Setup(repo => repo.GetById(bankAccountGuid2).Result).Returns(bankAccount2);
        var exception =
            Assert.ThrowsAsync<Minibank.Core.Exceptions.ValidationException>(() =>
                _bankAccountService.TransferAsync(transaction));

        Assert.Contains(
            "The final currencies of the transaction and the accounts to which the money is received do not match",
            exception.Result.Message);
    }

    [Fact]
    public void CalculateCommission_Success_ShouldCalculateCorrectCommission()
    {
        var bankAccountGuid = Guid.NewGuid();
        var bankAccountGuid2 = Guid.NewGuid();
        var transactionId = Guid.NewGuid();

        var fromAccount = new BankAccountModel()
        {
            Id = bankAccountGuid, UserId = Guid.NewGuid(), AmountOfMoney = 10, Currency = CurrencyModel.USD,
            IsActive = true
        };
        var toAccount = new BankAccountModel()
        {
            Id = bankAccountGuid2, UserId = Guid.NewGuid(), AmountOfMoney = 10, Currency = CurrencyModel.USD,
            IsActive = true
        };
        var transaction = new TransactionModel()
        {
            AmountOfMoney = 5, Currency = CurrencyModel.USD, FromAccountId = bankAccountGuid,
            ToAccountId = bankAccountGuid2, Id = transactionId
        };
        var correctCommission = transaction.AmountOfMoney * (decimal)0.02;

        _fakeBankAccountRepository.Setup(repo => repo.GetById(bankAccountGuid).Result).Returns(fromAccount);
        _fakeBankAccountRepository.Setup(repo => repo.GetById(bankAccountGuid2).Result).Returns(toAccount);

        var commissionFromService = _bankAccountService.CalculateCommissionAsync(transaction);

        Assert.Equal(correctCommission, commissionFromService.Result);
    }

    [Fact]
    public void CalculateCommission_WithOneUserTwoAccounts_ShouldCalculateCorrectCommission()
    {
        var bankAccountGuid = Guid.NewGuid();
        var bankAccountGuid2 = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var fromAccount = new BankAccountModel()
        {
            Id = bankAccountGuid, UserId = userId, AmountOfMoney = 10, Currency = CurrencyModel.USD,
            IsActive = true
        };
        var toAccount = new BankAccountModel()
        {
            Id = bankAccountGuid2, UserId = userId, AmountOfMoney = 10, Currency = CurrencyModel.USD,
            IsActive = true
        };
        var transaction = new TransactionModel()
        {
            AmountOfMoney = 5, Currency = CurrencyModel.USD, FromAccountId = bankAccountGuid,
            ToAccountId = bankAccountGuid2, Id = transactionId
        };
        const int correctCommission = 0;

        _fakeBankAccountRepository.Setup(repo => repo.GetById(bankAccountGuid).Result).Returns(fromAccount);
        _fakeBankAccountRepository.Setup(repo => repo.GetById(bankAccountGuid2).Result).Returns(toAccount);

        var commissionFromService = _bankAccountService.CalculateCommissionAsync(transaction);

        Assert.Equal(correctCommission, commissionFromService.Result);
    }


    [Fact]
    public void Transfer_Success_ShouldCreateTransaction()
    {
        var bankAccountGuid = Guid.NewGuid();
        var bankAccountGuid2 = Guid.NewGuid();
        var transactionId = Guid.NewGuid();

        var fromAccount = new BankAccountModel()
        {
            Id = bankAccountGuid, UserId = Guid.NewGuid(), AmountOfMoney = 10, Currency = CurrencyModel.USD,
            IsActive = true
        };
        var toAccount = new BankAccountModel()
        {
            Id = bankAccountGuid2, UserId = Guid.NewGuid(), AmountOfMoney = 10, Currency = CurrencyModel.USD,
            IsActive = true
        };
        var transaction = new TransactionModel()
        {
            AmountOfMoney = 5, Currency = CurrencyModel.USD, FromAccountId = bankAccountGuid,
            ToAccountId = bankAccountGuid2, Id = transactionId
        };

        _fakeBankAccountRepository.Setup(repo => repo.GetById(bankAccountGuid).Result).Returns(fromAccount);
        _fakeBankAccountRepository.Setup(repo => repo.GetById(bankAccountGuid2).Result).Returns(toAccount);
        _fakeTransactionRepository.Setup(repo => repo.Create(transaction).Result).Returns(transactionId);
        var transactionIdFromService = _bankAccountService.TransferAsync(transaction);
        Assert.Equal((decimal) 4.9, _bankAccountService.GetByIdAsync(bankAccountGuid).Result.AmountOfMoney);
        Assert.Equal(15, _bankAccountService.GetByIdAsync(bankAccountGuid2).Result.AmountOfMoney);
        Assert.Equal(transactionId, transactionIdFromService.Result);
        _fakeUnitOfWork.Verify(work => work.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Transfer_WithNullBalanceAfterTransfer_ShouldCreateTransaction()
    {
        var bankAccountGuid = Guid.NewGuid();
        var bankAccountGuid2 = Guid.NewGuid();
        var transactionId = Guid.NewGuid();

        var fromAccount = new BankAccountModel()
        {
            Id = bankAccountGuid, UserId = Guid.NewGuid(), AmountOfMoney = 0, Currency = CurrencyModel.USD,
            IsActive = true
        };
        var toAccount = new BankAccountModel()
        {
            Id = bankAccountGuid2, UserId = Guid.NewGuid(), AmountOfMoney = 0, Currency = CurrencyModel.USD,
            IsActive = true
        };
        var transaction = new TransactionModel()
        {
            AmountOfMoney = 0, Currency = CurrencyModel.USD, FromAccountId = bankAccountGuid,
            ToAccountId = bankAccountGuid2, Id = transactionId
        };
        
        _fakeBankAccountRepository.Setup(repo => repo.GetById(bankAccountGuid).Result).Returns(fromAccount);
        _fakeBankAccountRepository.Setup(repo => repo.GetById(bankAccountGuid2).Result).Returns(toAccount);
        _fakeTransactionRepository.Setup(repo => repo.Create(transaction).Result).Returns(transactionId);
        var transactionIdFromService = _bankAccountService.TransferAsync(transaction);

        Assert.Equal(transactionId, transactionIdFromService.Result);
    }
    

    [Fact]
    public void CloseBankAccount_Success_CloseCalled()
    {
        var bankAccountGuid = Guid.NewGuid();
        var bankAccount = new BankAccountModel()
        {
            Id = bankAccountGuid, UserId = Guid.NewGuid(), AmountOfMoney = 0, Currency = CurrencyModel.USD,
            IsActive = true
        };

        _fakeBankAccountRepository.Setup(repo => repo.GetById(bankAccountGuid).Result).Returns(bankAccount);
        _bankAccountService.CloseAsync(bankAccountGuid);
        _fakeBankAccountRepository.Verify(repo => repo.Close(bankAccountGuid), Times.Once);
        _fakeUnitOfWork.Verify(work => work.SaveChanges(), Times.Once);
    }
}
