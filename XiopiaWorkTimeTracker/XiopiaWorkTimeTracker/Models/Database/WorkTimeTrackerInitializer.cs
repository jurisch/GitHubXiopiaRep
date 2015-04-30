using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XiopiaWorkTimeTracker.Models.Database
{
    public class WorkTimeTrackerInitializer : System.Data.Entity.DropCreateDatabaseAlways<WorkTimeTrackerDbContext>
    {
        protected override void Seed(WorkTimeTrackerDbContext context)
        {
            var users = new List<Employee>
            {
                new Employee{ Account = "jschander", Password="jschander", FirstName = "Juri", LastName = "Schander", Email = "juri.schander@xiopia.com"},
                new Employee{ Account = "srampitsch", Password="srampitsch", FirstName = "Silke", LastName = "Rampitsch", Email = "silke.rampitsch@xiopia.com"},
                new Employee{ Account = "balpay", Password="balpay", FirstName = "Alpay", LastName = "Bir", Email = "alpay.bir@xiopia.com"},
                new Employee{ Account = "wkern", Password="wkern", FirstName = "Winfried", LastName = "Kern", Email = "winfrid.kern@xiopia.com"},
                new Employee{ Account = "dschwaier", Password="dschwaier", FirstName = "Dean", LastName = "Schwaier", Email = "dean.schwaier@xiopia.com"},
                new Employee{ Account = "aberni", Password="aberni", FirstName = "Attilio", LastName = "Berni", Email = "attilio.berni@xiopia.com"}
            };
            users.ForEach(r => context.Employees.Add(r));
            context.SaveChanges();
            var roles = new List<WorkTimeRole>
            {
                new WorkTimeRole{ Name = "User"},
                new WorkTimeRole{ Name = "Accounting" },
                new WorkTimeRole{ Name = "Admin"},
                new WorkTimeRole{ Name = "Project Supervisor"}
            };
            roles.ForEach(r => context.WorkTimeTrackerRoles.Add(r));
            context.SaveChanges();
            var projects = new List<Project>
            {
                new Project{ Name = "Urlaub", ProjectResponsibleId = 0},
                new Project{ Name = "Krank" , ProjectResponsibleId = 0},
                new Project{ Name = "Momentan ohne Projekt", ProjectResponsibleId = 0},
                new Project{ Name = "Weiterbildung", ProjectResponsibleId = 0},
                new Project{ Name = "Business Development", ProjectResponsibleId = 0},
                new Project{ Name = "Personalmanagement", ProjectResponsibleId = 0},
                new Project{ Name = "Vorstellungsgespräche", ProjectResponsibleId = 0}
            };
            projects.ForEach(r => context.Projects.Add(r));
            context.SaveChanges();
            var settings = new List<Setting>
            {
                new Setting{ Name = "WeekFirstDay", Value = "1"},
                new Setting{ Name = "WeekLastDay", Value = "5"},
            };
            settings.ForEach(r => context.GlobalSettings.Add(r));
            context.SaveChanges();
        }
    }
}