using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Repositories;

namespace XiopiaWorkTimeTracker.Models.Database
{
    public class WorkTimeTrackerInitializer : System.Data.Entity.DropCreateDatabaseAlways<WorkTimeTrackerDbContext>
    {
        protected override void Seed(WorkTimeTrackerDbContext context)
        {
            var rolesRepo = new RolesRepository();

            var roles = new List<WorkTimeRole>
            {
                new WorkTimeRole{ Name = "User"},
                new WorkTimeRole{ Name = "Accounting" },
                new WorkTimeRole{ Name = "Admin"},
                new WorkTimeRole{ Name = "ProjectSupervisor"}
            };
            roles.ForEach(r => rolesRepo.Add(r));
            rolesRepo.SaveChanges();

            var usersRepo = new UserRepository();
            var emp1 = new Employee { Account = "jschander", Password = "jschander", FirstName = "Juri", LastName = "Schander", Email = "juri.schander@xiopia.com"};
            emp1.AddRole(rolesRepo.GetByName("User").Id);
            emp1.AddRole(rolesRepo.GetByName("Accounting").Id);
            emp1.AddRole(rolesRepo.GetByName("Admin").Id);
            emp1.AddRole(rolesRepo.GetByName("ProjectSupervisor").Id);
            usersRepo.Add(emp1);

            var emp2 = new Employee { Account = "balpay", Password = "balpay", FirstName = "Alpay", LastName = "Bir", Email = "alpay.bir@xiopia.com" };
            emp2.AddRole(rolesRepo.GetByName("User").Id);
            emp2.AddRole(rolesRepo.GetByName("Accounting").Id);
            emp2.AddRole(rolesRepo.GetByName("Admin").Id);
            emp2.AddRole(rolesRepo.GetByName("ProjectSupervisor").Id);
            usersRepo.Add(emp2);

            var emp3 = new Employee { Account = "srampitsch", Password = "srampitsch", FirstName = "Silke", LastName = "Rampitsch", Email = "silke.rampitsch@xiopia.com"};
            emp3.AddRole(rolesRepo.GetByName("User").Id);
            emp3.AddRole(rolesRepo.GetByName("Accounting").Id);
            usersRepo.Add(emp3);

            var emp4 = new Employee { Account = "wkern", Password = "wkern", FirstName = "Winfried", LastName = "Kern", Email = "winfrid.kern@xiopia.com" };
            usersRepo.Add(emp4);

            var emp5 = new Employee { Account = "dschwaier", Password = "dschwaier", FirstName = "Dean", LastName = "Schwaier", Email = "dean.schwaier@xiopia.com"};
            emp5.AddRole(rolesRepo.GetByName("User").Id);
            emp5.AddRole(rolesRepo.GetByName("ProjectSupervisor").Id);
            usersRepo.Add(emp5);

            var emp6 = new Employee { Account = "aberni", Password = "aberni", FirstName = "Attilio", LastName = "Berni", Email = "attilio.berni@xiopia.com" };
            emp6.AddRole(rolesRepo.GetByName("User").Id);
            emp6.AddRole(rolesRepo.GetByName("Accounting").Id);
            emp6.AddRole(rolesRepo.GetByName("ProjectSupervisor").Id);
            usersRepo.Add(emp6);

            usersRepo.SaveChanges();

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
            var settings = new List<Setting>
            {
                new Setting{ DaysAweek = 5, HoursAday = 8, HoursAweek = 40, MonthsAyear=12, VacationDays=30},
            };
            settings.ForEach(r => context.GlobalSettings.Add(r));
            context.SaveChanges();
        }
    }
}