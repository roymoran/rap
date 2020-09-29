using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace RAP.Contracts
{
    public interface IStorage<T>
    {
        Task<T> CreateAsync(T item);
        Task<T> FindAsync(Guid id);
        Task<List<T>> FindAllAsync();
        Task<T> FindByAsync(Expression<Func<T, bool>> predicate);
        Task<T> UpdateAsync(T item);
    }
}
