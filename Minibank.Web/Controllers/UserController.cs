using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Services;
using Minibank.Web.Dto;

namespace Minibank.Web.Controllers
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
        public UserModel Get(Guid id)
        {
            var model = _userService.Get(id);

            return new UserModel
            {
                Id = model.Id,
                Login = model.Login,
                Email = model.Email
            };
        }
        
        [HttpGet]
        public IEnumerable<UserModel> GetAll()
        {
            return _userService.GetAll()
                .Select(it => new UserModel
                {
                    Id = it.Id,
                    Login = it.Login,
                    Email = it.Email
                });
        }
        
        [HttpPost]
        public void Create(UserDto model)
        {
            _userService.Create(new UserModel
            {
                Login = model.Login,
                Email = model.Email
            });
        }
        
        [HttpPut("/{id}")]
        public void Update(UserModel model)
        {
            _userService.Update(model);
        }
        
        [HttpDelete("/{id}")]
        public void Delete(Guid id)
        {
            _userService.Delete(id);
        }
    }
}