using CHESF.COMPRAS.Domain.SGNF;
using Microsoft.EntityFrameworkCore;

namespace CHESF.COMPRAS.Repository.Context
{
    public class SGNFContext : DbContext
    {
        public SGNFContext()
        {
        }
        
        public SGNFContext(DbContextOptions<SGNFContext> options) : base(options)
        {
        }
        
        public virtual DbSet<Contrato> Contrato { get; set; }
        public virtual DbSet<Fornecedor> Fornecedor { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<ContratoFornecedor> ContratoFornecedor { get; set; }
        public virtual DbSet<NotaFiscal> NotaFiscal { get; set; }
        public virtual DbSet<StatusNotaFiscal> StatusNotaFiscal { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contrato>();
            modelBuilder.Entity<Fornecedor>();
            modelBuilder.Entity<Status>();
            modelBuilder.Entity<ContratoFornecedor>();
            modelBuilder.Entity<NotaFiscal>()
                .HasMany(nota => nota.HistoricoNotaFiscal)
                .WithOne(status => status.NotaFiscal)
                .HasForeignKey(status => status.IdNotaFiscal);
            modelBuilder.Entity<NotaFiscal>()
                .HasOne<StatusNotaFiscal>(nota => nota.StatusNotaFiscal)
                .WithMany(status => status.NotasFiscais)
                .HasForeignKey(nota => nota.IdStatus);
            modelBuilder.Entity<StatusNotaFiscal>();
            modelBuilder.Entity<Usuario>();

            base.OnModelCreating(modelBuilder);
        }

    }
}
