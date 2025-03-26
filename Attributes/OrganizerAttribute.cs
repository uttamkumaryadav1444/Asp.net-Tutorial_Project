using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventManagementWebApp.Attributes
{
    public class OrganizerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;

            if (!user.IsInRole("Organizer"))
            {
                context.Result = new RedirectToActionResult("Index", "Event", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
