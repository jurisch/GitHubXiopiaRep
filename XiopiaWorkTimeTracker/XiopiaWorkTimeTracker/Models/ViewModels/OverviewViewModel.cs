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

        public OverviewViewModel(int userId, int month)
        {
            _month = month;
            var usersRepository = new UserRepository();
            _user = usersRepository.Get(userId);
        }

        public int WorkdaysInMonth
        {
            get
            {
                return 20;
            }
        }
        public int WorkdaysYear { get; set; }

        public int WorkedNormalDays
        {
            get
            {
                int days = 0;
                var monthEntries = _user.GetTimeEntriesForMonth(_month);
                foreach (var entry in monthEntries)
                {
                    if (entry.WorkStartTime.HasValue && entry.WorkEndTime.HasValue)
                    {
                        days++;
                    }
                }
                return days;
            }
        }

        public int WorkedOtherDays
        {
            get
            {
                return 0;
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
    }
}