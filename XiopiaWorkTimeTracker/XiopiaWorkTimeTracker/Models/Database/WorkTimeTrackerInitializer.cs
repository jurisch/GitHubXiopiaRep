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
                new WorkTimeRole{ Name = "Project Supervisor"}
            };
            roles.ForEach(r => rolesRepo.Add(r));
            rolesRepo.SaveChanges();

            //List<WorkTimeRole> rolesList = new List<WorkTimeRole>();
            //var roleUser = rolesRepo.GetByName("User");
            //var roleAccount = rolesRepo.GetByName("Accounting");
            //var roleAdmin = rolesRepo.GetByName("Admin");
            //var roleProj = rolesRepo.GetByName("Project Supervisor");
            //rolesList.Add(roleUser);
            //rolesList.Add(roleAccount);
            //rolesList.Add(roleAdmin);
            //rolesList.Add(roleProj);



            var usersRepo = new UserRepository();
            //var users = new List<Employee>();
            // User with all roles
            var emp1 = new Employee { Account = "jschander", Password = "jschander", FirstName = "Juri", LastName = "Schander", Email = "juri.schander@xiopia.com"};
            usersRepo.Add(emp1);
            usersRepo.SaveChanges();
            UserToRoleMapping roleMapping = new UserToRoleMapping() {
                EmployeeId = usersRepo.GetByAccount("jschander").Id,
                WorkTimeRoleId = rolesRepo.GetByName("User").Id
            };
            var map = new UserToRoleRepository();
            map.Add(roleMapping);
            map.SaveChanges();

            //users.Add(emp1);
            //var emp3 = new Employee { Account = "balpay", Password = "balpay", FirstName = "Alpay", LastName = "Bir", Email = "alpay.bir@xiopia.com", WorkTimeRoles = rolesList };
            //users.Add(emp3);

            //var emp2 = new Employee { Account = "srampitsch", Password = "srampitsch", FirstName = "Silke", LastName = "Rampitsch", Email = "silke.rampitsch@xiopia.com", WorkTimeRoles = rolesList };
            //users.Add(emp2);
            //var emp4 = new Employee { Account = "wkern", Password = "wkern", FirstName = "Winfried", LastName = "Kern", Email = "winfrid.kern@xiopia.com" };
            //users.Add(emp4);
            //var emp5 = new Employee { Account = "dschwaier", Password = "dschwaier", FirstName = "Dean", LastName = "Schwaier", Email = "dean.schwaier@xiopia.com", WorkTimeRoles = rolesList };
            //users.Add(emp5);
            //var emp6 = new Employee { Account = "aberni", Password = "aberni", FirstName = "Attilio", LastName = "Berni", Email = "attilio.berni@xiopia.com", WorkTimeRoles = rolesList };
            //users.Add(emp6);
            //users.ForEach(r => usersRepo.Add(r));
            //usersRepo.SaveChanges();

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
            //var settings = new List<Setting>
            //{
            //    new Setting{ DaysAweek = 5, HoursAday = 8, HoursAweek = 40, MonthsAyear=12, VacationDays=30},
            //};
            //settings.ForEach(r => context.GlobalSettings.Add(r));
            //context.SaveChanges();
        }
    }
}