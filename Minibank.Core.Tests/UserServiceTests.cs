using System;
using FluentValidation;
using Minibank.Core.Domain.BankAccounts.Repositories;
using Minibank.Core.Domain.Users;
using Minibank.Core.Domain.Users.Repositories;
using Minibank.Core.Domain.Users.Services;
using Minibank.Core.Domain.Users.Validators;
using Moq;
using Xunit;

namespace Minibank.Core.Tests;

public class UserServiceTests
{
    private readonly IUserService _userService;
    private readonly Mock<IUserRepository> _fakeUserRepository;
    private readonly Mock<IUnitOfWork> _fakeUnitOfWork;
    private readonly UserValidator _fakeUserValidator;
    private readonly Mock<IBankAccountRepository> _fakeBankAccountRepository;

    public UserServiceTests()
    {
        _fakeUserRepository = new Mock<IUserRepository>();
        _fakeBankAccountRepository = new Mock<IBankAccountRepository>();
        _fakeUnitOfWork = new Mock<IUnitOfWork>();
        _fakeUserValidator = new UserValidator();

        _userService = new UserService(_fakeUnitOfWork.Object, _fakeUserRepository.Object,
            _fakeBankAccountRepository.Object, _fakeUserValidator);
    }

    [Fact]
    public void AddUser_WithEmptyLoginOrEmail_ShouldThrowException()
    {
        var user = new UserModel { Email = "email@mail.ru", Id = Guid.NewGuid(), Login = "" };
        Assert.ThrowsAsync<ValidationException>(() => _userService.CreateAsync(user));
        
        user.Email = "";
        user.Login = "login";
        Assert.ThrowsAsync<ValidationException>(() => _userService.CreateAsync(user));
    }
    
    [Fact]
    public void AddUser_WithValidLoginAndEmail_ShouldCreateUser()
    {
        var user = new UserModel { Email = "email@mail.ru", Id = Guid.NewGuid(), Login = "Login" };
    }
}