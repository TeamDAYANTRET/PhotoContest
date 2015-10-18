using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoContest.Data.Contracts;
using System.Data.Entity;
using System.Linq;

namespace PhotoContest.Data
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext context;

        private readonly IDbSet<T> set;

        public GenericRepository()
            : this(new PhotoContestDbContext())
        {
        }

        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.set = context.Set<T>();
        }

        public IQueryable<T> All()
        {
            return this.set;
        }

        public T GetById(object id)
        {
            return this.set.Find(id);
        }

        public T Add(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Added);
            return entity;
        }

        public T Update(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Modified);
            return entity;
        }

        public void Delete(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Deleted);
        }

        public void Delete(object id)
        {
            var entity = this.GetById(id);
            this.ChangeEntityState(entity, EntityState.Deleted);
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }
        public Task<int> SaveChangesAsync()
        {
            return this.context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }

        private void ChangeEntityState(T entity, EntityState state)
        {
            var entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.set.Attach(entity);
            }

            entry.State = state;
        }
    }
}
