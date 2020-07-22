using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace FootballApp.API.Data
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void AddRange(ICollection<T> entities);
        void Remove(T entity);
        void RemoveRange(ICollection<T> entities);
        void Update(T entity);
        Task<T> GetById(int id);
        Task<ICollection<T>> GetAll();
        Task<ICollection<T>> Find(Expression<Func<T, bool>> predicate);
    }
}