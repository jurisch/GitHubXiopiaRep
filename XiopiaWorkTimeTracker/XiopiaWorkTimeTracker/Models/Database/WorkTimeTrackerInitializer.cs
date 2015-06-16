using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Repositories;

namespace XiopiaWorkTimeTracker.Models.Database
{
    public class WorkTimeTrackerInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<WorkTimeTrackerDbContext>
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
            var emp1 = new Employee { Account = "jschander", Password = "jschander", FirstName = "Juri", LastName = "Schander", Email = "juri.schander@xiopia.com" };
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

            var emp3 = new Employee { Account = "srampitsch", Password = "srampitsch", FirstName = "Silke", LastName = "Rampitsch", Email = "silke.rampitsch@xiopia.com" };
            emp3.AddRole(rolesRepo.GetByName("User").Id);
            emp3.AddRole(rolesRepo.GetByName("Accounting").Id);
            usersRepo.Add(emp3);

            var emp4 = new Employee { Account = "wkern", Password = "wkern", FirstName = "Winfried", LastName = "Kern", Email = "winfrid.kern@xiopia.com" };
            usersRepo.Add(emp4);

            var emp5 = new Employee { Account = "dschwaier", Password = "dschwaier", FirstName = "Dean", LastName = "Schwaier", Email = "dean.schwaier@xiopia.com" };
            emp5.AddRole(rolesRepo.GetByName("User").Id);
            emp5.AddRole(rolesRepo.GetByName("ProjectSupervisor").Id);
            usersRepo.Add(emp5);

            var emp6 = new Employee { Account = "aberni", Password = "aberni", FirstName = "Attilio", LastName = "Berni", Email = "attilio.berni@xiopia.com" };
            emp6.AddRole(rolesRepo.GetByName("User").Id);
            emp6.AddRole(rolesRepo.GetByName("Accounting").Id);
            emp6.AddRole(rolesRepo.GetByName("ProjectSupervisor").Id);
            usersRepo.Add(emp6);

            usersRepo.SaveChanges();

            var projectsRepo = new ProjectsRepository();
            projectsRepo.Add(new Project { Name = "Momentan ohne Projekt", ProjectResponsible = "Default" });
            projectsRepo.Add(new Project { Name = "Weiterbildung", ProjectResponsible = "Default" });
            projectsRepo.Add(new Project { Name = "Business Development", ProjectResponsible = "Default" });
            projectsRepo.Add(new Project { Name = "Personalmanagement", ProjectResponsible = "Default" });
            projectsRepo.Add(new Project { Name = "Vorstellungsgespräche", ProjectResponsible = "Default" });
            projectsRepo.SaveChanges();

            var settings = new List<Setting>
            {
                new Setting{ DaysAweek = 5, HoursAday = 8, HoursAweek = 40, MonthsAyear=12, VacationDays=30, GermanStateId=2},
            };
            settings.ForEach(r => context.GlobalSettings.Add(r));
            context.SaveChanges();

			//Start: Test Data For GermanHolidays 
			var holidayStatesRepository = new HolidayStatesRepository();
			holidayStatesRepository.Add(new GermanState { Land = "Baden_Würtenberg" });
			holidayStatesRepository.Add(new GermanState { Land = "Bayern" });
			holidayStatesRepository.Add(new GermanState { Land = "Berlin" });
			holidayStatesRepository.Add(new GermanState { Land = "Brandenburg" });
			holidayStatesRepository.Add(new GermanState { Land = "Bremen" });
			holidayStatesRepository.Add(new GermanState { Land = "Hamburg" });
			holidayStatesRepository.Add(new GermanState { Land = "Hessen" });
			holidayStatesRepository.Add(new GermanState { Land = "Mecklenburg_Vorpommern" });
			holidayStatesRepository.Add(new GermanState { Land = "Niedersachsen" });
			holidayStatesRepository.Add(new GermanState { Land = "Nordrhein_Westfalen" });
			holidayStatesRepository.Add(new GermanState { Land = "Rheinland_Pfalz" });
			holidayStatesRepository.Add(new GermanState { Land = "Saarland" });
			holidayStatesRepository.Add(new GermanState { Land = "Sachsen" });
			holidayStatesRepository.Add(new GermanState { Land = "Sachsen_Anhalt" });
			holidayStatesRepository.Add(new GermanState { Land = "Schleswig_Holstein" });
			holidayStatesRepository.Add(new GermanState { Land = "Thüringen" });
			holidayStatesRepository.SaveChanges();

			var holidaysTypRepository = new HolidaysTypRepository();
			holidaysTypRepository.Add(new HolidayTyp { Id = GermanHoliday.Feiertagsarten.Fest, FeiertagsArt = "Fester Feiertag" });
			holidaysTypRepository.Add(new HolidayTyp { Id = GermanHoliday.Feiertagsarten.Beweglich, FeiertagsArt = "Bewegliche Feiertag" });
			holidaysTypRepository.SaveChanges();

			var germanHolidayRepository = new GermanHolidayRepository();
			var gh1 = new GermanHoliday { Feiertag = "Neujahr", Datum = "01.01.", FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Fest).Id, Festgelegt=true};
			var gh2 = new GermanHoliday { Feiertag = "Heiligen Drei Könige", Datum = "06.01.", FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Fest).Id, Festgelegt = true };

			var bw1 = new GermanHoliday { Feiertag = "Fastnacht", TageHinzu = -47, FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Beweglich).Id, Festgelegt = false };
			//var bw2 = new GermanHoliday { Feiertag = "Friedensfest", TageHinzu = -2, FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Beweglich).Id, Festgelegt = false };
			//var bw3 = new GermanHoliday { Feiertag = "Reformationstag", TageHinzu = -2, FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Beweglich).Id, Festgelegt = false };
			//var bw4 = new GermanHoliday { Feiertag = "Volkstrauertag", TageHinzu = -2, FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Beweglich).Id, Festgelegt = false };
			//var bw5 = new GermanHoliday { Feiertag = "Buß und Bettag", TageHinzu = -2, FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Beweglich).Id, Festgelegt = false };
			//var bw6 = new GermanHoliday { Feiertag = "Totensonntag", TageHinzu = -2, FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Beweglich).Id, Festgelegt = false };
			//var bw7 = new GermanHoliday { Feiertag = "1. Advent", TageHinzu = -2, FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Beweglich).Id, Festgelegt = false };
			//var bw8 = new GermanHoliday { Feiertag = "2. Advent", TageHinzu = -2, FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Beweglich).Id, Festgelegt = false };
			//var bw9 = new GermanHoliday { Feiertag = "3. Advent", TageHinzu = -2, FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Beweglich).Id, Festgelegt = false };
			//var bw10 = new GermanHoliday { Feiertag = "4. Advent", TageHinzu = -2, FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Beweglich).Id, Festgelegt = false };

			var gh3 = new GermanHoliday { Feiertag = "Karfreitag", TageHinzu = -2, FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Beweglich).Id, Festgelegt = true };
			var gh4 = new GermanHoliday { Feiertag = "Ostersonntag", TageHinzu = 0, FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Beweglich).Id, Festgelegt = true };
			var gh5 = new GermanHoliday { Feiertag = "Ostermontag", TageHinzu = 1, FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Beweglich).Id, Festgelegt = true };
			var gh6 = new GermanHoliday { Feiertag = "Tag der Arbeit", Datum = "01.05.", FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Fest).Id, Festgelegt = true };
			var gh7 = new GermanHoliday { Feiertag = "Christi Himmelfahrt", TageHinzu = 39, FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Beweglich).Id, Festgelegt = true };
			var gh8 = new GermanHoliday { Feiertag = "Pfingstsonntag", TageHinzu = 49, FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Beweglich).Id, Festgelegt = true };
			var gh9 = new GermanHoliday { Feiertag = "Pfingstmontag", TageHinzu = 50, FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Beweglich).Id, Festgelegt = true };
			var gh10 = new GermanHoliday { Feiertag = "Fronleichnam", TageHinzu = 60, FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Beweglich).Id, Festgelegt = true };
			var gh11 = new GermanHoliday { Feiertag = "Mariä Himmelfahrt", Datum = "15.08.", FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Fest).Id, Festgelegt = true };
			var gh12 = new GermanHoliday { Feiertag = "Tag der dt. Einheit", Datum = "03.10.", FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Fest).Id, Festgelegt = true };
			var gh13 = new GermanHoliday { Feiertag = "Allerheiligen", Datum = "01.11.", FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Fest).Id, Festgelegt = true };
			var gh14 = new GermanHoliday { Feiertag = "1. Weinachtstag", Datum = "25.12.", FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Fest).Id, Festgelegt = true };
			var gh15 = new GermanHoliday { Feiertag = "2. Weinachtstag", Datum = "26.12.", FeiertagsArt = holidaysTypRepository.GetById(GermanHoliday.Feiertagsarten.Fest).Id, Festgelegt = true };
			germanHolidayRepository.Add(gh1);germanHolidayRepository.Add(gh2); germanHolidayRepository.Add(bw1); germanHolidayRepository.Add(gh3);
			germanHolidayRepository.Add(gh4);
			germanHolidayRepository.Add(gh5);germanHolidayRepository.Add(gh6);germanHolidayRepository.Add(gh7);germanHolidayRepository.Add(gh8);
			germanHolidayRepository.Add(gh9);germanHolidayRepository.Add(gh10);germanHolidayRepository.Add(gh11);germanHolidayRepository.Add(gh12);
			germanHolidayRepository.Add(gh13);germanHolidayRepository.Add(gh14);germanHolidayRepository.Add(gh15);
			germanHolidayRepository.SaveChanges();


			var gHolidayRepository = germanHolidayRepository.GetAll();

			germanHolidayRepository.AllHolidays[2].AddGermanStateToHoliday(1); germanHolidayRepository.AllHolidays[2].AddGermanStateToHoliday(2); germanHolidayRepository.AllHolidays[2].AddGermanStateToHoliday(14);
			germanHolidayRepository.AllHolidays[10].AddGermanStateToHoliday(1); germanHolidayRepository.AllHolidays[10].AddGermanStateToHoliday(2); germanHolidayRepository.AllHolidays[10].AddGermanStateToHoliday(7);
			germanHolidayRepository.AllHolidays[10].AddGermanStateToHoliday(10); germanHolidayRepository.AllHolidays[10].AddGermanStateToHoliday(11); germanHolidayRepository.AllHolidays[10].AddGermanStateToHoliday(12);
			germanHolidayRepository.AllHolidays[11].AddGermanStateToHoliday(12);
			germanHolidayRepository.AllHolidays[13].AddGermanStateToHoliday(1); germanHolidayRepository.AllHolidays[13].AddGermanStateToHoliday(2); germanHolidayRepository.AllHolidays[13].AddGermanStateToHoliday(10);
			germanHolidayRepository.AllHolidays[13].AddGermanStateToHoliday(11); germanHolidayRepository.AllHolidays[13].AddGermanStateToHoliday(12);
			germanHolidayRepository.SaveChanges();
			//End: Test Data For GermanHolidays 

		}

	}
}