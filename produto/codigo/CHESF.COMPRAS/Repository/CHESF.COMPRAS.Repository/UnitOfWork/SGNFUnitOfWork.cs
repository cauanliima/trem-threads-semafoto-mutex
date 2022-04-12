using CHESF.COMPRAS.IRepository.UnitOfWork;
using CHESF.COMPRAS.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace CHESF.COMPRAS.Repository.UnitOfWork
{
    public class SGNFUnitOfWork : ISGNFUnitOfWork
    {
        public DbContext Context { get; }

        public SGNFUnitOfWork(SGNFContext context)
        {
            Context = context;
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void DetachAll()
        {
            foreach (var entity in Context.ChangeTracker.Entries())
            {
                entity.State = EntityState.Detached;
            }
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}