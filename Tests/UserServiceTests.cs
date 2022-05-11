using System;
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
    public void CreateUser_WithEmptyLoginOrEmail_ShouldThrowException()
    {
        var userWithEmptyLogin = new UserModel { Id = Guid.NewGuid(), Email = "email@mail.ru", Login = "" };
        var userWithEmptyEmail = new UserModel { Id = Guid.NewGuid(), Email = "", Login = "Login" };

        var loginException =
            Assert.ThrowsAsync<ValidationException>(() => _userService.CreateAsync(userWithEmptyLogin));
        var emailException =
            Assert.ThrowsAsync<ValidationException>(() => _userService.CreateAsync(userWithEmptyEmail));

        Assert.Contains("Login cannot be empty", loginException.Result.Message);
        Assert.Contains("Email cannot be empty", emailException.Result.Message);
        _fakeUnitOfWork.Verify(work => work.SaveChanges(), Times.Never);
    }

    [Fact]
    public void CreateUser_Success_ShouldCreateUser()
    {
        var guid = Guid.NewGuid();
        var user = new UserModel { Email = "email@mail.ru", Id = guid, Login = "Login" };

        _fakeUserRepository.Setup(repo => repo.Create(user).Result).Returns(guid);

        Assert.Equal(_userService.CreateAsync(user).Result, guid);
        _fakeUnitOfWork.Verify(work => work.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetUserById_Success_ShouldReturnUserModel()
    {
        var guid = Guid.NewGuid();
        var user = new UserModel { Email = "email@mail.ru", Id = guid, Login = "Login" };

        _fakeUserRepository.Setup(repo => repo.Create(user).Result).Returns(guid);
        _fakeUserRepository.Setup(repo => repo.GetById(guid).Result).Returns(user);
        var returnedUser = _userService.GetByIdAsync(guid).Result;

        _fakeUnitOfWork.Verify(work => work.SaveChanges(), Times.Never);
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
        Assert.ThrowsAsync<ObjectNotFoundException>(() =>
            _userService.UpdateAsync(new UserModel() { Id = Guid.NewGuid() }));
        _fakeUnitOfWork.Verify(work => work.SaveChanges(), Times.Never);
    }
    
    [Fact]
    public void UpdateUser_Success_ShouldUpdateUser()
    {
        var guid = Guid.NewGuid();
        var user = new UserModel { Email = "email@mail.ru", Id = guid, Login = "Login" };

        _fakeUserRepository.Setup(repo => repo.GetById(guid).Result).Returns(user);
        _userService.UpdateAsync(user);
        
        _fakeUserRepository.Verify(repo => repo.Update(user), Times.Once());
        _fakeUnitOfWork.Verify(work => work.SaveChanges(), Times.Once);
    }

    [Fact]
    public void DeleteUser_WithNotExistingUserModel_ShouldThrowException()
    {
        Assert.ThrowsAsync<ObjectNotFoundException>(() =>
            _userService.UpdateAsync(new UserModel() { Id = Guid.NewGuid() }));
        _fakeUnitOfWork.Verify(work => work.SaveChanges(), Times.Never);
    }

    [Fact]
    public void DeleteUser_WithUserWhoHasActiveBankAccount_ShouldThrowException()
    {
        var guid = Guid.NewGuid();
        _fakeBankAccountRepository.Setup(repo => repo.Any(guid).Result).Returns(true);
        _fakeUserRepository.Setup(repo => repo.GetById(guid).Result)
            .Returns(new UserModel() { Id = guid, Email = "email", Login = "login" });

        var exception =
            Assert.ThrowsAsync<Minibank.Core.Exceptions.ValidationException>(() => _userService.DeleteAsync(guid));
        Assert.Contains("have an active bank accounts", exception.Result.Message);
        _fakeUnitOfWork.Verify(work => work.SaveChanges(), Times.Never);
    }
}