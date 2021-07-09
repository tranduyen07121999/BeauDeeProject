using Application.Configurations.Middleware;
using Application.Interfaces;
using Data.RequestModels;
using Data.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthentication.Controllers
{
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly ISvService _service;
        public ServicesController(ISvService service)
        {
            _service = service;
        }

        [Authorize("Admin")]
        [HttpGet]
        [Route("Services/GetAll")]
        public async Task<ResponseModel<ServiceResponse>> GetAllProduct([FromQuery] PaginationRequest model)
        {
            return await _service.GetAll(model);
        }
        [Authorize("Admin")]
        [HttpGet]
        [Route("Services/Search/{value}")]
        public async Task<ResponseModel<ServiceResponse>> SearchService([FromQuery] PaginationRequest model, string value)
        {
            return await _service.SearchService(model, value);
        }
        [Authorize("Admin")]
        [HttpPost]
        [Route("Services/Create")]
        public async Task<ResponseModel<ServiceResponse>> CreateService(ServiceRequest model)
        {
            return await _service.CreateService(model);
        }
        [Authorize("Admin")]
        [HttpPut]
        [Route("Services/Update")]
        public async Task<ResponseModel<ServiceResponse>> UpdateService(Guid id, ServiceRequest model)
        {
            return await _service.UpdateService(id, model);
        }
        [Authorize("Admin")]
        [HttpPut]
        [Route("Services/Disable")]
        public async Task<ResponseModel<ServiceResponse>> DisableService(Guid id)
        {
            return await _service.DisableService(id);
        }
    }
}
