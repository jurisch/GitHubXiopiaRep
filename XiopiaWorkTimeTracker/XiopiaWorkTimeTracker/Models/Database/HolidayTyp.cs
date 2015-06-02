using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XiopiaWorkTimeTracker.Models.Database
{
	public class HolidayTyp
	{
		public GermanHoliday.Feiertagsarten Id { get; set; }

		public string FeiertagsArt { get; set; }
	}
}