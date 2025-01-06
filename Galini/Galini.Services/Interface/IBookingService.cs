using Galini.Models.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Services.Interface
{
    internal interface IBookingService
    {
        public Task<CreateBookingResponse> CreateBooking(CreateBookingRequest request, Guid subjectId);
        public Task<IPaginate<GetChapterResponse>> GetListChapter(int page, int size);
        public Task<IPaginate<GetTopicResponse>> GetListTopic(Guid id, int page, int size);
        public Task<GetChapterResponse> GetChapterById(Guid id);
        public Task<bool> UpdateChapter(Guid id, UpdateChapterRequest request, Guid subjectId);
        public Task<bool> RemoveChapter(Guid id);
    }
}
