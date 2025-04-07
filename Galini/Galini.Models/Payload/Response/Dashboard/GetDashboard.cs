using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.Dashboard
{
    public class GetDashboard
    {
        public int TotalListeners { get; set; }
        public int TotalUsers { get; set; }
        public int TotalBlogs { get; set; }
        public decimal TotalTransaction { get; set; }
        public ChartData Chart { get; set; }
    }

    public class ChartData
    {
        public List<string> Labels { get; set; }
        public List<int> Values { get; set; }
    }
}
