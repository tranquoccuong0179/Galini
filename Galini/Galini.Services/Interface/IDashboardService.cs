using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galini.Models.Payload.Request.Blog;
using Galini.Models.Payload.Response;

namespace Galini.Services.Interface
{
    public interface IDashboardService
    {
        Task<BaseResponse> GetDashboard();

    }
}
