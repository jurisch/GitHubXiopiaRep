using System;
using System.Collections.Generic;
using System.Linq;
using XiopiaWorkTimeTracker.Models.Database;
using XiopiaWorkTimeTracker.Models.Repositories;

namespace XiopiaWorkTimeTracker.BusinessLogic
{
	public class DateRowModel
	{
		private static Dictionary<int, List<GermanHoliday>> myHolidays;
		public DateTime WorkDate { get; set; }
		public List<WorkTimeRow> DataRow { get; set; }
		public bool AttrIll { get; set; }
		public bool AttrHoliday { get; set; }
		public int DayId { get; set; }

		public bool TryGetHoliday(DateTime date, out String Name)
		{
			var context = new WorkTimeTrackerDbContext();
			GermanHolidayRepository germanholidayrep = new GermanHolidayRepository();
			if (germanholidayrep.TryGetFeiertag(date, context.GlobalSettings.First().GermanStateId, out Name))
				return true;
			else
				Name = String.Empty;

			return false;

		}

		//public List<GermanHoliday> AttrGermanHoliday
		//{
		//	get
		//	{
		//		if (myHolidays.ContainsKey(this.WorkDate.Month))
		//			return myHolidays[this.WorkDate.Month];

		//		GermanHolidayRepository germanholidayrep = new GermanHolidayRepository();
		//		List<GermanHoliday> x = new List<GermanHoliday>();
		//		Dictionary<int, GermanHoliday> germanHolidays = germanholidayrep.AllHolidays;
		//		foreach (var id in germanholidayrep.AllHolidaysByMonthAccepted[this.WorkDate.Month])
		//		{
		//			x.Add(germanHolidays[id]);
		//		}
		//		myHolidays.Add(this.WorkDate.Month, x);

		//		return x;
		//	}
		//}

		static DateRowModel()
		{
			myHolidays = new Dictionary<int, List<GermanHoliday>>();
		}
	}
}