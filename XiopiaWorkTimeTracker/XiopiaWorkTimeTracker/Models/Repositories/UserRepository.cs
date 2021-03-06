﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.Repositories
{
    public class UserRepository : Repository<Employee>
    {
        public Employee GetByGuid(Guid guid)
        {
            return DbSet.Where(u => u.Guid == guid).First();
        }

        public Employee GetByAccount(string acc)
        {
            return DbSet.Where(ac => ac.Account.Equals(acc)).First();
        }

        //public void AddRole(int userId, int roleId)
        //{
        //    var user = DbSet.Add(userId);
        //}
    }
}