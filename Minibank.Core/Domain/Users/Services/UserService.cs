using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Minibank.Core.Domain.BankAccounts.Repositories;
using Minibank.Core.Domain.Users.Repositories;
using Minibank.Core.Exceptions;
using ValidationException = Minibank.Core.Exceptions.ValidationException;

namespace Minibank.Core.Domain.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IValidator<UserModel> _userValidator;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository,
            IBankAccountRepository bankAccountRepository, IValidator<UserModel> userValidator)
        {
            _userRepository = userRepository;
            _bankAccountRepository = bankAccountRepository;
            _userValidator = userValidator;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserModel> GetById(Guid id)
        {
            var user = _userRepository.GetById(id);

            if (user is null)
            {
                throw new ObjectNotFoundException($"User with id = {id} does not exist");
            }

            return await user;
        }

        public async Task<IEnumerable<UserModel>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<Guid> Create(UserModel userModel)
        {
            await _userValidator.ValidateAndThrowAsync(userModel);

            var user = _userRepository.Create(userModel);
            _unitOfWork.SaveChanges();
            return await user;
        }

        public async Task Update(UserModel userModel)
        {
            await _userRepository.Update(userModel);
            _unitOfWork.SaveChanges();
        }

        public async Task Delete(Guid id)
        {
            var user = _userRepository.GetById(id);

            if (user is null)
            {
                throw new ObjectNotFoundException($"User with id = {id} doesnt exists");
            }

            if (await _bankAccountRepository.Any(id))
            {
                throw new ValidationException($"User with id = {id} have an active bank accounts");
            }

            await _userRepository.Delete(id);
            _unitOfWork.SaveChanges();
        }
    }
}