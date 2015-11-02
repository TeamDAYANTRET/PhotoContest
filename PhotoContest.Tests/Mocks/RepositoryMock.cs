using PhotoContest.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoContest.Tests.Mocks
{
    public class RepositoryMock<T> : IRepository<T> where T : class
    {
        public IList<T> Entities { get; set; }
        public bool IsSaveCalled { get; set; }

        public RepositoryMock()
        {
            this.Entities = new List<T>();
        }

        public IQueryable<T> All()
        {
            return this.Entities.AsQueryable();
        }

        public T GetById(object id)
        {
            throw new NotImplementedException();
        }

        public T Add(T entity)
        {
            this.Entities.Add(entity);
            return entity;
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            this.Entities.Remove(entity);
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            this.IsSaveCalled = true;
        }


        int IRepository<T>.SaveChanges()
        {
            return Convert.ToInt32(this.IsSaveCalled = true);
        }

        public Task<int> SaveChangesAsync()
        {
            return Task<int>.Run(() => Convert.ToInt32(this.IsSaveCalled = true));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
