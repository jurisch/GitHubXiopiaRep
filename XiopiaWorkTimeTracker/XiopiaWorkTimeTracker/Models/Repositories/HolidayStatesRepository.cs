using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.Repositories
{
	public class HolidayStatesRepository :Repository<GermanState>
	{
		public GermanState GetById(int id)
		{
			return DbSet.Where(g => g.Id == id).First();
		}

		public List<GermanState> GetAll()
		{
			return DbSet.Select(g => g).ToList();
		}

		//public List<GermanState> GetByHolidayTyp(int holidayid)
		//{
		//	var statesList = new List<GermanState>();
		//	var holidayTypsRepo = new HolidaysTypRepository();
		//	var userToProjMapRepo = new ProjectToMembersRepository();

		//	return userToProjMapRepo.GetProjectsByUserGuid(userGuid);
		//}
	}
}