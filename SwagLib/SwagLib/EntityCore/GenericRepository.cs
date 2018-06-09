using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace SwagLib.EntityCore
{
    public class GenericRepository<TE> : IGenericRepository<TE> where TE : class
    {
        #region Setup

        internal DbContext Context;
        internal DbSet<TE> DbSet;

        public GenericRepository(DbContext context)
        {
            this.Context = context;
            this.DbSet = context.Set<TE>();
        }
        #endregion

        #region Operations
        public virtual IEnumerable<TE> Get(Expression<Func<TE, bool>> filter = null,
            Func<IQueryable<TE>, IOrderedQueryable<TE>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TE> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TE GetById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual void Insert(TE entity)
        {
            DbSet.Add(entity);
        }

        public virtual void DeleteById(object id)
        {
            TE entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TE entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
        }

        public virtual void Update(TE entity)
        {
            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        #endregion

        #region Task Operations

        public async Task<IEnumerable<TE>> GetAsync(Expression<Func<TE, bool>> filter = null, Func<IQueryable<TE>, IOrderedQueryable<TE>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TE> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<TE> GetByIdAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public Task InsertTask(TE entity)
        {
           return new Task ( () => DbSet.Add(entity));
        }

        public async Task DeleteByIdAsync(object id)
        {
            TE entityToDelete = await DbSet.FindAsync(id);
            await DeleteTask(entityToDelete);
        }

        public Task DeleteTask(TE entity)
        {
            
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            return new Task( () => DbSet.Remove(entity));
        }

        public Task UpdateTask(TE entity) 
        {
            Task task = new Task ( () => DbSet.Attach(entity));
            Context.Entry(entity).State = EntityState.Modified;
            return task;
        }

        #endregion
    }
}
