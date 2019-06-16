using DigitalVolunteer.Core.DataModels;

namespace DigitalVolunteer.Core.Extensions
{
    public static class UserExtensions
    {
        public static string GetShortName( this User user ) 
            => $"{user.FirstName} {user.LastName[0]}.";
    }
}