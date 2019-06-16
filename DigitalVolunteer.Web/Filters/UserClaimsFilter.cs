using DigitalVolunteer.Web.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DigitalVolunteer.Web.Filters
{
    public class UserClaimsFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting( ActionExecutingContext context )
        {
            var controller = context.Controller as Controller;
            var user = controller.User;
            var viewBag = controller.ViewBag;

            viewBag.UserId = user.GetId();
            viewBag.IsAuthorized = viewBag.UserId != null;
            viewBag.IsAdmin = user.IsAdmin();

            base.OnActionExecuting( context );
        }
    }
}
