using Galini.Models.Enum;
using Galini.Models.Paginate;
using Galini.Models.Payload.Request.Booking;
using Galini.Models.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    public interface IBookingService
    {
        public Task<BaseResponse> CreateBooking(CreateBookingRequest request, Guid workShiftId);
        public Task<BaseResponse> GetAllBookings(int page, int size, BookingEnum? status);
        public Task<BaseResponse> GetUserBookings(int page, int size, BookingEnum? status);
        public Task<BaseResponse> GetBookingById(Guid bookingId);
        public Task<BaseResponse> UpdateBooking(Guid bookingId, UpdateBookingRequest request);
        public Task<BaseResponse> RemoveBooking(Guid bookingId);
    }
}
