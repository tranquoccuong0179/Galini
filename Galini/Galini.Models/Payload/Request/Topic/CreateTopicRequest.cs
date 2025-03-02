using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galini.Models.Enum;

namespace Galini.Models.Payload.Request.Topic
{
    public class CreateTopicRequest
    {
        public TopicNameEnum Name { get; set; }
    }
}
