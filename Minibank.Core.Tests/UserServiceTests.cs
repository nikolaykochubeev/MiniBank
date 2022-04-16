using System;
using System.Threading.Tasks;
using Minibank.Core.Domain.BankAccounts.Repositories;
using Minibank.Core.Domain.Users;
using Minibank.Core.Domain.Users.Repositories;
using Minibank.Core.Domain.Users.Services;
using Minibank.Core.Domain.Users.Validators;
using Minibank.Core.Exceptions;
using Moq;
using Xunit;
using ValidationException = FluentValidation.ValidationException;

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
        var userWithEmptyLogin = new UserModel { Email = "email@mail.ru", Id = Guid.NewGuid(), Login = "" };
        var userWithEmptyEmail = new UserModel { Email = "email@mail.ru", Id = Guid.NewGuid(), Login = "" };

        Assert.ThrowsAsync<ValidationException>(() => _userService.CreateAsync(userWithEmptyEmail));
        Assert.ThrowsAsync<ValidationException>(() => _userService.CreateAsync(userWithEmptyLogin));
    }

    [Fact]
    public void AddUser_WithValidLoginAndEmail_ShouldCreateUser()
    {
        var guid = Guid.NewGuid();
        var user = new UserModel { Email = "email@mail.ru", Id = guid, Login = "Login" };

        _fakeUserRepository.Setup(repo => repo.Create(user).Result).Returns(guid);

        Assert.Equal(_userService.CreateAsync(user).Result, guid);
    }

    [Fact]
    public void GetUserById_WithExistingUserModel_ShouldReturnUserModel()
    {
        var guid = Guid.NewGuid();
        var user = new UserModel { Email = "email@mail.ru", Id = guid, Login = "Login" };

        _fakeUserRepository.Setup(repo => repo.Create(user).Result).Returns(guid);
        _fakeUserRepository.Setup(repo => repo.GetById(guid).Result).Returns(user);

        var returnedUser = _userService.GetByIdAsync(guid).Result;
        Assert.Equal(returnedUser, user);
    }

    [Fact]
    public void GetUserById_WithNotExistingUserModel_ShouldThrowException()
    {
        Assert.ThrowsAsync<ObjectNotFoundException>(() => _userService.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public void UpdateUser_WithNotExistingUserModel_ShouldThrowException()
    {
        Assert.ThrowsAsync<ObjectNotFoundException>(() => _userService.UpdateAsync(new UserModel(){Id = Guid.NewGuid()}));
    }

    [Fact]
    public void DeleteUser_WithUserWhoHasActiveBankAccount()
    {
        var guid = Guid.NewGuid();
        _fakeBankAccountRepository.Setup(repo => repo.Any(guid).Result).Returns(false);
        _fakeUserRepository.Setup(repo => repo.GetById(guid).Result).Returns(new UserModel(){Id = guid, Email = "email", Login = "login"});

        Assert.ThrowsAsync<ValidationException>(() => _userService.DeleteAsync(guid));
    }
    
}