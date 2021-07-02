using Application.Interfaces;
using Data.DataAccess;
using Data.Entities;
using Data.RequestModels;
using Data.ResponseModels;
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

        public async Task<ResponseModel<AuthenticateResponse>> Authenticate(AuthenticateRequest model)
        {
            var user = await _context.Users.Where(x => x.Username == model.Username && x.Password == model.Password)
                .Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefaultAsync();

            var data = new List<AuthenticateResponse>();

            if (user == null)
            {
                return new ResponseModel<AuthenticateResponse>(data)
                {
                    Message = "Username or password is incorrect",
                    Total = data.Count,
                    Type = "Authenticate"
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
                    Username = user.Username,
                    Token = token
                });
                return new ResponseModel<AuthenticateResponse>(data)
                {
                    Message = "Successful authentication",
                    Total = data.Count,
                    Type = "Authenticate"
                };
            }
        }
        public User GetById(Guid id)
        {
            return _context.Users.Where(x => x.Id.Equals(id)).Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefault();
        }

        public async Task<ResponseModel<UserResponse>> RegistrationUser(UserRegisterRequest model)
        {
            var username = await _context.Users.Where(x => x.Username.Equals(model.Username)).CountAsync();
            var phone = await _context.Users.Where(x => x.Phone.Equals(model.Phone)).CountAsync();
            var email = await _context.Users.Where(x => x.Email.Equals(model.Email)).FirstOrDefaultAsync();
            var message = "Blank";
            var status = 500;
            bool cemail = _emailHelper.EmailIsValid(model.Email);
            var list = new List<UserResponse>();
            if (username > 0 || phone > 0)
            {
                message = "Username or phone number already exits";
                status = 400;
            }
            else if (email != null)
            {
                message = "Email already exits";
                status = 400;
            }
            else if (cemail == false)
            {
                message = "Email is not validate";
                status = 400;
            }
            else
            {
                message = "Successfully";
                status = 201;
                var userId = Guid.NewGuid();
                var ruser = new User
                {
                    Id = userId,
                    Username = model.Username,
                    Password = model.Password,
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address,
                    MinValue = 0,
                    DayOfBirth = model.DayOfBirth,
                    Image = model.Image
                };
                var userroles = new UserRole
                {
                    UserId = userId,
                    RoleId = Guid.Parse("8239a139-d9d3-454f-8ea9-f3cd792d951b")
                };
                await _context.UserRoles.AddAsync(userroles);
                await _context.Users.AddAsync(ruser);
                await _context.SaveChangesAsync();
                list.Add(new UserResponse
                {
                    Id = ruser.Id,
                    Username = ruser.Username,
                    Name = ruser.Name,
                    Email = ruser.Email,
                    Phone = ruser.Phone,
                    Address = ruser.Address,
                    MinValue = ruser.MinValue,
                    DayOfBirth = ruser.DayOfBirth,
                    Image = ruser.Image
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
                Username = u.Username,
                Email = u.Email,
                Phone = u.Phone,
                DayOfBirth = u.DayOfBirth,
                Address = u.Address,
                Image = u.Image,
                MinValue = u.MinValue
            }).OrderBy(x => x.DayOfBirth).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<UserResponse>(users)
            {
                Total = users.Count,
                Type = "Users"
            };
        }

        public async Task<ResponseModel<UserResponse>> SearchUser(PaginationRequest model, string value)
        {
            var users = await _context.Users.Where(u => u.Name.Contains(value) || u.Username.Contains(value) || u.Email.Contains(value)
            || u.Phone.Contains(value) || u.Address.Contains(value)).Select(u => new UserResponse
            {
                Id = u.Id,
                Name = u.Name,
                Username = u.Username,
                Email = u.Email,
                Phone = u.Phone,
                DayOfBirth = u.DayOfBirth,
                Address = u.Address,
                Image = u.Image,
                MinValue = u.MinValue
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
                    Username = x.Username,
                    Password = model.Password,
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    DayOfBirth = model.DayOfBirth,
                    Address = model.Address,
                    MinValue = model.MinValue,
                    Image = model.Image
                }).FirstOrDefaultAsync();
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                list.Add(new UserResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.Username,
                    Email = user.Email,
                    Phone = user.Phone,
                    DayOfBirth = user.DayOfBirth,
                    Address = user.Address,
                    MinValue = user.MinValue,
                    Image = user.Image
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



    }
}
