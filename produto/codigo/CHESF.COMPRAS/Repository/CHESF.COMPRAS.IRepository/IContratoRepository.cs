using System.Collections.Generic;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.SGNF;
using CHESF.COMPRAS.IRepository.Base;

namespace CHESF.COMPRAS.IRepository
{
    public interface IContratoRepository : IRepositoryBase<Contrato>
    {
        public Task<IList<Contrato>> ListarParaCNPJ(long cnpj, int pagina, int total);
    }
}