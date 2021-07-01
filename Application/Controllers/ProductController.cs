using System;
using System.Threading.Tasks;
using Application.Configurations.Middleware;
using Application.Interfaces;
using Data.RequestModels;
using Data.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthentication.Controllers
{
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("Products/GetAll")]
        public async Task<ResponseModel<ProductResponse>> GetAllProduct([FromQuery] PaginationRequest model)
        {
            return await _service.GetAll(model);
        }
        [HttpGet]
        [Route("Products/Search/{value}")]
        public async Task<ResponseModel<ProductResponse>> SearchProduct([FromQuery] PaginationRequest model, string value)
        {
            return await _service.SearchProduct(model, value);
        }
        [HttpGet]
        [Route("Products/Search/{min}/{max}")]
        public async Task<ResponseModel<ProductResponse>> SearchProductByPrice([FromQuery] PaginationRequest model, decimal min, decimal max)
        {
            return await _service.SearchProductByPrice(model, min, max);
        }
        [HttpPost]
        [Route("Products/Create")]
        public async Task<ResponseModel<ProductResponse>> CreateProduct(ProductRequest model)
        {
            return await _service.CreateProduct(model);
        }
        [HttpPut]
        [Route("Products/Update")]
        public async Task<ResponseModel<ProductResponse>> UpdateProduct(Guid id, ProductRequest model)
        {
            return await _service.UpdateProduct(id, model);
        }
    }
}
