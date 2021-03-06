﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace XiopiaWorkTimeTracker.Models.Database
{
    public class UserToRoleMapping
    {
        public int Id { get; set; }

        [Required]
        public Guid EmployeeGuid { get; set; }

        [Required]
        public int WorkTimeRoleId { get; set; }
    }
}