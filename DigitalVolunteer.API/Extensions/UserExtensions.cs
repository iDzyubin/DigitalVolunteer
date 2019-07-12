using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Web.Models;

namespace DigitalVolunteer.API.Extensions
{
    public static class UserExtensions
    {
        public static User ConvertToUser( this EditUserViewModel model )
            => new User
            {
                Id        = model.Id,
                FirstName = model.FirstName,
                LastName  = model.LastName,
                Phone     = model.Phone,
                IsAdmin   = model.IsAdmin
            };
    }
}