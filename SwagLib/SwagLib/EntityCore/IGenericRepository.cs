using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SwagLib.EntityCore
{
    public interface IGenericRepository<T> where T : class
    {
        #region Operations

        //Get
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");

        //Get By Id
        T GetById(object id);

        //insert
        void Insert(T entity);

        //delete by id
        void DeleteById(object id);

        //delete 
        void Delete(T entity);

        //update
        void Update(T entity);

        #endregion

        #region Task Operations

        //Get
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");

        //Get By Id
        Task<T> GetByIdAsync(object id);

        //insert
        Task InsertTask(T entity);

        //delete by id
        Task DeleteByIdAsync(object id);

        //delete 
        Task DeleteTask(T entity);

        //update
        Task UpdateTask(T entity);


        #endregion

    }
}
