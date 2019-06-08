using System;
using System.Collections.Generic;
using DigitalVolunteer.Core.DomainModels;

namespace DigitalVolunteer.Core.Repositories
{
    public class MoqAccountRepository : IAccountRepository
    {
        private static readonly List<Account> Accounts = new List<Account>
        {
            new Account{Id = Guid.NewGuid(), FirstName = "Tim",  LastName = "Cook",      Description = GetDescription(), Rating = {LikeCount = 100, DislikeCount = 10}},
            new Account{Id = Guid.NewGuid(), FirstName = "Bill", LastName = "Gates",     Description = GetDescription(), Rating = {LikeCount = 70,  DislikeCount = 20}},
            new Account{Id = Guid.NewGuid(), FirstName = "John", LastName = "Appleseed", Description = GetDescription(), Rating = {LikeCount = 50,  DislikeCount = 70}},
        };
        
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

        public Account Get( Guid id ) => Accounts.Find( x => x.Id == id );

        public List<Account> GetAll() => Accounts;

        public static string GetDescription() =>
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do " +
            "eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
            "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi " +
            "ut aliquip ex ea commodo consequat.";
        
        public Rating GetRating( Guid id )
        {
            return new Rating { LikeCount = 100, DislikeCount = 5 };
        }
    }
}