using Application.Interfaces;
using Data.DataAccess;
using Data.Entities;
using Data.RequestModels;
using Data.ResponseModels;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FireSharp.Config;
using FireSharp.Interfaces;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utilities.AppSettings;
using Utilities.Helpers;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly BeauDeeProjectContext _context;
        private readonly AppSetting _appSetting;
        private readonly EmailHepler _emailHelper;

        public UserService(BeauDeeProjectContext context, IOptions<AppSetting> appSetting, IOptions<EmailHepler> emailHepler)
        {
            _context = context;
            _appSetting = appSetting.Value;
            _emailHelper = emailHepler.Value;
        }


        private async Task<AuthenticateUserResponse> VerifiedFireBaseToken(string firebaseToken)
        {
            try
            {
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(firebaseToken);
                var result = new AuthenticateUserResponse
                {
                    Uid = decodedToken.Uid,
                    Name = decodedToken.Claims.GetValueOrDefault("name").ToString(),
                    Email = decodedToken.Claims.GetValueOrDefault("email").ToString(),
                };
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<ResponseModel<AuthenticateResponse>> Authenticate(AuthenticateRequest model)
        {
            //initFireBase();
            var data = new List<AuthenticateResponse>();
            var message = "blank";
            var status = 500;
            AuthenticateUserResponse auser = await VerifiedFireBaseToken(model.Token);
            if (auser == null)
            {

                message = "Token is incorrect";
                status = 404;
            }
            else
            {
                message = "Successful authentication";
                status = 200;
                var user = await _context.Users.Where(x => x.Uid == auser.Uid).FirstOrDefaultAsync();
                if (user == null)
                {
                    var list = new List<UserResponse>();
                    message = "Successfully";
                    status = 201;
                    var userId = Guid.NewGuid();
                    var userroles = new UserRole
                    {
                        UserId = userId,
                        RoleId = Guid.Parse("f914c465-84d4-4a48-819e-31692a9fc983")
                    };
                    var ruser = new User
                    {
                        Id = userId,
                        Uid = auser.Uid,
                        Name = auser.Name,
                        Email = auser.Email,
                        Phone = null,
                        Address = null,
                        MinValue = null,
                        DayOfBirth = null,
                        Image = null,
                        Password = Guid.NewGuid().ToString()

                    };

                    await _context.UserRoles.AddAsync(userroles);
                    await _context.Users.AddAsync(ruser);
                    await _context.SaveChangesAsync();


                }
                var cuser = await _context.Users.Where(x => x.Uid == auser.Uid).Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefaultAsync();
                var token = generateJwtToken(cuser);
                data.Add(new AuthenticateResponse
                {
                    Id = cuser.Id,
                    Email = cuser.Email,
                    Name = cuser.Name,
                    Role = cuser.UserRoles.Select(x => x.Role.Name).ToArray(),
                    Uid = cuser.Uid,
                    Token = token
                });
            }
            return new ResponseModel<AuthenticateResponse>(data)
            {
                Message = message,
                Status = status,
                Total = data.Count,
                Type = "Authenticate"
            };

        }

        public async Task<ResponseModel<AuthenticateResponse>> AdminAuthenticate(AdminAuthenticateRequest model)
        {
            var user = await _context.Users.Where(x => x.Email == model.Email && x.Password == model.Password)
                .Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefaultAsync();
            var data = new List<AuthenticateResponse>();

            if (user == null)
            {
                return new ResponseModel<AuthenticateResponse>(data)
                {
                    Message = "Email or password is incorrect",
                    Total = data.Count,
                    Type = "AdminAuthenticate"
                };
            }
            else
            {
                var token = generateJwtToken(user);
                data.Add(new AuthenticateResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Role = user.UserRoles.Select(x => x.Role.Name).ToArray(),
                    Uid = user.Uid,
                    Token = token
                });
            }
            return new ResponseModel<AuthenticateResponse>(data)
            {
                Message = "Successful authenticate",
                Total = data.Count,
                Type = "AdminAuthenticate"
            };
        }
           





public User GetById(Guid id)
{
    return _context.Users.Where(x => x.Id.Equals(id)).Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefault();
}



private string generateJwtToken(User user)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_appSetting.Secret);
    var id = user.Id.ToString();
    var roles = String.Join(",", user.UserRoles.Select(x => x.Role.Name));

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()), new Claim("roles", String.Join(",", user.UserRoles.Select(x => x.Role.Name))) }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    var t = tokenHandler.WriteToken(token);
    return tokenHandler.WriteToken(token);
}
public async Task<ResponseModel<UserResponse>> GetAll(PaginationRequest model)
{

    var users = await _context.Users.Select(u => new UserResponse
    {
        Id = u.Id,
        Name = u.Name,
        Uid = u.Uid,
        Email = u.Email,
        Phone = u.Phone,
        DayOfBirth = u.DayOfBirth,
        Address = u.Address,
        Image = u.Image,
        MinValue = u.MinValue,
        Role = _context.UserRoles.Where(x => x.UserId.Equals(u.Id)).Select(x => x.Role.Name).FirstOrDefault(),
    }).OrderBy(x => x.DayOfBirth).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
    return new ResponseModel<UserResponse>(users)
    {
        Total = users.Count,
        Type = "Users"
    };
}

public async Task<ResponseModel<UserResponse>> SearchUser(PaginationRequest model, string value)
{
    var users = await _context.Users.Where(u => u.Name.Contains(value) || u.Email.Contains(value)
    || u.Phone.Contains(value) || u.Address.Contains(value)).Select(u => new UserResponse
    {
        Id = u.Id,
        Name = u.Name,
        Uid = u.Uid,
        Email = u.Email,
        Phone = u.Phone,
        DayOfBirth = u.DayOfBirth,
        Address = u.Address,
        Image = u.Image,
        MinValue = u.MinValue,
        Role = _context.UserRoles.Where(x => x.UserId.Equals(u.Id)).Select(x => x.Role.Name).FirstOrDefault(),
    }).OrderBy(x => x.DayOfBirth).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
    return new ResponseModel<UserResponse>(users)
    {
        Total = users.Count,
        Type = "Users"
    };
}
public async Task<ResponseModel<UserResponse>> UpdateUser(Guid id, UserRequest model)
{

    var list = new List<UserResponse>();
    var message = "Blank";
    var status = 500;
    bool cemail = _emailHelper.EmailIsValid(model.Email);
    if (cemail == false)
    {
        message = "Email is not validate";
        status = 400;
    }
    else
    {
        message = "Successfully";
        status = 201;
        var user = await _context.Users.Where(x => x.Id.Equals(id)).Select(x => new User
        {
            Id = id,
            Name = model.Name,
            Email = model.Email,
            Uid = x.Uid,
            Phone = model.Phone,
            DayOfBirth = model.DayOfBirth,
            Address = model.Address,
            MinValue = model.MinValue,
            Image = model.Image,
            Password = x.Password
        }).FirstOrDefaultAsync();
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        list.Add(new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Uid = user.Uid,
            Email = user.Email,
            Phone = user.Phone,
            DayOfBirth = user.DayOfBirth,
            Address = user.Address,
            MinValue = user.MinValue,
            Image = user.Image,
            Role = _context.UserRoles.Where(x => x.UserId.Equals(user.Id)).Select(x => x.Role.Name).FirstOrDefault(),
        });
    }
    return new ResponseModel<UserResponse>(list)
    {
        Message = message,
        Status = status,
        Total = list.Count,
        Type = "User"
    };

}

public async Task<ResponseModel<UserResponse>> GetUserByEmail(string email)
{
    var user = await _context.Users.Where(u => u.Email.Equals(email)).Select(u => new UserResponse
    {
        Id = u.Id,
        Name = u.Name,
        Uid = u.Uid,
        Email = u.Email,
        Phone = u.Phone,
        DayOfBirth = u.DayOfBirth,
        Address = u.Address,
        Image = u.Image,
        MinValue = u.MinValue,
        Role = _context.UserRoles.Where(x => x.UserId.Equals(u.Id)).Select(x => x.Role.Name).FirstOrDefault(),
    }).ToListAsync();
    return new ResponseModel<UserResponse>(user)
    {
        Total = user.Count,
        Type = "User"
    };
}
    }
}
