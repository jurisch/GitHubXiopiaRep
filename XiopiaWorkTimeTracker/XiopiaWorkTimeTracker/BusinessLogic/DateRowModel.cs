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
				List<DateTime> germanHoliday = new List<DateTime>();
				GermanHolidayRepository germanholidayrep = new GermanHolidayRepository();
				List<GermanHoliday> x = germanholidayrep.GetAllByMonth(this.WorkDate.Month);
				//foreach (GermanHoliday h in x)
				//{
				//	//germanHoliday.Add(DateTime.Parse(h.Datum + "" + DateTime.Now.Year));
				//	germanHoliday.Add(DateTime.Parse(h.Datum));
				//}
				//return germanHoliday;
				return x;
			}
		}
    }
}