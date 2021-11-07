using System.Linq;
using CHESF.COMPRAS.Domain.E_Edital;
using CHESF.COMPRAS.IRepository.Base;

namespace CHESF.COMPRAS.IRepository
{
    public interface IAnexoRepository: IRepositoryBase<Anexo>
    {
        IQueryable<Anexo> TodasDaLicitacao(long licitacaoId);
    }
}