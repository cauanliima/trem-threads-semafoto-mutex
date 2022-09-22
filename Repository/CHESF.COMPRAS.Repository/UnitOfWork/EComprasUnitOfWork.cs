using CHESF.COMPRAS.Repository.Context;
using CHESF.COMPRAS.IRepository.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CHESF.COMPRAS.Repository.UnitOfWork
{
    public class EcomprasUnitOfWork : IEComprasUnitOfWork
    {
        public DbContext Context { get; }

        public EcomprasUnitOfWork(ComprasContext context)
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