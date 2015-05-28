
using Ressources;
using System.ComponentModel.DataAnnotations;

namespace XiopiaWorkTimeTracker.Models.Database
{
    public class Setting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int HoursAday { get; set; }
        public int HoursAweek { get; set; }
        public int MonthsAyear { get; set; }
        public int DaysAweek { get; set; }
        public int VacationDays { get; set; }

		[Display(Name = "Bundesland")]
		public int GermanStateId { get; set; }
    }
}