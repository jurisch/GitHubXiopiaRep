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
		private DateTime datumConverted;
		private string datum;
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
		public DateTime DatumConverted
		{
			get
			{
				return datumConverted;

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

		private void Initial()
		{
			GermanHolidayRepository ghr = new GermanHolidayRepository();
			HolidayToStateRepository hsr = new HolidayToStateRepository();

			List<GermanHoliday> allHolidays = ghr.GetAllConverted();
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
			this.datumConverted = h.DatumConverted.Value;
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