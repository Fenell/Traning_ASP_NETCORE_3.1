using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Win32.SafeHandles;

namespace eShopSolution.AdminApp.Controllers
{
	[Authorize]
	public class BaseController : Controller
	{
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Request.Cookies["Token"];
			if (session == null)
			{
				context.Result = RedirectToAction("Index", "Login");
			}
		}
	}
}
