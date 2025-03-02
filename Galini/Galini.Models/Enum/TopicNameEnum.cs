using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;

namespace Galini.Models.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TopicNameEnum
    {
        [Description("Stress Lo Âu")]
        StressAnxiety,

        [Description("Trầm Cảm")]
        Depression,

        [Description("Mối Quan Hệ (gia đình, bạn bè, tình yêu)")]
        Relationships,

        [Description("Công Việc Học Tập")]
        WorkStudy,

        [Description("Tự Tin Giá Trị Bản Thân")]
        SelfConfidence,

        [Description("Kiểm Soát Cảm Xúc")]
        EmotionalControl,

        [Description("Mất Mát Tổn Thương")]
        LossTrauma,

        [Description("Rối Loạn Giấc Ngủ")]
        SleepDisorders,

        [Description("Định Hướng Cuộc Sống")]
        LifeOrientation,

        [Description("Kỹ Năng Giao Tiếp")]
        CommunicationSkills
    }
}
