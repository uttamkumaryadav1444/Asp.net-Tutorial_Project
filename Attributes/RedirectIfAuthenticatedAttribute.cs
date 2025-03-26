using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementWebApp.Attributes
{
    public class RedirectIfAuthenticatedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;

            if (user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Index", "Event", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
