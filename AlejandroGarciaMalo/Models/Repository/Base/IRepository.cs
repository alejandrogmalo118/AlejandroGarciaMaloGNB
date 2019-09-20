using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlejandroGarciaMalo.Models.Repository.Base
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T obj);
        void Delete(T id);
        void DeleteRange(IEnumerable<T> entities);
    }
}
