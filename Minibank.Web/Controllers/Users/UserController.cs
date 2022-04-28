using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domain.Users;
using Minibank.Core.Domain.Users.Services;
using Minibank.Web.Controllers.Users.Dto;

namespace Minibank.Web.Controllers.Users
{
    [ApiController]
    [Route("api/v1/minibank/[controller]/[action]")]
    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<UserModel> GetById(Guid id)
        {
            var model = await _userService.GetByIdAsync(id);

            return new UserModel
            {
                Id = model.Id,
                Login = model.Login,
                Email = model.Email,
            };
        }

        [HttpGet]
        public async Task<IEnumerable<UserModel>> GetAll()
        {
            var users = await _userService.GetAllAsync();

            return users.Select(model => new UserModel
            {
                Id = model.Id,
                Login = model.Login,
                Email = model.Email,
            });
        }

        [HttpPost]
        public async Task<Guid> Create(UserDto model)
        {
            return await _userService.CreateAsync(new UserModel
            {
                Login = model.Login,
                Email = model.Email
            });
        }

        [HttpPut("{id}")]
        public async Task Update(Guid id, UserDto model)
        {
            await _userService.UpdateAsync(new UserModel
            {
                Id = id,
                Login = model.Login,
                Email = model.Email
            });
        }

        [HttpDelete("{id}")]
        public async Task DeleteById(Guid id)
        {
            await _userService.DeleteAsync(id);
        }
    }
}