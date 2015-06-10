using LDAPTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LDAPTest.Controllers
{
	public class AccountController : Controller
	{
		// GET: Account
		public ActionResult LogOn()
		{
			return View();
		}

		[HttpPost]
		public ActionResult LogOn(LogOnModel model, string returnUrl)
		{

			MembershipProvider domainProvider;
			domainProvider = Membership.Providers["AdMembershipProvider"];

			if (ModelState.IsValid) {
				if (ValidateUserAD(model.UserName, model.Password)){
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
					if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
					{
						return Redirect(returnUrl);
					}
					else {


						return RedirectToAction("Index","Ldap");
					}
                }
			}
			return View(model);
		}

		public bool ValidateUserAD(String UserName, String Password) {

			var provider = Membership.Provider;
			string name = provider.ApplicationName;

			return Membership.ValidateUser(UserName, Password);
		}

		public ActionResult LogOff()
		{
			FormsAuthentication.SignOut();
			return View();
		}

		private static string ErrorCodetoString(MembershipCreateStatus createStatus){

			return createStatus.ToString();
		}




	}
}