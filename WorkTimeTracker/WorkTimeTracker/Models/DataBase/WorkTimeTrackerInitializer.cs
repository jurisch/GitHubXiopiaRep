using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkTimeTracker.Models.DataBase
{
    public class WorkTimeTrackerInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<WorkTimeTrackerDbContext>
    {
        protected override void Seed(WorkTimeTrackerDbContext context)
        {
            var users = new List<Employee>
            {
                new Employee{ Account = "jschander", FirstName = "Juri", LastName = "Schander", Email = "juri.schander@xiopia.com"},
                new Employee{ Account = "srampitsch", FirstName = "Silke", LastName = "Rampitsch", Email = "silke.rampitsch@xiopia.com"},
                new Employee{ Account = "balpay", FirstName = "Alpay", LastName = "Bir", Email = "alpay.bir@xiopia.com"},
                new Employee{ Account = "wkern", FirstName = "Winfried", LastName = "Kern", Email = "winfrid.kern@xiopia.com"},
                new Employee{ Account = "dschwaier", FirstName = "Dean", LastName = "Schwaier", Email = "dean.schwaier@xiopia.com"},
                new Employee{ Account = "aberni", FirstName = "Attilio", LastName = "Berni", Email = "attilio.berni@xiopia.com"}
            };
            //users.ForEach(r => context.Employees.Add(r));
            //context.SaveChanges();
            //var roles = new List<WorkTimeTrackerRole>
            //{
            //    new WorkTimeTrackerRole{ Name = "User"},
            //    new WorkTimeTrackerRole{ Name = "Accounting" },
            //    new WorkTimeTrackerRole{ Name = "Admin"},
            //    new WorkTimeTrackerRole{ Name = "Project Supervisor"}
            //};
            //roles.ForEach(r => context.WorkTimeTrackerRoles.Add(r));
            //context.SaveChanges();
            //var projects = new List<Project>
            //{
            //    new Project{ Name = "Urlaub", ProjectResponsibleId = 0},
            //    new Project{ Name = "Krank" , ProjectResponsibleId = 0},
            //    new Project{ Name = "Momentan ohne Projekt", ProjectResponsibleId = 0},
            //    new Project{ Name = "Weiterbildung", ProjectResponsibleId = 0},
            //    new Project{ Name = "Business Development", ProjectResponsibleId = 0},
            //    new Project{ Name = "Personalmanagement", ProjectResponsibleId = 0},
            //    new Project{ Name = "Vorstellungsgespräche", ProjectResponsibleId = 0}
            //};
            //projects.ForEach(r => context.Projects.Add(r));
            //context.SaveChanges();
        }

    }
}