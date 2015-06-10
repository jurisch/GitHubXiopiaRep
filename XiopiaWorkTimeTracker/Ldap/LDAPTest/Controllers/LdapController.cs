using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices;
using System.Configuration;
using LDAPTest.Models;
using System.Web.Security;
using LDAPTest.Attributes;

namespace LDAPTest.Controllers
{
	public class LdapController : Controller
	{

		AccountDBContext accdb = new AccountDBContext();

		// GET: Ldap/index
		[RequiresAuthentication]
		public ActionResult Index()
		{
			var People = accdb.Person.ToList();
			return View(People);
		}

		[RequiresAuthentication]
		public ActionResult Syncronization()
		{
			return View();
		}

		[RequiresAuthentication]
		public ActionResult GetAllUserFromAD()
		{
			var People = accdb.Person.ToList();

			string connection = ConfigurationManager.ConnectionStrings["ADConnection"].ToString();
			DirectorySearcher dssearch = new DirectorySearcher(connection);
			//dssearch.Filter = "(displayName=Alpay Bir)";
			//SearchResult sresult = dssearch.FindOne();
			dssearch.Filter = ("(objectCategory=user)");
			SearchResultCollection sresult = dssearch.FindAll();
			foreach (SearchResult sr in sresult)
			{
				DirectoryEntry dsresult = sr.GetDirectoryEntry();
				try
				{
					if (!People.Exists(p => p.Username == dsresult.Properties["displayName"][0].ToString()))
					{

						var Username = dsresult.Properties["displayName"][0].ToString();
						var Name = dsresult.Properties["givenName"][0].ToString();
						var SecondName = dsresult.Properties["sn"][0].ToString();
						var AccountName = dsresult.Properties["sAMAccountName"][0].ToString();
						var Email = dsresult.Properties["mail"][0].ToString();
                        Account acct = new Account();
						acct.Username = Username;
						acct.Name = Name;
						acct.SecondName = SecondName;
						acct.AccountName = AccountName;
						acct.Email = Email;
						accdb.Person.Add(acct);
						accdb.SaveChanges();
					}
				}
				catch (Exception e)
				{
				}

			}

			return RedirectToAction("Index");
		}

		public ActionResult DataHandler()
		{
			List<Account> employees = accdb.Person.ToList();

			return Json(new
			{
				employees
			},
			JsonRequestBehavior.AllowGet);
		}

		public ActionResult EditSelectedUserData(int? Id)
		{
			try
			{
				var People = accdb.Person.ToList();
				return View(accdb.Person.Find(Id));
			}
			catch
			{
				return View();
			}

		}

		[HttpPost]
		public ActionResult EditSelectedUserData(int? Id, Account person)
		{
			try
			{
				using (AccountDBContext accdb = new AccountDBContext())
				{
					accdb.Entry(person).State = System.Data.Entity.EntityState.Modified;
					accdb.SaveChanges();
					return RedirectToAction("Index");
				}
			}
			catch
			{
				return View();
			}

		}

		public ActionResult DeleteSelectedUser(int? Id)
		{
			using (AccountDBContext accdb = new AccountDBContext())
			{
				return View(accdb.Person.Find(Id));
			}
		}

		[HttpPost]
		public ActionResult DeleteSelectedUser(int? Id, Account person)
		{
			try
			{
				using (AccountDBContext accdb = new AccountDBContext())
				{
					accdb.Entry(person).State = System.Data.Entity.EntityState.Deleted;
					accdb.SaveChanges();
					return RedirectToAction("Index");
				}
			}
			catch
			{
				return View();
			}
		}

		[RequiresAuthentication]
		public ActionResult Home()
		{
			if (Session["LogedUserID"] != null)
			{
				return View();
			}
			else
			{
				return RedirectToAction("Login");
			}
		}


		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			if (Request.IsAuthenticated)
			{
				return RedirectToAction("Home", "Ldap");
			}
			ViewBag.ReturnUrl = returnUrl;

			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(string returnUrl, LogOnModel usr)
		{
			// this action is for handle post (login)
			if (ModelState.IsValid)	// this is check validity
			{
				// Path to you LDAP directory server.
				// Contact your network administrator to obtain a valid path.
				string adPath = "ldap://xsrvdc3.xiopia.local:389/OU=Unterfoehring,OU=xiopia,DC=xiopia,DC=local";
				string txtDomainName = "Xiopia";
				LDAP.LdapAuthentication adAuth = new LDAP.LdapAuthentication(adPath);

				string error ="";
				try
				{
					if (true == adAuth.IsAuthenticated(txtDomainName, usr))
					{
						// Retrieve the user's groups
						string groups = adAuth.GetGroups();
						// Create the authetication ticket
						FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, usr.UserName, DateTime.Now, DateTime.Now.AddMinutes(60), false, groups);
						// Now encrypt the ticket.
						string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
						// Create a cookie and add the encrypted ticket to the cookie as data.
						HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
						// Add the cookie to the outgoing cookies collection.
						Response.Cookies.Add(authCookie);

						Account user = adAuth.getUserData();
						if (null == user)
						{
							Session["LogedUserID"] = usr.UserName.ToString();
							Session["LogedUserFullname"] = usr.UserName.ToString();

							//TODO:automatisch addieren ? oder admin mail? 
							error = "You are not in User List!!!";
						}
						else
						{
							Session["LogedUserID"] = user.ID.ToString();
							Session["LogedUserFullname"] = user.Username.ToString();
						}
					}
					else
					{
						error = "Authentication failed, check username and password.";
					}
				}
				catch (Exception ex)
				{
					error = "Error authenticating. " + ex.Message;
				}

				return RedirectToAction("Home");
			}
			else
				return View(usr);
		}



	}
}