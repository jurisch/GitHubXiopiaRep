using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models;

namespace XiopiaWorkTimeTracker.BusinessLogic
{
    public class WorkTimesBuilder
    {
        public static List<DateRowModel> GetMonth(int month)
        {
            var DateRows = new List<DateRowModel>();
            var daysCount = DateTime.DaysInMonth(DateTime.Now.Year, month);
            for (int i = 1; i <= daysCount; i++)
            {
                DateRows.Add(new DateRowModel() {
                    WorkDate = new DateTime(DateTime.Now.Year, month, i)
                });
            }
            return DateRows;

        }
    }
}