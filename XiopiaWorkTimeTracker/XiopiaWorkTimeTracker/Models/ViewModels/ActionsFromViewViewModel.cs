using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XiopiaWorkTimeTracker.Models.ViewModels
{
    public class ActionsFromViewViewModel
    {
        public int UserId { get; set; }
        public string Action { get; set; }
        public int ProjectId { get; set; }
        public string ElementId { get; set; }
        public string Element { get; set; }
        public string Value { get; set; }
        public string AttrIll { get; set; }
        public string AttrHoliday { get; set; }
    }
}