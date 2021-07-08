using Data.Entities;
using Data.RequestModels;
using Data.ResponseModels;
using System;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<ResponseModel<AuthenticateResponse>> Authenticate(AuthenticateRequest model);
        Task<ResponseModel<AuthenticateResponse>> AdminAuthenticate(AdminAuthenticateRequest model);

        User GetById(Guid id);


        Task<ResponseModel<UserResponse>> SearchUser(PaginationRequest model, string value);
        Task<ResponseModel<UserResponse>> UpdateUser(Guid id, UserRequest model);
        Task<ResponseModel<UserResponse>> GetAll(PaginationRequest model);
        Task<ResponseModel<UserRoleResponse>> UpdateRoleArtist(Guid id, UserRoleRequest model);
        Task<ResponseModel<UserResponse>> GetUserByEmail(string email);




    }
}
