using LinqToDB.Mapping;

namespace DigitalVolunteer.Core.DataModels
{
    public enum UserStatus
    {
        Unknown = 0,
        Unconfirmed = 1,
        Confirmed = 2,
        Deleted = 3
    }
}
