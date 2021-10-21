using CHESF.COMPRAS.Domain.E_Edital;
using Microsoft.EntityFrameworkCore;

namespace CHESF.BSV.Repository.Context
{
    public class ComprasContext : DbContext
    {
        public ComprasContext()
        {
        }

        public ComprasContext(DbContextOptions<ComprasContext> options) : base(options)
        {
        }

        public virtual DbSet<Licitacao> Licitacaos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Licitacao>();
      
            base.OnModelCreating(modelBuilder);
        }
    }
}