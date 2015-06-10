using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace LDAPTest.Models
{
	public class Account
	{
		public int ID { get; set; }
        public Guid Guid { get; private set; }
		public string Username { get; set; }
		public string Name { get; set; }
		public string SecondName { get; set; }
		public string AccountName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }

        public Account()
        {
            this.Guid = Guid.NewGuid();
        }
    }

	public class AccountDBContext : DbContext
	{
		public DbSet<Account> Person { get; set; }
	}

}