using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XiopiaWorkTimeTracker.Models.Database
{
	public class HolidayToStateMapping
	{
		public int Id { get; set; }

		public int GermanHolidayId { get; set; }

		public int GermanStateId { get; set; }
	}
}