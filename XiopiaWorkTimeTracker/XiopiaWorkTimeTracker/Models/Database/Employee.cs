using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Repositories;

namespace XiopiaWorkTimeTracker.Models.Database
{
    public class Employee
    {
        public Employee()
        {
            this.Guid = Guid.NewGuid();
            this.workTimeRepo = new WorkTimeEntriesRepository();
        }

        private WorkTimeEntriesRepository workTimeRepo = null;

        public int Id { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public Guid Guid { get; private set; }

        public virtual List<int> WorkTimeRoleIds { get; set; }

        public void AddRole(int roleId)
        {
            var userToRoleMapRepo = new UserToRoleRepository();
            var roleMapping = new UserToRoleMapping() {
                EmployeeGuid = this.Guid,
                WorkTimeRoleId = roleId
            };
            userToRoleMapRepo.Add(roleMapping);
            userToRoleMapRepo.SaveChanges();
        }

        public bool HasRole(string roleStr)
        {
            var rolesRepo = new RolesRepository();
            var role = rolesRepo.GetByName(roleStr);
            var userToRoleMapRepo = new UserToRoleRepository();
            var allRoles = userToRoleMapRepo.GetByUserGuid(this.Guid);
            foreach(var us in allRoles)
            {
                if (us.WorkTimeRoleId == role.Id)
                    return true;
            }
            return false;
        }

        public List<WorkTimeEntry> GetTimeEntriesForMonth(int month)
        {
            return this.workTimeRepo.GetUserEntriesForMonth(this.Id, month);
        }

        public WorkTimeEntry GetTimeEntryByid(int entryId)
        {
            return this.workTimeRepo.Get(entryId);
        }

        public void AddTimeEntry(WorkTimeEntry entry)
        {
            entry.EmployeeId = this.Id;
            this.workTimeRepo.Add(entry);
            this.workTimeRepo.SaveChanges();
        }

        public List<WorkTimeEntry> GetTodayStartedEntries()
        {
            return this.workTimeRepo.GetUserTodayStartedEntries(this.Id);
        }

        public void UpdateTimeEntry(WorkTimeEntry entry)
        {
            this.workTimeRepo.SetModified(entry);
            this.workTimeRepo.SaveChanges();
        }
    }
}