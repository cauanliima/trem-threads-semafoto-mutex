using System.Collections.Generic;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.E_Edital;
using CHESF.COMPRAS.Domain.QueryParams;


namespace CHESF.COMPRAS.IService
{
    public interface ILicitacaoService
    {
        Task<Licitacao> Buscar(long id);

        Task<IEnumerable<Licitacao>> Listar(ListaQueryParams queryParams);
        Task<IEnumerable<Licitacao>> Listar(LicitacaoFiltroQueryParams filtroQuery);
        Task<IEnumerable<Licitacao>> Listar(LicitacoesFavoritadasQueryParams filtroQuery);
    }
}