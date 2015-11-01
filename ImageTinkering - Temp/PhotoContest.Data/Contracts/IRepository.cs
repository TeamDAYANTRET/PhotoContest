using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoContest.Data.Contracts
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IQueryable<T> All();

        T GetById(object id);

        T Add(T entity);

        T Update(T entity);

        void Delete(T entity);

        void Delete(object id);

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
