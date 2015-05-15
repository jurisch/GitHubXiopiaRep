using System;
using System.Collections.Generic;

namespace XiopiaWorkTimeTracker.BusinessLogic
{
    public class DateRowModel
    {
        public DateTime WorkDate { get; set; }
        public List<WorkTimeRow> DataRow { get; set; }
    }
}