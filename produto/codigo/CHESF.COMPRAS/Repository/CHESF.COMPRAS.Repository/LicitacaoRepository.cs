using CHESF.COMPRAS.Domain.E_Edital;
using CHESF.COMPRAS.IRepository;
using CHESF.COMPRAS.IRepository.UnitOfWork;
using CHESF.COMPRAS.Repository.Base;

namespace CHESF.COMPRAS.Repository
{
    public class LicitacaoRepository : RepositoryBase<Licitacao>, ILicitacaoRepository
    {
        public LicitacaoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}