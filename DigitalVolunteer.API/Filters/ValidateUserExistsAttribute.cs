using System;
using DigitalVolunteer.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DigitalVolunteer.API.Filters
{
    // TODO. Generic realization.
    public class ValidateUserExistsAttribute : IActionFilter
    {
        private readonly IUserRepository _userRepository;

        public ValidateUserExistsAttribute( IUserRepository userRepository )
        {
            _userRepository = userRepository;
        }

        public void OnActionExecuting( ActionExecutingContext context )
        {
            var args = context.ActionArguments;
            if( !args.ContainsKey( "id" ) || !( args[ "id" ] is Guid ) )
            {
                context.Result = new BadRequestObjectResult( "Id is not valid" );
                return;
            }

            var id = ( Guid ) args[ "id" ];
            if( _userRepository.Get( id ) == null )
            {
                context.Result = new NotFoundObjectResult( "User not found" );
            }
        }


        public void OnActionExecuted( ActionExecutedContext context )
        {
        }
    }
}