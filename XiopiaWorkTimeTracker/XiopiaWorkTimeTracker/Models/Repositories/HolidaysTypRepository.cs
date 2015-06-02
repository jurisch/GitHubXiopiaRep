using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.Repositories
{
	public class HolidaysTypRepository : Repository<HolidayTyp>
	{
		public HolidayTyp GetById(GermanHoliday.Feiertagsarten id)
		{
			return DbSet.Where(g => g.Id == id).First();
		}

		public List<HolidayTyp> GetAll()
		{
			return DbSet.Select(g => g).ToList();
		}
	}
}