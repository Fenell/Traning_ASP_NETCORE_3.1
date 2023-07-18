using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Win32.SafeHandles;

namespace eShopSolution.AdminApp.Controllers
{
	public class BaseController : Controller
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var session = context.HttpContext.Session.GetString("Token");
			if (session == null)
			{
				context.Result = RedirectToAction("Index", "Login");
			}
		}
	}
}
