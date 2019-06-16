using System;
using DigitalVolunteer.Core.DataModels;

namespace DigitalVolunteer.Core.Extensions
{
    public static class TaskExtensions
    {
        public static string GetScheduleInformation( this DigitalTask task )
        {
            var (start, end) = (task.StartDate, task.EndDate);

            if( start.HasValue && end.HasValue )
            {
                return $"{start.Value.ConvertToString()} - {end.Value.ConvertToString()}";
            }
            if( start.HasValue )
            {
                return $"Начать {start.Value.ConvertToString()}";
            }
            if( end.HasValue )
            {
                return $"Закончить до {end.Value.ConvertToString()}";
            }

            return String.Empty;
        }
    }
}
