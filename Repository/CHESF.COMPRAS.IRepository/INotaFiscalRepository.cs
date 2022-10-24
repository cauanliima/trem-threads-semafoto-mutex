using System.Collections.Generic;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.SGNF;
using CHESF.COMPRAS.IRepository.Base;

namespace CHESF.COMPRAS.IRepository
{
    public interface INotaFiscalRepository : IRepositoryBase<NotaFiscal>
    {
        public Task<IList<NotaFiscal>> ListarParaContrato(int idContrato, int pagina, int total);

        public Task<IList<NotaFiscal>> ListarNotaFiscalPagasNaoNotificadas();

    }
}