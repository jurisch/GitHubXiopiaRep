using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XiopiaWorkTimeTracker.Models.Database;
using XiopiaWorkTimeTracker.Models.ViewModels;

namespace XiopiaWorkTimeTracker.Models
{
	public class SettingsViewModel
	{
		[Display(Name = "HoursAday", ResourceType = typeof(Ressources.Language))]
		public int HoursAday { get; set; }
		[Display(Name = "HoursAweek", ResourceType = typeof(Ressources.Language))]
		public int HoursAweek { get; set; }
		[Display(Name = "MonthsAyear", ResourceType = typeof(Ressources.Language))]
		public int MonthsAyear { get; set; }
		[Display(Name = "DaysAweek", ResourceType = typeof(Ressources.Language))]
		public int DaysAweek { get; set; }
		[Display(Name = "VacationDays", ResourceType = typeof(Ressources.Language))]
		public int VacationDays { get; set; }

		[Display(Name = "Bundesland")]
		public int GermanStateId { get; set; }

		public UserAndRolesViewModel usrvm { get; set;}

		public FeierTag germanHolidays { get; set; }
	}
}