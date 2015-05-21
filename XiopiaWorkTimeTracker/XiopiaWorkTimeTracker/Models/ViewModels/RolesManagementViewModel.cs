using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.ViewModels
{
    public class RolesManagementViewModel
    {
		public Employee User { get; set; }

		public int UserRoleId { get; set; }

		public int AdminRoleId { get; set; }

		public int AccountingRoleId { get; set; }

		public int ProjectSupervisorRoleId { get; set; }

		public List<Employee> employees { get; set; }

		public RolesManagementViewModel( List<Employee> _employees)
		{
			employees = _employees;
		}
	}
}