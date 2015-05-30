using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Database;
using XiopiaWorkTimeTracker.Models.Repositories;

namespace XiopiaWorkTimeTracker.Models.ViewModels
{
	public class FeierTag
	{
		private int id;
		private string art;
		private string feiertag;
		private DateTime datum;
		private string testDatum;
		private int tageHinzu;
		private List<string> länder;
		private bool festgelegt;


		public FeierTag() {
			Initial();
		}

		public int Id
		{
			get
			{
				return this.id;
			}
		}

		public String Feiertag
		{
			get
			{
				return this.feiertag;
			}
		}
		public DateTime Datum
		{
			get
			{
				if (null == testDatum) return GetDatum(GetOstersonntag(DateTime.Now.Year), this.tageHinzu);
				else
					
					return DateTime.Parse(this.testDatum +""+DateTime.Now.Year);
			}
		}
		public List<string> Länder
		{
			get
			{
				return länder;
			}
		}

		public string Art
		{
			get
			{
				return this.art;
			}
		}

		public bool Festgelegt
		{
			get
			{
				return this.festgelegt;
			}
		}



		public List<FeierTag> feiertage = new List<FeierTag>();

		public String GetFeiertag(DateTime datum, int land)
		{

			// Liste der Feiertage durchgehen
			foreach (FeierTag f in feiertage)
			{
				if (datum.ToShortDateString().Equals(f.GetDatum(GetOstersonntag(datum.Year), 0).ToShortDateString()))
				{
					HolidayStatesRepository hsr = new HolidayStatesRepository();
					// Prüfen ob das Land enthalten ist
					foreach (string l in f.Länder)
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

		public DateTime GetOstersonntag(int jahr)
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

		public DateTime GetDatum(DateTime osterSonntag, int tageHinzu)
		{
			HolidaysTypRepository typ = new HolidaysTypRepository();
			if (!art.Equals(typ.GetById(1).FeiertagsArt))
			{
				datum = osterSonntag.AddDays(tageHinzu);
			}
			else
			{
				datum = DateTime.Parse(testDatum + "." + osterSonntag.Year);
			}

			return DateTime.Parse(datum.Day.ToString() + "." + datum.Month.ToString() + "." + osterSonntag.Year);
		}


		private void Initial()
		{
			GermanHolidayRepository ghr = new GermanHolidayRepository();
			HolidayToStateRepository hsr = new HolidayToStateRepository();

			List<GermanHoliday> allHolidays = ghr.GetAll();
			foreach (GermanHoliday h in allHolidays)
			{
				feiertage.Add(new FeierTag(h));
			}
		}

		internal FeierTag(GermanHoliday h)
		{
			HolidaysTypRepository typ = new HolidaysTypRepository();
			this.id = h.Id;
			this.feiertag = h.Feiertag;
			this.testDatum = h.Datum;
			this.tageHinzu = h.TageHinzu;
			this.art = typ.GetById(h.FeiertagsArt).FeiertagsArt;
			this.länder = new List<string>();
			if (h.Land.Count < 1) this.länder.Add("Alle Deutschen Bundesländer");
			else
				foreach (var gs in h.Land) {
				
					this.länder.Add(gs);
				}
			this.festgelegt = h.Festgelegt;
		}

	}
}