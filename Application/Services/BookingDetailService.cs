using Application.Interfaces;
using Data.DataAccess;
using Data.Entities;
using Data.RequestModels;
using Data.ResponseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BookingDetailService : IBookingDetailService
    {
        private readonly BeauDeeProjectContext _context;
        public BookingDetailService(BeauDeeProjectContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<BookingDetailResponse>> GetAll(PaginationRequest model)
        {
            var bookingdetails = await _context.BookingDetails.Select(p => new BookingDetailResponse
            {
                ServiceId = p.ServiceId,
                UserId = p.UserId,
                ProductId = p.ProductId,
                BookingId = p.BookingId
            }).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<BookingDetailResponse>(bookingdetails)
            {
                Total = bookingdetails.Count,
                Type = "BookingDetails"
            };
        }
        public async Task<ResponseModel<BookingDetailResponse>> CreateBookingDetail(BookingDetailRequest model)
        {

            var booking = await _context.Bookings.Where(x => x.UserId.Equals(model.UserId)).FirstOrDefaultAsync();
            if (booking == null)
            {
                booking = new Booking
                {
                    Id = Guid.NewGuid(),
                    UserId = model.UserId
                };
                await _context.Bookings.AddAsync(booking);
            }

            var bookingdetails = new BookingDetail
            {
                ServiceId = model.ServiceId,
                UserId = booking.UserId,
                ProductId = model.ProductId,
                BookingId = booking.Id
            };
            await _context.BookingDetails.AddAsync(bookingdetails);
            await _context.SaveChangesAsync();
            var list = new List<BookingDetailResponse>();
            list.Add(new BookingDetailResponse
            {
                ServiceId = bookingdetails.ServiceId,
                UserId = bookingdetails.UserId,
                BookingId = bookingdetails.BookingId,
                ProductId = bookingdetails.ProductId
            });
            return new ResponseModel<BookingDetailResponse>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "BookingDetail"
            };
        }

    }
}
