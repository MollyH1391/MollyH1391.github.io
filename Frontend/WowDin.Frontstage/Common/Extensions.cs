using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace WowDin.Frontstage.Common
{
    public static class Extensions
    {

        public static T JsonDeserialize<T>(this string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static string JsonSerialize(this object source)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(source);
        }

        public static string GetDescription(this Enum value)
        {
            var fi=value.GetType().GetField(value.ToString());
            var attributes=(DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description :value.ToString();
        }

        public static DateTime TransferToTaipeiTime(this DateTime utcTime)
        {
            DateTime result;
            var taipeiId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Taipei Standard Time" : "Asia/Taipei";

            if (TimeZoneInfo.Local.Id == taipeiId)
            {
                result = utcTime.ToLocalTime();
            }
            else
            {
                result = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcTime, taipeiId);
            }

            return result;
        }
    }
}
