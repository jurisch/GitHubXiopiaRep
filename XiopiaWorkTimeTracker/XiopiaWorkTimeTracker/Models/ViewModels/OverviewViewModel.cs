using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Database;
using XiopiaWorkTimeTracker.Models.Repositories;

namespace XiopiaWorkTimeTracker.Models.ViewModels
{
    public class OverviewViewModel
    {
        private Employee _user;
        private int _month;
		private List<DateTime> germanHoliday = new List<DateTime>();
		GermanHolidayRepository germanholidayrep = new GermanHolidayRepository();
		List<GermanHoliday> x = new List<GermanHoliday>();

		public OverviewViewModel(int userId, int month)
        {
            _month = month;
            var usersRepository = new UserRepository();
            _user = usersRepository.Get(userId);
			Dictionary<int, GermanHoliday> germanHolidays = germanholidayrep.AllHolidays;
			foreach (var id in germanholidayrep.AllHolidaysByMonthAccepted[month])
			{
				x.Add(germanHolidays[id]);
			}
		}

        public int WorkdaysInMonth
        {
            get
            {
                int wDays = 0;
                var daysCount = DateTime.DaysInMonth(DateTime.Now.Year, _month);

				foreach (GermanHoliday h in x) {
					//germanHoliday.Add(DateTime.Parse(h.Datum+""+DateTime.Now.Year));
					germanHoliday.Add(h.DatumConverted.Value);
				}

                for (int i = 1; i < daysCount + 1; i++ )
                {
					bool holiday = false;
                    var curDay = new DateTime(DateTime.Now.Year, _month, i);
                    if(!curDay.DayOfWeek.ToString("d").Equals("6") && !curDay.DayOfWeek.ToString("d").Equals("0"))
                    {
						foreach (DateTime d in germanHoliday) {
							if (d == curDay)
							{
								holiday = true;
							}
						}
                        if (!holiday) wDays++;
                    }
                }
                return wDays;
            }
        }
        public int WorkdaysYear 
        {
            get
            {
                int wDays = 0;
                for (int j = 1; j < 13; j++)
                {
                    var daysCount = DateTime.DaysInMonth(DateTime.Now.Year, j);
                    for (int i = 1; i < daysCount + 1; i++)
                    {
                        var curDay = new DateTime(DateTime.Now.Year, j, i);
                        if (!curDay.DayOfWeek.ToString("d").Equals("6") && !curDay.DayOfWeek.ToString("d").Equals("0"))
                        {
                            wDays++;
                        }
                    }
                }
                return wDays;
            }
        }

        public int WorkedNormalDays
        {
            get
            {
                int days = 0;
                var monthEntries = _user.GetTimeEntriesForMonth(_month);
                foreach (var entry in monthEntries)
                {
                    if (entry.WorkStartTime.HasValue && entry.WorkEndTime.HasValue &&
                        !entry.WorkDay.DayOfWeek.ToString("d").Equals("0") &&
                        !entry.WorkDay.DayOfWeek.ToString("d").Equals("6"))
                    {
                        days++;
                    }
                }
                return days;
            }
        }

        public float WorkedOtherDays
        {
            get
            {
                int days = 0;
                var monthEntries = _user.GetTimeEntriesForMonth(_month);
                foreach (var entry in monthEntries)
                {
                    if (entry.WorkStartTime.HasValue && entry.WorkEndTime.HasValue &&
                        (entry.WorkDay.DayOfWeek.ToString("d").Equals("0") ||
                        entry.WorkDay.DayOfWeek.ToString("d").Equals("6")))
                    {
                        days++;
                    }
                }
                return days;
            }
        }

        public float WorkedHowrs
        {
            get
            {
                int hours = 0;
                var monthEntries = _user.GetTimeEntriesForMonth(_month);
                foreach (var entry in monthEntries)
                {
                    if (entry.WorkStartTime.HasValue && entry.WorkEndTime.HasValue)
                    {
                        var timeSpan = entry.WorkEndTime.Value - entry.WorkStartTime.Value;
                        hours += timeSpan.Hours * 60 + timeSpan.Minutes;
                        hours -= entry.PauseLength.HasValue ? entry.PauseLength.Value : 0;
                    }
                }
                return (float)hours/60;
            }
        }

        public int IllDays
        {
            get
            {
                int days = 0;
                var monthEntries = _user.GetTimeEntriesForMonth(_month);
                foreach (var entry in monthEntries)
                {
                    if (entry.AttrIll)
                    {
                        days++;
                    }
                }
                return days;
            }
        }

        public int VacationDays
        {
            get
            {
                int days = 0;
                var monthEntries = _user.GetTimeEntriesForMonth(_month);
                foreach (var entry in monthEntries)
                {
                    if (entry.AttrHoliday)
                    {
                        days++;
                    }
                }
                return days;
            }
        }

        public int TargetHowers
        {
            get
            {
                return WorkdaysInMonth * 8;
            }
        }

	}
}