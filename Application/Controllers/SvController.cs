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
        [HttpGet]
        [Route("Services/GetAll")]
        public async Task<ResponseModel<ServiceResponse>> GetAllProduct([FromQuery] PaginationRequest model)
        {
            return await _service.GetAll(model);
        }
        [HttpGet]
        [Route("Services/Search/{value}")]
        public async Task<ResponseModel<ServiceResponse>> SearchService([FromQuery] PaginationRequest model, string value)
        {
            return await _service.SearchService(model, value);
        }
        [HttpPost]
        [Route("Services/Create")]
        public async Task<ResponseModel<ServiceResponse>> CreateService(ServiceRequest model)
        {
            return await _service.CreateService(model);
        }
        [HttpPut]
        [Route("Services/Update")]
        public async Task<ResponseModel<ServiceResponse>> UpdateService([FromQuery] Guid id, ServiceRequest model)
        {
            return await _service.UpdateService(id, model);
        }
    }
}
