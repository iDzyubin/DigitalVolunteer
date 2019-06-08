using System;
using System.Collections.Generic;
using DigitalVolunteer.Core.DomainModels;

namespace DigitalVolunteer.Core.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public void Add( Account item )
        {
            throw new NotImplementedException();
        }

        public void Remove( Guid id )
        {
            throw new NotImplementedException();
        }

        public void Update( Account item )
        {
            throw new NotImplementedException();
        }

        public Account Get( Guid id )
        {
            throw new NotImplementedException();
        }

        public List<Account> GetAll()
        {
            throw new NotImplementedException();
        }

        public Rating GetRating( Guid id )
        {
            return new Rating();
        }
    }
}