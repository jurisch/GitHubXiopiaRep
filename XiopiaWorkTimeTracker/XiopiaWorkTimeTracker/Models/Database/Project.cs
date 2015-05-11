using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Ressources;

namespace XiopiaWorkTimeTracker.Models.Database
{
    public class Project
    {
		[Key]
		public int Id { get; set; }

		[Display(Name = "ProjectName", ResourceType = typeof(Language))]
		[Required(AllowEmptyStrings = false, ErrorMessageResourceName = "errProjName", ErrorMessageResourceType = typeof(Language))]
		public string Name { get; set; }

		[Display(Name = "ProjectResponsible", ResourceType = typeof(Language))]
		[Required(AllowEmptyStrings = false, ErrorMessageResourceName = "errProjectResponsible", ErrorMessageResourceType = typeof(Language))]
		public int ProjectResponsibleId { get; set; }

		public virtual IEnumerable<Employee> Employees { get; set; }
    }
}