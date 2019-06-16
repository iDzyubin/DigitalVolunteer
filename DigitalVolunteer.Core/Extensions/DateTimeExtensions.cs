using System;
using System.Globalization;

namespace DigitalVolunteer.Core.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Convert to format: 08 июн, 16:21
        /// </summary>
        /// <returns></returns>
        public static string ConvertToString( this DateTime date ) => date.ToString(
            format: "dd MMM, H:mm",
            provider: CultureInfo.CreateSpecificCulture( "ru" ) );
    }
}
