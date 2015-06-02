using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XiopiaWorkTimeTracker.Models.Repositories;
using System.Linq;
using System.Web;

namespace XiopiaWorkTimeTracker.Models.Database
{
	public class GermanHoliday
	{

		public enum Feiertagsarten
		{
			Fest = 1,
			Beweglich = 2
		}
		public int Id { get; set; }

		[Required]
		public string Feiertag { get; set; }

		public DateTime? DatumConverted { get; set; }

		public string Datum { get; set; }

		public int TageHinzu { get; set; }

		[Required]
		public Feiertagsarten FeiertagsArt { get; set; }

		public bool Festgelegt { get; set; }


		public virtual List<int> LänderId { get; set; }

		public List<string> Land
		{
			get
			{
				List<string> names = new List<string>();
				var holidayToStateRepo = new HolidayToStateRepository();

				var lands = holidayToStateRepo.GetGermanStatesByHolidayId(this.Id);
				foreach (var l in lands)
				{
					names.Add(l.Land);
				}
				return names;
			}
		}

		public void AddGermanStateToHoliday(int stateId)
		{
			var statesRepo = new HolidayStatesRepository();
			var state = statesRepo.GetById(stateId);
			var holidayToStateRepo = new HolidayToStateRepository();
			holidayToStateRepo.Add(new HolidayToStateMapping() { GermanStateId = state.Id, GermanHolidayId = this.Id });
			holidayToStateRepo.SaveChanges();
		}

		public void RemoveGermanStateFromHoliday(int stateId)
		{
			var holidayMapping = new HolidayToStateMapping()
			{
				GermanHolidayId = this.Id,
				GermanStateId = stateId
			};

			var statesRepo = new HolidayStatesRepository();
			var state = statesRepo.GetById(stateId);
			var holidayToStateRepo = new HolidayToStateRepository();
			holidayToStateRepo.SetDeleted(holidayMapping);
			holidayToStateRepo.SaveChanges();
		}

	}
}