using System;
using System.Collections.Generic;
using System.Linq;
using DigitalVolunteer.Core.DomainModels;
using DigitalVolunteer.Core.Extensions;
using DigitalVolunteer.Core.Repositories;

namespace DigitalVolunteer.Core.Services
{
    public class ExecutorService
    {
        private readonly IAccountRepository _accountRepository;

        
        public ExecutorService( IAccountRepository accountRepository )
        {
            _accountRepository = accountRepository;
        }

        public IEnumerable<Executor> GetExecutorList()
            => from account in _accountRepository.GetAll()
               select new Executor
               {
                   Id = account.Id,
                   Name = $"{account.FirstName} {account.LastName[ 0 ]}.",
                   Description = GetShortDescription( account.Description ),
                   Rating = _accountRepository.GetRating( account.Id )
               };

        
        public Executor GetInformationAboutExecutor( Guid id )
            => _accountRepository.Get( id ).ConvertToExecutor();

        
        private string GetShortDescription( string description )
            => description.Length <= 200
                ? description
                : description.Substring( 0, 200 ) + "...";
    }
}