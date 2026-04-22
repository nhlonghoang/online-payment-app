using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePaymentApp.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        // tracked = false to stop EF from tracking record
        IEnumerable<T> GetAll(string? includeProperties = null);  
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
