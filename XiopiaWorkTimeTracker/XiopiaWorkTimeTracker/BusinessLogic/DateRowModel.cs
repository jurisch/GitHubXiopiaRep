using System;
using System.Collections.Generic;
using XiopiaWorkTimeTracker.Models.Database;
using XiopiaWorkTimeTracker.Models.Repositories;

namespace XiopiaWorkTimeTracker.BusinessLogic
{
    public class DateRowModel
    {
        public DateTime WorkDate { get; set; }
        public List<WorkTimeRow> DataRow { get; set; }
        public bool AttrIll { get; set; }
        public bool AttrHoliday { get; set; }
        public int DayId { get; set; }

		public List<GermanHoliday> AttrGermanHoliday
		{
			get
			{
				GermanHolidayRepository germanholidayrep = new GermanHolidayRepository();
				List<GermanHoliday> x = new List<GermanHoliday>();
				Dictionary<int, GermanHoliday> germanHolidays = germanholidayrep.GetAll();
                foreach (var id in germanholidayrep.GetAllByMonthAccepted(this.WorkDate.Month))
				{
					x.Add(germanHolidays[id]);
                }
				return x;
			}
		}
    }
}