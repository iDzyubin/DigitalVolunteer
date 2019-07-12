using System;
using DigitalVolunteer.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DigitalVolunteer.API.Filters
{
    // TODO. Generic realization.
    public class ValidateCategoryExistsAttribute : Attribute, IActionFilter
    {
        private readonly ICategoryRepository _categoryRepository;

        
        public ValidateCategoryExistsAttribute( ICategoryRepository categoryRepository )
        {
            _categoryRepository = categoryRepository;
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
            if( _categoryRepository.Get( id ) == null )
            {
                context.Result = new NotFoundObjectResult( "Entity not found" );
            }
        }

        
        public void OnActionExecuted( ActionExecutedContext context )
        {
        }
    }
}