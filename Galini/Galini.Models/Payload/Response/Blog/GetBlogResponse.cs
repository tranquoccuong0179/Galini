using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.Blog
{
    public class GetBlogResponse
    {
        public Guid? Id { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public int? Views { get; set; }

        public int? Likes { get; set; }
    }
}
