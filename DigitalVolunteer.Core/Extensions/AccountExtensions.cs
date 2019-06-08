using System;
using DigitalVolunteer.Core.DomainModels;

namespace DigitalVolunteer.Core.Extensions
{
    public static class AccountExtensions
    {
        public static Executor ConvertToExecutor( this Account account )
            => new Executor
            {
                Id          = account.Id,
                Name        = account.FirstName,
                Description = account.Description,
                Rating      = account.Rating
            };

        public static string GetShortName( this Account account ) =>
            account.FirstName +
            ( String.IsNullOrEmpty( account.LastName ) ? "" : $" {account.LastName[ 0 ]}" );
    }
}