using Application.Interfaces;
using Data.RequestModels;
using Data.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("Users/Authenticate")]
        public async Task<ResponseModel<AuthenticateResponse>> Authenticate(AuthenticateRequest model)
        {
            return await _userService.Authenticate(model);
        }
        [HttpPost]
        [Route("Users/Register")]
        public async Task<ResponseModel<UserResponse>> RegistrationUser(UserRegisterRequest model)
        {
            return await _userService.RegistrationUser(model);
        }
        [HttpGet]
        [Route("Users/GetAll")]
        public async Task<ResponseModel<UserResponse>> GetAllUser([FromQuery] PaginationRequest model)
        {
            return await _userService.GetAll(model);
        }
        [HttpGet]
        [Route("Users/Search/{value}")]
        public async Task<ResponseModel<UserResponse>> SearchUser([FromQuery] PaginationRequest model, string value)
        {
            return await _userService.SearchUser(model, value);
        }


        [HttpPut]
        [Route("Users/Update")]
        public async Task<ResponseModel<UserResponse>> UpdateUser(Guid id, UserRequest model)
        {
            return await _userService.UpdateUser(id, model);
        }
    }
}
