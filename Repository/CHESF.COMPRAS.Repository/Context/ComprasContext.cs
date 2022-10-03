using CHESF.COMPRAS.Domain.E_Edital;
using Microsoft.EntityFrameworkCore;

namespace CHESF.COMPRAS.Repository.Context
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
        public virtual DbSet<Anexo> Anexos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dispositivo>();
            modelBuilder.Entity<DispositivoMetadado>();
            modelBuilder.Entity<Anexo>();

            base.OnModelCreating(modelBuilder);
        }
    }
}