﻿using Data.Entities;
using Data.RequestModels;
using Data.ResponseModels;
using System;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<ResponseModel<AuthenticateResponse>> Authenticate(AuthenticateRequest model);
        User GetById(Guid id);

        //Task<ResponseModel<UserResponse>> RegistrationUser(UserRegisterRequest model);

        Task<ResponseModel<UserResponse>> SearchUser(PaginationRequest model, string value);
        Task<ResponseModel<UserResponse>> UpdateUser(Guid id, UserRequest model);
        Task<ResponseModel<UserResponse>> GetAll(PaginationRequest model);



    }
}
