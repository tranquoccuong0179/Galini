using Galini.Models.Paginate;
using Galini.Models.Request.Booking;
using Galini.Models.Response;
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
        public Task<BaseResponse> GetAllBookings(int page, int size);
        public Task<BaseResponse> GetUserBookings(Guid userId, int page, int size);
        public Task<BaseResponse> GetListenerBookings(Guid listenerId, int page, int size);
        public Task<BaseResponse> GetBookingById(Guid bookingId);
        public Task<BaseResponse> UpdateBooking(Guid bookingId, CreateBookingRequest request);
        public Task<BaseResponse> RemoveBooking(Guid bookingId);
    }
}
