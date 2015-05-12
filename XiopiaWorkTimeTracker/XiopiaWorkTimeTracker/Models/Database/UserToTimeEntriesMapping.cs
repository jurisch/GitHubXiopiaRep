using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XiopiaWorkTimeTracker.Models.Database
{
	public class UserToTimeEntriesMapping
	{

		public int Id { get; set; }

		public int UserId { get; set; }

		public int TimeEntriyId { get; set; }

	}
}