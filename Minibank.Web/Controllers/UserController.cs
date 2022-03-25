using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Services;
using Minibank.Web.Controllers.Users.Dto;

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
        public UserModel GetById(Guid id)
        {
            var model = _userService.GetById(id);

            return new UserModel
            {
                Id = model.Id,
                Login = model.Login,
                Email = model.Email,
            };
        }
        
        [HttpGet]
        public IEnumerable<UserModel> GetAll()
        {
            return _userService.GetAll()
                .Select(model => new UserModel
                {
                    Id = model.Id,
                    Login = model.Login,
                    Email = model.Email,
                });
        }
        
        [HttpPost]
        public Guid Create(UserDto model)
        {
            return _userService.Create(new UserModel
            {
                Login = model.Login,
                Email = model.Email
            });
            
        }
        
        [HttpPut("{id}")]
        public void Update(Guid id, UserDto model)
        {
            _userService.Update(new UserModel
            {
                Id = id,
                Login = model.Login,
                Email = model.Email
            });
        }
        
        [HttpDelete("{id}")]
        public void DeleteById(Guid id)
        {
            _userService.Delete(id);
        }
    }
}