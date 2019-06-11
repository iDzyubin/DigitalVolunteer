using DigitalVolunteer.Core.DataModels;

namespace DigitalVolunteer.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByEmail( string email );
    }
}
