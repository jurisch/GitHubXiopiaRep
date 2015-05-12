using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.Repositories
{
    public class Repository<T> where T : class
    {
        private WorkTimeTrackerDbContext context = new WorkTimeTrackerDbContext();

        protected DbSet<T> DbSet
        {
            get;
            set;
        }

        public Repository()
        {
            DbSet = context.Set<T>();
        }

        public void SetModified(T obj)
        {
            context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }

        public void SetDeleted(T obj)
        {
            context.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
        }

        public List<T> GetAll()
        {
            return DbSet.ToList();
        }

        public T Get(int id)
        {
            return DbSet.Find(id);
        }

        public void Add (T entity)
        {
            DbSet.Add(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}