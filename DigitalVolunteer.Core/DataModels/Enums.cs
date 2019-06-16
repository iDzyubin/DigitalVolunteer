namespace DigitalVolunteer.Core.DataModels
{
    public enum UserStatus
    {
        Unknown = 0,
        Unconfirmed = 1,
        Confirmed = 2,
        Deleted = 3
    }
    
    public enum DigitalTaskStatus
    {
        Open = 0,
        Completed = 1,
        Canceled = 2
    }

    public enum DigitalTaskFormat
    {
        Freelance = 0,
        UpWork = 1
    }
}
