using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Utils
{
    public static class EnumExtensions
    {
        public static T? GetEnumFromDescription<T>(this string description) where T : struct, System.Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                var attribute = field.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null && attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
                {
                    return (T)System.Enum.Parse(typeof(T), field.Name);
                }
            }
            return null;
        }
    }
}
