using Galini.Models.Entity;
using Galini.Models.Payload.Response.Account;
using Galini.Models.Payload.Response.Review;
using Galini.Models.Payload.Response.Topic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.ListenerInfo
{
    public class GetListenerInfoResponse
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public double? Star { get; set; }
        public decimal Price { get; set; }
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public string Gender { get; set; } = null!;
        //public Guid AccountId { get; set; }
        public GetAccountResponse GetAccountResponse { get; set; }
        public List<GetTopicResponse> GetTopicResponse { get; set; }
        public List<CreateReviewResponse> CreateReviewResponses { get; set; }
    }
}
