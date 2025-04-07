using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Enum;
using Galini.Models.Payload.Response;
using Galini.Models.Payload.Response.Dashboard;
using Galini.Repository.Interface;
using Galini.Services.Interface;
using Galini.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Galini.Services.Implement
{
    public class DashboardService : BaseService<DashboardService>, IDashboardService
    {
        public DashboardService(IUnitOfWork<HarmonContext> unitOfWork, ILogger<DashboardService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse> GetDashboard()
        {
            var totalUsers = await _unitOfWork.GetRepository<Account>().CountAsync(
                predicate: u => u.IsActive && u.Role.Equals(RoleEnum.Customer.GetDescriptionFromEnum()));

            var totalListeners = await _unitOfWork.GetRepository<Account>().CountAsync(
                predicate: u => u.IsActive && u.Role.Equals(RoleEnum.Listener.GetDescriptionFromEnum()));

            var totalBlogs = await _unitOfWork.GetRepository<Blog>().CountAsync();

            var transactions = await _unitOfWork.GetRepository<Transaction>().GetListAsync(
                predicate: t => t.IsActive && t.Type.Equals(TransactionTypeEnum.DEPOSIT.GetDescriptionFromEnum()));
            decimal totalTransaction = transactions.Sum(t => t.Amount);

            var transactionByYearMonth = transactions
                .GroupBy(t => new { t.CreateAt.Year, t.CreateAt.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Amount = g.Sum(t => t.Amount)
                })
            .ToList();

            var years = transactionByYearMonth.Select(t => t.Year).Distinct().OrderBy(y => y).Take(2).ToList();

            var chartData = new ChartData
            {
                Labels = new List<string>(),
                Values = new List<int>()
            };

            foreach (var year in years)
            {
                for (int month = 1; month <= 12; month++)
                {
                    chartData.Labels.Add($"Tháng {month}/{year}");
                    var transactionInMonth = transactionByYearMonth
                        .FirstOrDefault(t => t.Year == year && t.Month == month);
                    chartData.Values.Add(transactionInMonth != null ? (int)transactionInMonth.Amount : 0);
                }
            }

            return new BaseResponse()
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Dashboard",
                data = new GetDashboard
                {
                    TotalListeners = totalListeners,
                    TotalUsers = totalUsers,
                    TotalBlogs = totalBlogs,
                    TotalTransaction = totalTransaction,
                    Chart = chartData
                }
            };
        }
    }
}
