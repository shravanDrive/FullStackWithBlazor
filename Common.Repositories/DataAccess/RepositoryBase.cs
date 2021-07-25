using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Repositories.DataAccess
{
    /// <summary>
    /// Repository Base for Entity Framework
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class RepositoryBase<TEntity>: IRepositoryBase<TEntity> where TEntity : class
    {
        /// <summary>
        /// The Base Database Context
        /// </summary>
        private readonly BaseDbContext context;

        /// <summary>
        /// The Entity
        /// </summary>
        private readonly DbSet<TEntity> entity;

        /// <summary>
        /// Initializes a new instance of context
        /// </summary>
        /// <param name="context"></param>
        public RepositoryBase(BaseDbContext context)
        {
            this.context = context;                                                                                                                                                                            
            this.entity = this.context.Set<TEntity>();
        }

        /// <summary>
        /// Get data by Identifier Doesnt work
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public virtual async Task<TEntity> GetByIdAsync(int id)
        //{
        //    try
        //    {
        //        return await this.entity.FindAsync(id).ConfigureAwait(false);
        //    }
        //    catch(Exception ex)
        //    {
        //        return null;
        //    }
            
        //}

        public virtual async Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            IQueryable<TEntity> query = this.entity;
            if(predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.AsNoTracking<TEntity>().ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Adds the specified entity
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>Task</returns>
        public virtual async Task AddAsync(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await this.entity.AddAsync(entity).ConfigureAwait(false);
            await this.context.SaveChangesAsync().ConfigureAwait(false);

            // Change tracker is enabled by default. If you want to sae the same entity more than once,
            // it throws an error as it is tracking every record
            // to save the entity more than once, we detatch the record from the context
            this.context.Entry(entity).State = EntityState.Detached;
        }

        /// <summary>
        /// Count the number of records in the entity
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<int> CountByConditionAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate != null)
            {
                return await this.entity.CountAsync(predicate).ConfigureAwait(false);
            }

            return await this.entity.CountAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Gets all the entries in the entity
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await this.entity.AsNoTracking<TEntity>().ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Gets it by condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetByConditionAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IEnumerable<TEntity> query = await this.GetByConditionAsync(predicate).ConfigureAwait(false);
            if (orderBy != null)
            {
                return await orderBy(query.AsQueryable<TEntity>()).ToListAsync().ConfigureAwait(false);
            }

            return query.ToList();
        }

        /// <summary>
        /// Deletes the specified entity asynchronously
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(object id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            TEntity entityToDelete = await this.entity.FindAsync(id).ConfigureAwait(false);
            await this.DeleteAsync(entityToDelete).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the specified entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(TEntity entity)
        {
            this.context.Entry(entity).State = EntityState.Deleted;
            this.entity.Remove(entity);
            await this.context.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            this.context.Entry(entity).State = EntityState.Modified;
            await this.context.SaveChangesAsync().ConfigureAwait(false);

            // Change tracker is enabled by default. If you want to sae the same entity more than once,
            // it throws an error as it is tracking every record
            // to save the entity more than once, we detatch the record from the context
            this.context.Entry(entity).State = EntityState.Deleted;
        }

        /// <summary>
        /// Execute Stored Procedure
        /// </summary>
        /// <param name="spname"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<IList<TEntity>> ExecWithStoredProcedureAync(string spname, params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(spname))
            {
                throw new ArgumentException($"'{nameof(spname)}' cannot be null or empty.", nameof(spname));
            }

            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var query = this.BuildStoredProcedureQuery(spname, parameters);
            return await this.entity.FromSqlRaw<TEntity>(query, parameters).ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Build Stored Procedure query
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string BuildStoredProcedureQuery(string spName, SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentException($"'{nameof(spName)}' cannot be null or empty.", nameof(spName));
            }

            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            string query = string.Empty;
            if (parameters.Length == 0)
            {
                query = $"EXECUTE {spName}";
            }
            else
            {
                query = string.Format($"EXECUTE {spName} {string.Join(",", parameters.Select(x => x.ParameterName))}", parameters);
            }

            return query;
        }

        /// <summary>
        /// Execute Stored Procedure {Requires 1 output variable in the SP
        /// If multiple to be returned club all the output variables as a nvarchar json 
        /// and return it}
        /// </summary>
        /// <param name="stringquery"></param>
        /// <returns></returns>
        public async Task<int> ExecWithStoredProcedureAsync(System.FormattableString stringquery)
        {
            return await this.context.Database.ExecuteSqlInterpolatedAsync(stringquery).ConfigureAwait(false);
        }

        /// <summary>
        /// Exec Stored procedure which has select statements inside of it.
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> ExecSp(System.FormattableString stringquery)
        {
            return await this.entity.FromSqlInterpolated(stringquery).ToListAsync();
        }
    }
}