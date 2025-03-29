using Galini.Models.Entity;
using Galini.Repository.Interface;
using Galini.Utils;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Implement
{
    public class PremiumCheckJob : IJob
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<PremiumCheckJob> _logger;
        public PremiumCheckJob(IUnitOfWork<HarmonContext> unitOfWork, ILogger<PremiumCheckJob> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var expiredUserInfo = await _unitOfWork.GetRepository<UserInfo>().GetListAsync(
                predicate: x => x.IsActive && x.DateEnd < TimeUtil.GetCurrentSEATime()
            );

            var normalPremium = await _unitOfWork.GetRepository<Premium>().SingleOrDefaultAsync(
                predicate: x => x.IsActive && x.Type.Equals("normal")
            );

            if (normalPremium == null)
            {
                _logger.LogWarning("Không tìm thấy gói premium mặc định.");
                return;
            }

            DateTime dateEndUtc = new DateTime(9999, 11, 30, 23, 59, 59, DateTimeKind.Utc);
            DateTime dateEnd = TimeUtil.ConvertToSEATime(dateEndUtc);

            if (expiredUserInfo.Any())
            {
                foreach (var item in expiredUserInfo)
                {
                    item.PremiumId = normalPremium.Id;
                    item.DateEnd = dateEnd;
                    item.UpdateAt = TimeUtil.GetCurrentSEATime();
                }

                _unitOfWork.GetRepository<UserInfo>().UpdateRange(expiredUserInfo);
                await _unitOfWork.CommitAsync();

            }
            else
            {
                _logger.LogInformation("Không có tài khoản premium nào hết hạn.");
            }
        }

    }
}
