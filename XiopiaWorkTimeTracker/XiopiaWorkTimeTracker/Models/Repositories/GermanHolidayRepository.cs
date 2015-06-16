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
		private static Dictionary<DateTime, GermanHoliday> _allHolidaysAcceptedByDate;

		static GermanHolidayRepository()
		{
			_allHolidays = new Dictionary<int, GermanHoliday>();
			_allHolidaysPerMonthAcceptedById = new Dictionary<int, List<int>>();
			_allHolidaysAcceptedByDate = new Dictionary<DateTime, GermanHoliday>();

		}

		public Dictionary<Int32, GermanHoliday> AllHolidays
		{
			get
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
		}

		public Dictionary<int, List<int>> AllHolidaysByMonthAccepted
		{
			get
			{
				if (_allHolidaysPerMonthAcceptedById == null || _allHolidaysPerMonthAcceptedById.Count == 0)
				{
					Dictionary<int, List<int>> temp = new Dictionary<int, List<int>>();
					for (int month = 1; month < 13; month++)
					{
						temp.Add(month, new List<int>());
					}
					foreach (GermanHoliday germanHoliday in AllHolidays.Values)
					{
						if (germanHoliday.Festgelegt)
						{
							temp[germanHoliday.DatumConverted.Value.Month].Add(germanHoliday.Id);
						}
					}
					_allHolidaysPerMonthAcceptedById = temp;
				}

				return _allHolidaysPerMonthAcceptedById;
			}
		}

		public Dictionary<DateTime, GermanHoliday> AllHolidaysAcceptedByDate
		{
			get
			{
				if (_allHolidaysAcceptedByDate == null || _allHolidaysAcceptedByDate.Count == 0)
				{
					Dictionary<DateTime, GermanHoliday> temp = new Dictionary<DateTime, GermanHoliday>();
					foreach (GermanHoliday germanHoliday in AllHolidays.Values)
					{
						if (germanHoliday.Festgelegt)
						{
							temp.Add(germanHoliday.DatumConverted.Value, germanHoliday);
						}
					}
					_allHolidaysAcceptedByDate = temp;
				}

				return _allHolidaysAcceptedByDate;
			}
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

		public Boolean TryGetFeiertag(DateTime datum, int land, out String name)
		{
			if (AllHolidaysAcceptedByDate.ContainsKey(datum))
			{
				// Liste der Feiertage durchgehen
				GermanHoliday germanHoliday = AllHolidaysAcceptedByDate[datum];
				if (germanHoliday.Land.Count == 0)
				{
					name = germanHoliday.Feiertag;
					return true;
				}

				HolidayStatesRepository hsr = new HolidayStatesRepository();
				// Prüfen ob das Land enthalten ist
				String landName = hsr.GetById(land).Land;
				if (!String.IsNullOrEmpty(landName) && germanHoliday.Land.Contains(landName))
				{
					name = germanHoliday.Feiertag;
					return true;
				}
			}

			name = String.Empty;
            return false;
		}

		public void UpdateHolidayOnCacheById(int id)
		{
			var tmpHol = GetGermanHolidayConverted(DbSet.Where(g => g.Id == id).First());
			GetAll()[id] = tmpHol;

			List<int> acceptedIdsByMonth = AllHolidaysByMonthAccepted[tmpHol.DatumConverted.Value.Month];

			if (tmpHol.Festgelegt)
			{
				if (!acceptedIdsByMonth.Contains(id))
					acceptedIdsByMonth.Add(id);
				if (!AllHolidaysAcceptedByDate.ContainsKey(tmpHol.DatumConverted.Value))
					AllHolidaysAcceptedByDate.Add(tmpHol.DatumConverted.Value, tmpHol);
			}
			if (!tmpHol.Festgelegt)
			{
				if (acceptedIdsByMonth.Contains(id))
					acceptedIdsByMonth.Remove(id);
				if (AllHolidaysAcceptedByDate.ContainsKey(tmpHol.DatumConverted.Value))
					AllHolidaysAcceptedByDate.Remove(tmpHol.DatumConverted.Value);
			}
		}
	}
}