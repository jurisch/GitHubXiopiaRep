using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.Repositories
{
	public class HolidayToStateRepository : Repository<HolidayToStateMapping>
	{
		public List<GermanHoliday> GetHolidayByStateId(int id)
		{
			var holidays = new List<GermanHoliday>();
			var germanHolidaysRepo = new GermanHolidayRepository();
			var mappings = DbSet.Where(h => h.GermanStateId == id).ToList();
			foreach (var m in mappings)
			{
				holidays.Add(germanHolidaysRepo.AllHolidays[m.GermanHolidayId]);
			}
			return holidays;
		}

		public List<GermanState> GetGermanStatesByHolidayId(int id)
		{
			var germanStates = new List<GermanState>();
			var statesRepo = new HolidayStatesRepository();
			var holidayMappings = DbSet.Where(m => m.GermanHolidayId == id);
			foreach (var pr in holidayMappings)
			{
				germanStates.Add(statesRepo.GetById(pr.GermanStateId));
			}
			return germanStates;
		}
	}
}