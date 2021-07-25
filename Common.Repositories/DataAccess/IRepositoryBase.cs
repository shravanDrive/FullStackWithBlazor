using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Repositories.DataAccess
{

    /// <summary>
    /// Defines the IRepository Base
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepositoryBase<TEntity> where TEntity:class
    {
        /// <summary>
        /// Get the Id identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //Task<TEntity> GetByIdAsync(int id);

        /// <summary>
        /// GetByConditionAsync
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// ExecSp
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> ExecSp(System.FormattableString stringquery);
    }
}
