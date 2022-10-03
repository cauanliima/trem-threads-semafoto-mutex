using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CHESF.COMPRAS.IRepository.Base;
using CHESF.COMPRAS.IRepository.UnitOfWork;

namespace CHESF.COMPRAS.Repository.Base
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected DbSet<T> _entities;
        
        public RepositoryBase(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            _entities = UnitOfWork.Context.Set<T>();
        }
        public async Task<IQueryable<T>> GetAll()
        {
            try
            {
                return _entities.AsQueryable();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IList<T>> GetAll(int pagina, int total)
        {
            return await _entities.AsNoTracking().Skip(pagina > 0 ? (pagina - 1) * total : 0).Take(total).ToListAsync();
        }

        public async Task<int> Count()
        {
            return await _entities.AsNoTracking().CountAsync();
        }

        public async Task<int> Count(Expression<Func<T, bool>> expression)
        {
            return await _entities.AsNoTracking().CountAsync(expression);
        }


        public async Task<IQueryable<T>> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return _entities.Where(expression);
        }
        
        public IQueryable<T> GetByConditionWithoutTracking(Expression<Func<T, bool>> expression)
        {
            return _entities.AsNoTracking().Where(expression);
        }

        public async Task<bool> Existe(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
        {
            return await _entities.AsNoTracking().AnyAsync(expression, cancellationToken);
        }
        
        public async Task<bool> Existe(Expression<Func<T, bool>> expression)
        {
            return await Existe(expression, default);
        }

        public async Task<IList<T>> GetByCondition(Expression<Func<T, bool>> expression, int offset, int total)
        {
            return await _entities.AsNoTracking()
                            .Where(expression)
                            .Skip(offset).Take(total)
                            .ToListAsync();
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return await _entities.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<T> Insert(T entity)
        {
            await _entities.AddAsync(entity);
            return entity;
        }
        
        public async Task<T> Insert(T entity, bool commit = true, bool detach = false)
        {
            
            _entities.Add(entity);
           
            try
            {
                await UnitOfWork.Context.SaveChangesAsync();
            }
            catch
            {
                if (!detach)
                {
                    throw;
                }
               
                _entities = UnitOfWork.Context.Set<T>();
                UnitOfWork.Context.Entry(entity).State = EntityState.Detached;

                throw;
            }

            _entities = UnitOfWork.Context.Set<T>();
            UnitOfWork.Context.Entry(entity).State = EntityState.Detached;
            
            return entity;
        }
        

        public void Detach(T entity)
        {
            UnitOfWork.Context.Entry(entity).State = EntityState.Detached;
        }

        public void DetachAll()
        {
            UnitOfWork.DetachAll();
            ClearEntities();
        }

        public void ClearEntities()
        {
            _entities = UnitOfWork.Context.Set<T>();
        }

        public async Task<T> Update(T entity)
        {
            UnitOfWork.Context.Entry(entity).State = EntityState.Modified;
            _entities.Update(entity);
        
            return entity;
        }
        
        public async Task<T> Update(T entityToUpdate, bool commit = true, bool detach = false)
        {
            _entities.Attach(entityToUpdate);
            UnitOfWork.Context.Entry(entityToUpdate).State = EntityState.Modified;

            // if (!commit) return;

            try
            {
                await UnitOfWork.Context.SaveChangesAsync();
            }
            catch
            {
                if (!detach)
                {
                    throw;
                }
               
                _entities = UnitOfWork.Context.Set<T>();
                UnitOfWork.Context.Entry(entityToUpdate).State = EntityState.Detached;

                throw;
            }

            // if (!detach) return;
           
            _entities = UnitOfWork.Context.Set<T>();
            UnitOfWork.Context.Entry(entityToUpdate).State = EntityState.Detached;

            return entityToUpdate;
        }

        public async Task<bool> Delete(T entity)
        {
            _entities.Remove(entity);
            return true;
        }

        public async Task SaveChanges()
        {
            await UnitOfWork.Context.SaveChangesAsync();
        }
    }
}
