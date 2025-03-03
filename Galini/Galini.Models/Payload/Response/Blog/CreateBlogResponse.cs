using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.Blog
{
    public class CreateBlogResponse
    {
        public Guid? Id { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }
    }
}
