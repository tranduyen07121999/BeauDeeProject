using Data.RequestModels;
using Data.ResponseModels;
using System;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBookingDetailService
    {
        Task<ResponseModel<BookingDetailResponse>> GetAll(PaginationRequest model);
        Task<ResponseModel<BookingDetailResponse>> CreateBookingDetail(BookingDetailRequest model);
    }
}
