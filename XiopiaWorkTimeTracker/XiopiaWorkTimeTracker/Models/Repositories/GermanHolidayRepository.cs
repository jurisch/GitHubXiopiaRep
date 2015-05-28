using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

		public List<GermanHoliday> GetAllByMonth(int month)
		{
			var x = DbSet.Select(g => g).ToList();
			List<GermanHoliday> convertedDates = new List<GermanHoliday>();

			foreach (GermanHoliday h in x) {
				if (null == h.Datum)
				{
					var dt = GetDatum(GetOstersonntag(DateTime.Now.Year), h);
					if (dt.Month == month)
					{
						h.Datum = dt.ToString(); //.Take(6).ToString();
						convertedDates.Add(h);
					}
				}
				else if (h.Datum.Contains("." + month + ".") || h.Datum.Contains(".0" + month + ".")) {
					h.Datum = DateTime.Parse(h.Datum + "" + DateTime.Now.Year).ToString();	 //dt.ToString().Take(6).ToString();
					convertedDates.Add(h);
				}

			}

			//var x = DbSet.Where(g => g.Datum.Contains("."+month+".") || g.Datum.Contains(".0" + month + "."));
			return convertedDates;
		}

		public List<GermanHoliday> GetAllAccepted()
		{
			return DbSet.Where(g => g.Festgelegt == true).ToList();
		}





		//helper methoden
		private DateTime GetDatum(DateTime osterSonntag, GermanHoliday holiday)
		{
            DateTime datum = new DateTime();
			HolidaysTypRepository typ = new HolidaysTypRepository();
			if (!holiday.FeiertagsArt.Equals(typ.GetById(1).FeiertagsArt))
			{
				datum = osterSonntag.AddDays(holiday.TageHinzu);
			}
			else
			{
				datum = DateTime.Parse(holiday.Datum + "." + osterSonntag.Year);
			}

			return DateTime.Parse(datum.Day.ToString() + "." + datum.Month.ToString() + "." + osterSonntag.Year);
		}

		private DateTime GetOstersonntag(int jahr)
		{
			int c;
			int i;
			int j;
			int k;
			int l;
			int n;
			int OsterTag;
			int OsterMonat;

			c = jahr / 100;
			n = jahr - 19 * ((int)(jahr / 19));
			k = (c - 17) / 25;
			i = c - c / 4 - ((int)(c - k) / 3) + 19 * n + 15;
			i = i - 30 * ((int)(i / 30));
			i = i - (i / 28) * ((int)(1 - (i / 28)) * ((int)(29 / (i + 1))) * ((int)(21 - n) / 11));
			j = jahr + ((int)jahr / 4) + i + 2 - c + ((int)c / 4);
			j = j - 7 * ((int)(j / 7));
			l = i - j;

			OsterMonat = 3 + ((int)(l + 40) / 44);
			OsterTag = l + 28 - 31 * ((int)OsterMonat / 4);

			return Convert.ToDateTime(OsterTag.ToString() + "." + OsterMonat + "." + jahr);
		}

	}
}