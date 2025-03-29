using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.Booking;
using Galini.Models.Payload.Request.Premium;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Booking;
using Galini.Models.Payload.Response.FriendShip;
using Galini.Models.Payload.Response.Premium;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Implement
{
    public class BookingService : BaseService<BookingService>, IBookingService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public BookingService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<BookingService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<BaseResponse> CreateBooking(CreateBookingRequest request, Guid workShiftId)
        {
            Guid? id = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id) && x.IsActive);
            if (account == null)
            {
                _logger.LogWarning($"Không tìm thấy tài khoản có Id {id} .");
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Tài khoản không tồn tại",
                    data = null
                };
            }

            var workShift = await _unitOfWork.GetRepository<WorkShift>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(workShiftId) && x.IsActive);

            if (workShift == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Ca làm việc không tồn tại",
                    data = null
                };
            }

            var listener = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(workShift.AccountId) && x.IsActive);

            if (listener == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Tham vấn viên không tồn tại",
                    data = null
                };
            }

            var exitBooking = await _unitOfWork.GetRepository<Booking>().SingleOrDefaultAsync(
                predicate: x => x.WorkShiftId.Equals(workShift.Id) && x.IsActive && DateOnly.FromDateTime(x.Date) == DateOnly.FromDateTime(request.Date));
         

            if (exitBooking != null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Ca làm việc này đã bị đặt",
                    data = null
                };
            }

            bool checkDay = request.Date.ToString("dddd") == workShift.Day;
            if (!checkDay)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Ca làm việc " + workShiftId + " không tồn tại vào thứ " + request.Date.ToString("dddd"),
                    data = null
                };
            }

            if (request.Status != BookingEnum.Upcoming)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Status không hợp lệ, chỉ chấp nhận 'Upcoming' khi create",
                    data = null
                };
            }

            var booking = _mapper.Map<CreateBookingRequest, Booking>(request);
            booking.ListenerId = listener.Id;
            booking.WorkShiftId = workShiftId;
            booking.UserId = account.Id;

            await _unitOfWork.GetRepository<Booking>().InsertAsync(booking);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                var data = _mapper.Map<CreateBookingResponse>(booking);
                data.UserName = account.FullName;
                data.ListenerName = listener.FullName;
                data.Time = $"{workShift.StartTime:HH:mm} - {workShift.EndTime:HH:mm}"; 
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Đặt lịch thành công",
                    data = data
                };
            }
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Đặt lịch thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> GetAllBookings(int page, int size, BookingEnum? status)
        {
            if (page < 1 || size < 1)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Page hoặc size không hợp lệ.",
                    data = null
                };
            }

            var bookings = await _unitOfWork.GetRepository<Booking>().GetPagingListAsync(
                selector: b => new CreateBookingResponse
                {                 
                    UserName = b.User.FullName, 
                    ListenerName = b.WorkShift.Account.FullName, 
                    Time = $"{b.WorkShift.StartTime} - {b.WorkShift.EndTime}", 
                    Status = ParseBookingStatus(b.Status),
                    Date = b.Date
                },
                predicate: b => b.IsActive && (!status.HasValue || b.Status == status.ToString()),
                orderBy: q => q.OrderByDescending(b => b.CreateAt),
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách đặt lịch thành công",
                data = bookings
            };
        }

        public async Task<BaseResponse> GetBookingById(Guid bookingId)
        {
            var booking = await _unitOfWork.GetRepository<Booking>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(bookingId) && x.IsActive);

            if (booking == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm danh sách đặt lịch với ID này",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách đặt lịch thành công",
                data = new CreateBookingResponse
                {
                    UserName = booking.User.FullName,
                    ListenerName = booking.WorkShift.Account.FullName,
                    Time = $"{booking.WorkShift.StartTime} - {booking.WorkShift.EndTime}",
                    Status = ParseBookingStatus(booking.Status),
                    Date = booking.Date
                }
            };
        }

        public async Task<BaseResponse> GetUserBookings(int page, int size, BookingEnum? status)
        {
            Guid? id = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id) && x.IsActive);
            if (account == null)
            {
                _logger.LogWarning($"Không tìm thấy tài khoản có Id {id} .");
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Tài khoản không tồn tại",
                    data = null
                };
            }

            if (page < 1 || size < 1)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Page hoặc size không hợp lệ.",
                    data = null
                };
            }

            var bookings = await _unitOfWork.GetRepository<Booking>().GetPagingListAsync(
                selector: b => new GetBookingResponse 
                {
                    Id = b.Id,
                    ListenerId = b.ListenerId,
                    UserId = b.UserId,
                    UserName = b.User.FullName,
                    ListenerName = b.WorkShift.Account.FullName,
                    Time = $"{b.WorkShift.StartTime} - {b.WorkShift.EndTime}",
                    Status = ParseBookingStatus(b.Status),
                    Date = b.Date
                },
                predicate: b => b.IsActive && (!status.HasValue || b.Status == status.ToString()) && (b.UserId == account.Id || b.ListenerId == account.Id),
                orderBy: q => q.OrderByDescending(b => b.CreateAt),
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách đặt lịch thành công",
                data = bookings
            };
        }

        public async Task<BaseResponse> RemoveBooking(Guid bookingId)
        {
            var booking = await _unitOfWork.GetRepository<Booking>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(bookingId) && x.IsActive);

            if (booking == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy danh sách đặt lịch với ID này",
                    data = null
                };
            }

            booking.IsActive = false;
            booking.DeleteAt = TimeUtil.GetCurrentSEATime();
            booking.UpdateAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<Booking>().UpdateAsync(booking);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa đặt lịch thành công",
                    data = isSuccessfully
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa đặt lịch thất bại",
                data = isSuccessfully
            };
        }

        public async Task<BaseResponse> UpdateBooking(Guid bookingId, UpdateBookingRequest request)
        {
            var booking = await _unitOfWork.GetRepository<Booking>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(bookingId) && x.IsActive);

            if (booking == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy danh sách đặt lịch với ID này",
                    data = null
                };
            }

            booking = _mapper.Map(request, booking);

            _unitOfWork.GetRepository<Booking>().UpdateAsync(booking);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Cập nhật danh sách đặt lịch thành công",
                    data = new CreateBookingResponse
                    {
                        UserName = booking.User.FullName,
                        ListenerName = booking.WorkShift.Account.FullName,
                        Time = $"{booking.WorkShift.StartTime} - {booking.WorkShift.EndTime}",
                        Status = ParseBookingStatus(booking.Status),
                        Date = booking.Date
                    }
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Cập nhật danh sách đặt lịch thất bại",
                data = null
            };
        }

        private static BookingEnum ParseBookingStatus(string status)
        {
            return Enum.TryParse<BookingEnum>(status, out var statusEnum) ? statusEnum : BookingEnum.None;
        }
    }
}
