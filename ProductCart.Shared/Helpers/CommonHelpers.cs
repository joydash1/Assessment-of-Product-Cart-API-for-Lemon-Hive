using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCart.Shared.Helpers
{
    public class CommonHelpers
    {
        public static DateTime GetBangladeshTimeZone(DateTime dateTime)
        {
            var bangladeshTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime.ToUniversalTime(), bangladeshTimeZone);
        }
    }
}