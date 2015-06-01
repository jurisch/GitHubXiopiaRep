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
		private static Dictionary<Guid, Dictionary<Int32, GermanHoliday>> _userHolidays;
		private static Dictionary<Int32, GermanHoliday> _holidays;
		private static List<GermanHoliday> _allHolidays;
		private static List<GermanHoliday> _allHolidaysAccepted;
		private static Dictionary<int, List<GermanHoliday>> _allHolidaysPerMonthAccepted;

		static GermanHolidayRepository()
		{
			_holidays = new Dictionary<int, GermanHoliday>();
			_allHolidaysPerMonthAccepted = new Dictionary<int, List<GermanHoliday>>();
		}

		public GermanHoliday GetById(int id)
		{
			if (!_holidays.ContainsKey(id))
				_holidays.Add(id, DbSet.Where(g => g.Id == id).First());

			return _holidays[id];
		}

		public List<GermanHoliday> GetAll()
		{
			if (_allHolidays == null)
			{
				_allHolidays = GetGermanHolidaysConverted(DbSet.Select(g => g).ToList());
			}

			return _allHolidays;
		}

		public List<GermanHoliday> GetAllByMonth(int month)
		{
			if (!_allHolidaysPerMonthAccepted.ContainsKey(month))
			{
				List<GermanHoliday> _allHolidaystmp = new List<GermanHoliday>();
				foreach (GermanHoliday h in GetAllAccepted())
				{
					if (h.Datum.Contains("." + month + ".") || h.Datum.Contains(".0" + month + "."))
					{
						_allHolidaystmp.Add(h);
					}
				}
				_allHolidaysPerMonthAccepted.Add(month, _allHolidaystmp);
			}

			return _allHolidaysPerMonthAccepted[month];
		}

		private List<GermanHoliday> GetAllAccepted()
		{
			if (_allHolidaysAccepted == null)
			{
				_allHolidaysAccepted = GetGermanHolidaysConverted(DbSet.Where(g => g.Festgelegt == true).ToList());
			}
			return _allHolidaysAccepted;
		}

		private List<GermanHoliday> GetGermanHolidaysConverted(List<GermanHoliday> germanHolidays)
		{
			List<GermanHoliday> convertedDates = new List<GermanHoliday>();
			foreach (GermanHoliday h in germanHolidays)
			{
				if (null == h.Datum)
				{
					var dt = GetDatum(GetOstersonntag(DateTime.Now.Year), h);
					h.Datum = dt.ToString();
				}
				else
				{
					h.Datum = DateTime.Parse(h.Datum + "" + DateTime.Now.Year).ToString();
				}
				convertedDates.Add(h);
			}
			return convertedDates.Select(x => x).ToList();
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

		public String GetFeiertag(DateTime datum, int land)
		{
			var feiertage = GetAllAccepted();
			// Liste der Feiertage durchgehen
			foreach (GermanHoliday f in feiertage)
			{
				if (datum.ToShortDateString().Equals(GetDatum(GetOstersonntag(datum.Year), f).ToShortDateString()))
				{
					HolidayStatesRepository hsr = new HolidayStatesRepository();
					// Prüfen ob das Land enthalten ist
					foreach (string l in f.Land)
					{
						if (hsr.GetById(land).Equals(l))
						{
							return f.Feiertag;
						}
					}
				}
			}
			return "";
		}

		public Boolean IsFeiertag(DateTime date, int land)
		{
			return (GetFeiertag(date, land).Length > 0);
		}

	}
}