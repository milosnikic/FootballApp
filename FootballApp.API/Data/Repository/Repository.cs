using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;
        private readonly DbSet<T> _entities;

        public Repository(DbContext context)
        {
            Context = context;
            _entities = context.Set<T>();

        }

        public void Add(T entity)
        {
            _entities.Add(entity);
        }

        public void AddRange(ICollection<T> entities)
        {
            _entities.AddRange(entities);
        }

        public async Task<ICollection<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Where(predicate)
                                  .ToListAsync();
        }

        public async Task<ICollection<T>> GetAll()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }

        public void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        public void RemoveRange(ICollection<T> entities)
        {
            _entities.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }
    }
}