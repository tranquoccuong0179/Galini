﻿using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.FriendShip;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Account;
using Galini.Models.Payload.Response.FriendShip;
using Galini.Models.Utils;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;

namespace Galini.Services.Implement
{
    public class FriendShipService : BaseService<FriendShipService>, IFriendShipService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
    public FriendShipService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<FriendShipService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
    {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
    }

        public async Task<BaseResponse> CreateFriendShip(Guid userId, Guid friendId)
        {
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: x => x.IsActive && x.Id.Equals(userId));

            if (account == null)
            {
                _logger.LogWarning($"Không tìm thấy tài khoản có Id {userId} .");
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Tài khoản không tồn tại",
                    data = null
                };
            }

            var account2 = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: x => x.IsActive && x.Id.Equals(friendId));

            if (account2 == null)
            {
                _logger.LogWarning($"Không tìm thấy tài khoản có Id {friendId} .");
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Tài khoản không tồn tại",
                    data = null
                };
            }

            var friendShip = new FriendShip
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FriendId = friendId,
                Status = FriendShipEnum.Request.ToString(),
                IsActive = true,
                CreateAt = TimeUtil.GetCurrentSEATime(),
                UpdateAt = TimeUtil.GetCurrentSEATime(),
            };

            await _unitOfWork.GetRepository<FriendShip>().InsertAsync(friendShip);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Tạo lời mời kết bạn thành công",
                    data = _mapper.Map<CreateFriendShipResponse>(friendShip)
                };
            }
            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Tạo lời mời kết bạn thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> GetAllFriendShip(int page, int size, string? status, bool? sortByStatus)
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

            var friendShip = await _unitOfWork.GetRepository<FriendShip>().GetPagingListAsync(
                selector: a => _mapper.Map<CreateFriendShipResponse>(a),
                predicate: a => a.IsActive && (string.IsNullOrEmpty(status) || a.Status.Equals(status)),
                orderBy: l => sortByStatus.HasValue ? (sortByStatus.Value ? l.OrderBy(l => l.Status) : l.OrderByDescending(l => l.Status)) : l.OrderBy(l => l.CreateAt),
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin mối quan hệ thành công",
                data = friendShip
            };
        }

        public async Task<BaseResponse> GetFriendList(int page, int size)
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

            var friendships = await _unitOfWork.GetRepository<FriendShip>().GetListAsync(
                predicate: f => (f.UserId == account.Id || f.FriendId == account.Id) && f.Status == "Accepted");

            if (friendships == null || !friendships.Any())
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Không tìm thấy bạn bè nào.",
                    data = null
                };
            }

            var friendIds = new HashSet<Guid>(friendships.Select(f => f.UserId == account.Id ? f.FriendId : f.UserId).ToList());

            var friends = await _unitOfWork.GetRepository<Account>().GetPagingListAsync(
                predicate: a => friendIds.Contains(a.Id),
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách bạn bè thành công",
                data = friends
            };
        }

        public async Task<BaseResponse> GetFriendShipByAccountIdAndStatus(Guid accountId, FriendShipEnum status, int page, int size)
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

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.IsActive && x.Id.Equals(accountId));

            if (account == null)
            {
                _logger.LogWarning($"Không tìm thấy tài khoản có Id {accountId} .");
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Tài khoản không tồn tại",
                    data = null
                };
            }

            var friendShip = await _unitOfWork.GetRepository<FriendShip>().GetPagingListAsync(
                selector: a =>  new GetFriendShipResponse
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    FriendId = a.FriendId,
                    FriendFullName = a.Friend.FullName,
                    FriendDateOfBirth = a.Friend.DateOfBirth,
                    FriendGender = a.Friend.Gender,
                    FriendAvatarUrl = a.Friend.AvatarUrl,
                    Status = a.Status
                },
                predicate: a => a.IsActive && a.UserId == accountId && a.Status == status.ToString(),
                include: query => query.Include(a => a.Friend),
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin mối quan hệ thành công",
                data = friendShip
            };
        }

        public async Task<BaseResponse> GetFriendByAccountId(Guid accountId, int page, int size)
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

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.IsActive && x.Id.Equals(accountId));

            if (account == null)
            {
                _logger.LogWarning($"Không tìm thấy tài khoản có Id {accountId} .");
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Tài khoản không tồn tại",
                    data = null
                };
            }

            var friendShip = await _unitOfWork.GetRepository<FriendShip>().GetPagingListAsync(
                selector: a => _mapper.Map<CreateFriendShipResponse>(a),
                predicate: a => a.IsActive && (a.UserId == accountId || a.FriendId == accountId) && a.Status == FriendShipEnum.Accepted.ToString(),
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin mối quan hệ thành công",
                data = friendShip
            };
        }

        public async Task<BaseResponse> GetFriendShipById(Guid friendShipId)
        {
            var friendShip = await _unitOfWork.GetRepository<FriendShip>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(friendShipId) && x.IsActive);

            if (friendShip == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy mối quan hệ với ID này",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin mối quan hệ thành công",
                data = _mapper.Map<CreateFriendShipResponse>(friendShip)
            };
        }

        public async Task<BaseResponse> RemoveFriendShip(Guid friendShipId)
        {
            var friendShip = await _unitOfWork.GetRepository<FriendShip>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(friendShipId) && x.IsActive);

            if (friendShip == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin mối quan hệ với ID này",
                    data = null
                };
            }

            friendShip.IsActive = false;
            friendShip.DeleteAt = TimeUtil.GetCurrentSEATime();
            friendShip.UpdateAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<FriendShip>().UpdateAsync(friendShip);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Xóa thông tin mối quan hệ thành công",
                    data = isSuccessfully
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Xóa thông tin mối quan hệ thất bại",
                data = isSuccessfully
            };
        }

        public async Task<BaseResponse> UpdateFriendShip(Guid friendShipId, UpdateFriendShipRequest request)
        {
            var friendShip = await _unitOfWork.GetRepository<FriendShip>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(friendShipId) && x.IsActive);

            if (friendShip == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin mối quan hệ với ID này",
                    data = null
                };
            }

            friendShip = _mapper.Map(request, friendShip);

            _unitOfWork.GetRepository<FriendShip>().UpdateAsync(friendShip);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessfully)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Cập nhật thông tin mối quan hệ thành công",
                    data = _mapper.Map<CreateFriendShipResponse>(friendShip)
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                message = "Cập nhật thông tin mối quan hệ thất bại",
                data = null
            };
        }

        public async Task<BaseResponse> SearchFriendByPhone(string phoneNumber)
        {
            Guid? id = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var accountExist = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id) && x.IsActive);
            if (accountExist == null)
            {
                _logger.LogWarning($"Không tìm thấy tài khoản có Id {id} .");
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Tài khoản không tồn tại",
                    data = null
                };
            }

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Phone.Equals(phoneNumber) && x.IsActive && !x.Id.Equals(accountExist.Id));

            if (account == null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người dùng nào có số điện thoại này",
                    data = null
                };
            }

            var friendShip = await _unitOfWork.GetRepository<FriendShip>().SingleOrDefaultAsync(
                selector: a => _mapper.Map<CreateFriendShipResponse>(a),
                predicate: a => a.IsActive && ((a.UserId == account.Id && a.FriendId == id) || (a.UserId == id && a.FriendId == account.Id)) && a.Status == FriendShipEnum.Block.ToString());

            if (friendShip != null)
            {
                return new BaseResponse()
                {
                    status = StatusCodes.Status200OK.ToString(),
                    message = "Tài khoản của bạn đã bị đối phương chặn, hoặc bạn đã chặn đối phương",
                    data = null
                };
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin tài khoản thành công",
                data = _mapper.Map<RegisterUserResponse>(account)
            };
        }

        public async Task<BaseResponse> GetRequestList(int page, int size)
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

            Guid? id = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var accountExist = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id) && x.IsActive);
            if (accountExist == null)
            {
                _logger.LogWarning($"Không tìm thấy tài khoản có Id {id} .");
                return new BaseResponse()
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Tài khoản không tồn tại",
                    data = null
                };
            }

            var friendShip = await _unitOfWork.GetRepository<FriendShip>().GetPagingListAsync(
                selector: a => new GetFriendShipResponse
                {
                    Id = a.Id,
                    UserId = a.FriendId,
                    FriendId = a.UserId,
                    FriendFullName = a.User.FullName,
                    FriendDateOfBirth = a.User.DateOfBirth,
                    FriendGender = a.User.Gender,
                    FriendAvatarUrl = a.User.AvatarUrl,
                    Status = a.Status
                },
                predicate: a => a.IsActive && a.FriendId == accountExist.Id && a.Status == FriendShipEnum.Request.ToString(),
                include: query => query.Include(a => a.Friend),
                page: page,
                size: size);

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin mối quan hệ thành công",
                data = friendShip
            };
        }
    }
}
