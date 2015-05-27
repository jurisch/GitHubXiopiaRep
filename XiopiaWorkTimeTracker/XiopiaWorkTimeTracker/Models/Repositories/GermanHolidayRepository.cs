using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.Repositories
{
	public class GermanHolidayRepository : Repository<GermanHoliday>
	{
		public GermanHoliday GetById(int id)
		{
			return DbSet.Where(g => g.Id == id).First();
		}

		public List<GermanHoliday> GetAll()
		{
			return DbSet.Select(g => g).ToList();
		}

		public List<GermanHoliday> GetAllAccepted()
		{
			return DbSet.Where(g => g.Festgelegt == true).ToList();
		}
	}
}