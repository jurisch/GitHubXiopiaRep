using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkTimeTracker.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public virtual ICollection<int> WorkTimeTrackerRoleId { get; set; }
    }
}