using Application.Configurations.Middleware;
using Application.Interfaces;
using Data.RequestModels;
using Data.ResponseModels;
using FirebaseAdmin.Auth;
using Google.Apis.Util;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthentication.Controllers
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
        [Route("Users/Admin/Authenticate")]
        public async Task<ResponseModel<AuthenticateResponse>> AdminAuthenticate(AdminAuthenticateRequest model)
        {
            return await _userService.AdminAuthenticate(model);
        }
        [Authorize("Admin", "Customer", "Artist")]
        [HttpGet]
        [Route("Users/GetAll")]
        public async Task<ResponseModel<UserResponse>> GetAllUser([FromQuery] PaginationRequest model)
        {
            return await _userService.GetAll(model);
        }
        [Authorize("Admin", "Customer", "Artist")]
        [HttpGet]
        [Route("Users/Search/{value}")]
        public async Task<ResponseModel<UserResponse>> SearchUser([FromQuery] PaginationRequest model, [FromRoute] string value)
        {
            return await _userService.SearchUser(model, value);
        }
        [Authorize("Admin", "Customer", "Artist")]
        [HttpGet]
        [Route("Users/Get/{uid}")]
        public async Task<ResponseModel<UserResponse>> GetUserByUid([FromRoute] String uid)
        {
            return await _userService.GetUserByUid(uid);
        }
        [Authorize("Admin", "Customer", "Artist")]
        [HttpPut]
        [Route("Users/Update")]
        public async Task<ResponseModel<UserResponse>> UpdateUser(Guid id, UserRequest model)
        {
            return await _userService.UpdateUser(id, model);
        }
        [Authorize("Admin")]
        [HttpPut]
        [Route("Users/Update/Service")]
        public async Task<ResponseModel<UserRoleResponse>> UpdateRoleArtist(Guid id, UserRoleRequest model)
        {
            return await _userService.UpdateRoleArtist(id, model);
        }
        [Authorize("Admin")]
        [HttpPut]
        [Route("Users/Update/Role")]
        public async Task<ResponseModel<UserRoleResponse>> UpdateUserRole(Guid id, UserRoleUpdateRequest model)
        {
            return await _userService.UpdateUserRole(id, model);
        }
        [HttpGet("test")]

        public async Task<IActionResult> GetTokenAsync()
        {
            try
            {
                var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
                var token = await auth.CreateCustomTokenAsync("JHD1ngCtq3fqrnuyYF3rbYHzUB03");
                var idToken = await SignInWithCustomTokenAsync(token);
                var detoken = await auth.VerifyIdTokenAsync(idToken);
                return Ok(idToken);
            }
            catch (Exception)
            {

                throw;
            }

        }
        private static async Task<string> SignInWithCustomTokenAsync(string customToken)
        {
            string apiKey = "AIzaSyCWlGnQvkiS3-iCYSlia_v2VAILriLSnSQ"; // see above where to get it. 
            var rb = new Google.Apis.Requests.RequestBuilder
            {
                Method = Google.Apis.Http.HttpConsts.Post,
                BaseUri = new Uri($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithCustomToken")
            };
            rb.AddParameter(RequestParameterType.Query, "key", apiKey);
            var request = rb.CreateRequest();
            var jsonSerializer = Google.Apis.Json.NewtonsoftJsonSerializer.Instance;
            var payload = jsonSerializer.Serialize(new
            {
                token = customToken,
                returnSecureToken = true,
            });
            request.Content = new StringContent(payload, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var response = await client.SendAsync(request);
                //response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var parsed = JObject.Parse(json);
                return parsed.GetValue("idToken").ToString();
            }
        }
    }
}
