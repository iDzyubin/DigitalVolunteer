using System;
using DigitalVolunteer.Core.DataModels;

namespace DigitalVolunteer.Core.Extensions
{
    public static class UserExtensions
    {
        public static string GetShortName( this User user )
            => $"{user.FirstName} {user.LastName[0]}.";


        public static string GetShortDescription( this User user )
            => !String.IsNullOrEmpty( user.Description ) && user.Description.Length > 250
                ? user.Description.Substring( 0, 250 ) + "..."
                : user.Description;
    }
}