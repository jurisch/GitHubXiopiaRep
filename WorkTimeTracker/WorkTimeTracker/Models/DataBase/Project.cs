﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkTimeTracker.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ProjectResponsibleId { get; set; }

        public virtual ICollection<int> Members { get; set; }
    }
}