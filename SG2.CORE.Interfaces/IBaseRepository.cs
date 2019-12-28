using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace SG2.CORE.Interfaces
{
    public interface IBaseRepository<T> 
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<T> GetById(int id);

        // TODO: Check if this overloading can be avoided.
        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> List();

        // TODO: Check if this overloading can be avoided.
        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> List(params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> List(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Lists the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> List(Expression<Func<T, bool>> predicate, Expression<Func<T, string>> orderBy, int pageSize = 15, int pageNumber = 1, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Counts the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        Task<int> Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<int> Add(T entity);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<int> Delete(T entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<int> Update(T entity);

        /// <value>
        /// The logged in user id.
        /// </value>
        int LoggedInUserId { get; }

        /// <value>
        /// The user timezone off set.
        /// </value>
        TimeSpan UserTimezoneOffSet { get; }
    }
}

