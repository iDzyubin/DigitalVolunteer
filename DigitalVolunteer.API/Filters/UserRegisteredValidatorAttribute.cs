using DigitalVolunteer.Core.Services;
using DigitalVolunteer.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DigitalVolunteer.API.Filters
{
    public class UserRegisteredValidatorAttribute : IActionFilter
    {
        private readonly UserService _userService;

        public UserRegisteredValidatorAttribute( UserService userService )
        {
            _userService = userService;
        }

        public void OnActionExecuting( ActionExecutingContext context )
        {
            var args        = context.ActionArguments;
            var routeValues = context.RouteData.Values;
            var (controller, action) = ( ( string ) routeValues[ "controller" ], ( string ) routeValues[ "action" ] );

            if( controller == "Account" && action == "Registration" )
            {
                if( args[ "model" ] is RegistrationViewModel model && _userService.IsUserRegistered( model.Email ) )
                {
                    context.Result = new BadRequestObjectResult( "Аккаунт с таким e-mail адресом уже существует" );
                }
            }
        }

        public void OnActionExecuted( ActionExecutedContext context )
        {
        }
    }
}