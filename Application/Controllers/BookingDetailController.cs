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
    public class BookingDetailController : ControllerBase
    {
        private readonly IBookingDetailService _service;
        public BookingDetailController(IBookingDetailService service)
        {
            _service = service;
        }
        [Authorize("Admin")]
        [HttpGet]
        [Route("BookingDetails/GetAll")]
        public async Task<ResponseModel<BookingDetailResponse>> GetAllBookingDetail([FromQuery] PaginationRequest model)
        {
            return await _service.GetAll(model);
        }

        [Authorize("Admin")]
        [HttpPost]
        [Route("BookingDetails/Create")]
        public async Task<ResponseModel<BookingDetailResponse>> CreateBookingDetail(BookingDetailRequest model)
        {
            return await _service.CreateBookingDetail(model);
        }


    }
}
