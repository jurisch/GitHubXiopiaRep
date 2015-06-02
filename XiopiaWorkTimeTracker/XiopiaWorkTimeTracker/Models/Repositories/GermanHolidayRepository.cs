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
		private static Dictionary<Int32, GermanHoliday> _allHolidays;
		private static Dictionary<int, List<Int32>> _allHolidaysPerMonthAcceptedById;

		static GermanHolidayRepository()
		{
			_allHolidays = new Dictionary<int, GermanHoliday>();
			_allHolidaysPerMonthAcceptedById = new Dictionary<int, List<int>>();
		}

		public GermanHoliday GetById(int id)
		{
			return GetAll()[id];
		}

		public Dictionary<Int32, GermanHoliday> GetAll()
		{
			if (_allHolidays == null || _allHolidays.Count == 0)
			{
				List<GermanHoliday> germanHolidays = DbSet.Select(g => g).ToList();
				Dictionary<int, GermanHoliday> convertedDates = new Dictionary<int, GermanHoliday>();
				foreach (GermanHoliday h in germanHolidays)
				{
					convertedDates.Add(h.Id, GetGermanHolidayConverted(h));
				}
				_allHolidays = convertedDates;
			}
			return _allHolidays;
		}

		public List<int> GetAllByMonthAccepted(int month)
		{
			if (!_allHolidaysPerMonthAcceptedById.ContainsKey(month))
			{
				List<int> _allHolidaystmp = new List<int>();
				foreach (GermanHoliday germanHoliday in GetAll().Values)
				{
					if (germanHoliday.DatumConverted.Value.Month == month && germanHoliday.Festgelegt)
					{
						_allHolidaystmp.Add(germanHoliday.Id);
					}
				}
				_allHolidaysPerMonthAcceptedById.Add(month, _allHolidaystmp);
			}

			return _allHolidaysPerMonthAcceptedById[month];
		}

		private GermanHoliday GetGermanHolidayConverted(GermanHoliday h)
		{
			if (null == h.Datum)
			{
				h.DatumConverted = GetDatum(GetOstersonntag(DateTime.Now.Year), h);
			}
			else
			{
				h.DatumConverted = DateTime.Parse(h.Datum + "" + DateTime.Now.Year);
			}

			return h;
		}




		//helper methoden
		private DateTime GetDatum(DateTime osterSonntag, GermanHoliday holiday)
		{
			DateTime datum = new DateTime();
			HolidaysTypRepository typ = new HolidaysTypRepository();
			if (!holiday.FeiertagsArt.Equals(GermanHoliday.Feiertagsarten.Fest))
			{
				datum = osterSonntag.AddDays(holiday.TageHinzu);
			}
			else
			{
				datum = DateTime.Parse(holiday.Datum + "." + osterSonntag.Year);
			}

			return datum;
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
			var feiertage = GetAllByMonthAccepted(datum.Month);
			// Liste der Feiertage durchgehen
			foreach (int id in feiertage)
			{
				GermanHoliday germanHoliday = GetById(id);
				if (datum.ToShortDateString().Equals(GetDatum(GetOstersonntag(datum.Year), germanHoliday).ToShortDateString()))
				{
					HolidayStatesRepository hsr = new HolidayStatesRepository();
					// Prüfen ob das Land enthalten ist
					foreach (string l in germanHoliday.Land)
					{
						if (hsr.GetById(land).Equals(l))
						{
							return germanHoliday.Feiertag;
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

		public void UpdateHolidayOnCacheById(int id)
		{
			var tmpHol = GetGermanHolidayConverted(DbSet.Where(g => g.Id == id).First());
			GetAll()[id] = tmpHol;

			List<int> acceptedIdsByMonth = GetAllByMonthAccepted(tmpHol.DatumConverted.Value.Month);

			if (tmpHol.Festgelegt && !acceptedIdsByMonth.Contains(id))
			{
				acceptedIdsByMonth.Add(id);
			}
			if (!tmpHol.Festgelegt && acceptedIdsByMonth.Contains(id))
			{
				acceptedIdsByMonth.Remove(id);
			}
		}
	}
}