using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.SGNF;
using CHESF.COMPRAS.IRepository;
using CHESF.COMPRAS.IRepository.UnitOfWork;
using CHESF.COMPRAS.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace CHESF.COMPRAS.Repository
{
    public class NotaFiscalRepository : RepositoryBase<NotaFiscal>, INotaFiscalRepository
    {
        public NotaFiscalRepository(ISGNFUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
       
        public async Task<IList<NotaFiscal>> ListarParaContrato(int idContrato, int pagina, int total)
        {
            return await _entities
                .AsNoTracking()
                .Include(notaFiscal => notaFiscal.StatusNotaFiscal)
                .ThenInclude(statusNotaFiscal => statusNotaFiscal.Status)
                .Where(notaFiscal => notaFiscal.IdContrato == idContrato)
                .OrderByDescending(notaFiscal => notaFiscal.DataEmissao)
                .Skip(pagina > 0 ? (pagina - 1) * total : 0)
                .Take(total)
                .ToListAsync();
        }
    }
}