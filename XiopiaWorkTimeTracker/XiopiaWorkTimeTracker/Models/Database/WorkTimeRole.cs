using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace XiopiaWorkTimeTracker.Models.Database
{
    public class WorkTimeRole
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<UserToRoleMapping> UserToRoleMappings { get; set; }

        //public virtual List<Employee> Employees { get; set; }

    }
}