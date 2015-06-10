using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LDAPTest.Attributes
{
	public class RequiresAuthenticationAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
			{
				var redirectOnSuccess = filterContext.HttpContext.Request.Url.AbsolutePath;
				var redirectUrl = string.Format("?ReturnUrl={0}", redirectOnSuccess);
				var loginUrl = FormsAuthentication.LoginUrl + redirectUrl;
				filterContext.HttpContext.Response.Redirect(loginUrl, true);
			}
		}

	}
}