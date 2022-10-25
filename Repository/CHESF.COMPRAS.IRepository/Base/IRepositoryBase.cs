using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CHESF.COMPRAS.IRepository.Base
{
    public interface IRepositoryBase<T> where T: class
    {
        Task<IQueryable<T>> GetAll();
        Task<IList<T>> GetAll(int pagina, int total);
        Task<int> Count();
        Task<int> Count(Expression<Func<T, bool>> expression);
        Task<IQueryable<T>> GetByCondition(Expression<Func<T, bool>> expression);
        IQueryable<T> GetByConditionWithoutTracking(Expression<Func<T, bool>> expression);
        Task<bool> Existe(Expression<Func<T, bool>> expression);
        Task<bool> Existe(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
        Task<IList<T>> GetByCondition(Expression<Func<T, bool>> expression, int pagina, int total);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> expression);
        Task<T> Insert(T entity);
        Task<T> Insert(T entity, bool commit, bool detach);
        Task<T> Update(T entity);
        Task<T> Update(T entity, bool commit, bool detach);
        EntityEntry<T> GetEntry(T entity);
        void Detach(T entity);
        void DetachAll();
        void ClearEntities();
        Task SaveChanges();
        Task<bool> Delete(T entity);
    }
}