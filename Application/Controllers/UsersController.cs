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
        [HttpGet]
        [Route("Users/Get/{email}")]
        public async Task<ResponseModel<UserResponse>> GetUserByEmail(String email)
        {
            return await _userService.GetUserByEmail(email);
        }

        [HttpPut]
        [Route("Users/Update")]
        public async Task<ResponseModel<UserResponse>> UpdateUser(Guid id, UserRequest model)
        {
            return await _userService.UpdateUser(id, model);
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
