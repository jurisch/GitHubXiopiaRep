using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.ViewModels
{
	public class ProjectViewModel
	{
		public List<Project> projects { get; set; }
		public List<Employee> employees { get; set; }

		public ProjectViewModel(List<Project> _projects, List<Employee> _employees)
		{
			projects = _projects;
			employees = _employees;
		}
	}
}