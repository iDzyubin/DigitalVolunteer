using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DigitalVolunteer.API.Filters
{
    public class ModelValidatorAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting( ActionExecutingContext context )
        {
            if( !context.ModelState.IsValid )
            {
                context.Result = new BadRequestObjectResult( context.ModelState );
            }
        }

        public void OnActionExecuted( ActionExecutedContext context )
        {
        }
    }
}