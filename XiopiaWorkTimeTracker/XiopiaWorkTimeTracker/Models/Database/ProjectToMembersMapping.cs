using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XiopiaWorkTimeTracker.Models.Database
{
    public class ProjectToMembersMapping
    {
        public int Id { get; set; }

        public Guid ProjectGuid { get; set; }

        public int MemberId { get; set; }
    }
}