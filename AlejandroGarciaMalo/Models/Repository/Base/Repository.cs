using AutoMapper;
using AlejandroGarciaMalo.Models.DBContext;
using AlejandroGarciaMalo.Models.UnitOfWork;
using AlejandroGarciaMalo.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlejandroGarciaMalo.Models.Repository.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DbSet<T> _entities;

        private bool _isDisposed;

        protected readonly MyDbContext Context;
        
        public Repository(MyDbContext DbContext)
        {
            _isDisposed = false;
            Context = DbContext;
        }

        protected virtual DbSet<T> Entities
        {
            get { return _entities ?? (_entities = Context.Set<T>()); }
        }
        
        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
            _isDisposed = true;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await Entities.ToListAsync();
        }

        public void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Entities.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            _entities.AddRange(entities);
        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            SetEntryModified(entity);
        }

        public void Delete(T id)
        {
            var entity = Entities.Find(id);
            if (entity == null)
                throw new ArgumentNullException("entity");

            _entities.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _entities.RemoveRange(entities);
        }

        public virtual void SetEntryModified(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }
    }
}
