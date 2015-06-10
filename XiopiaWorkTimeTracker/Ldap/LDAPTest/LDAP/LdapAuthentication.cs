using System.Text;
using System.Collections;
using System.DirectoryServices;
using System;
using System.Configuration;
using LDAPTest.Models;
using System.Linq;
using System.Web.Security;

namespace LDAP
{
	internal class LdapAuthentication
	{
		private string _path;
		private string _filterAttribute;

		public LdapAuthentication(string path)
		{
			_path = path;
		}

		public bool IsAuthenticated(string domain, LogOnModel model)
		{
			string domainAndUsername = domain + @"\" + model.UserName;
			DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, model.Password);

			try
			{
				MembershipProvider domainProvider = Membership.Providers["AdMembershipProvider"];
				if (!ValidateUserAD(model.UserName, model.Password))
				{
					return false;
				}
				string connection = ConfigurationManager.ConnectionStrings["ADConnection"].ToString();
				DirectorySearcher dssearch = new DirectorySearcher(connection);
				dssearch.Filter = "(SAMAccountName=" + model.UserName + ")";
				SearchResult result = dssearch.FindOne();
				if (null == result)
				{
					return false;
				}
				// Update the new path to the user in the directory
				_path = result.Path;
				_filterAttribute = (String)result.Properties["cn"][0];
			}
			catch (Exception ex)
			{
				throw new Exception("Error authenticating user. " + ex.Message);
			}
			return true;
		}

		public bool ValidateUserAD(String UserName, String Password)
		{

			var provider = Membership.Provider;
			string name = provider.ApplicationName;

			return Membership.ValidateUser(UserName, Password);
		}


		public string GetGroups()
		{
			DirectorySearcher search = new DirectorySearcher(_path);
			search.Filter = "(cn=" + _filterAttribute + ")";
			search.PropertiesToLoad.Add("memberOf");
			StringBuilder groupNames = new StringBuilder();
			try
			{
				SearchResult result = search.FindOne();
				int propertyCount = result.Properties["memberOf"].Count;
				String dn;
				int equalsIndex, commaIndex;

				for (int propertyCounter = 0; propertyCounter < propertyCount;
					 propertyCounter++)
				{
					dn = (String)result.Properties["memberOf"][propertyCounter];

					equalsIndex = dn.IndexOf("=", 1);
					commaIndex = dn.IndexOf(",", 1);
					if (-1 == equalsIndex)
					{
						return null;
					}
					groupNames.Append(dn.Substring((equalsIndex + 1),
									  (commaIndex - equalsIndex) - 1));
					groupNames.Append("|");
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error obtaining group names. " +
				  ex.Message);
			}
			return groupNames.ToString();
		}

		internal Account getUserData()
		{
			using (AccountDBContext dc = new AccountDBContext())
			{
				var v = dc.Person.Where(a => a.Username.Equals(_filterAttribute)).FirstOrDefault();
				if (v != null)
				{
					return v;
				}
			}
			return null;
		}
	}
}