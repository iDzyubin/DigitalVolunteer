using System;
using DigitalVolunteer.Core.DomainModels;

namespace DigitalVolunteer.Core.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Rating GetRating( Guid id );
    }
}