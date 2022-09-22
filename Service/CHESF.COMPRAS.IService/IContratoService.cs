using System.Collections.Generic;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.QueryParams;
using CHESF.COMPRAS.Domain.SGNF;

namespace CHESF.COMPRAS.IService
{
    public interface IContratoService
    {
        Task<IEnumerable<Contrato>> Listar(ListaQueryParams queryParams);
        Task<IEnumerable<NotaFiscal>> ListarNotasFiscais(int idContrato, ListaQueryParams queryParams);
    }
}