using CHESF.COMPRAS.Domain.E_Edital;
using CHESF.COMPRAS.IRepository;
using CHESF.COMPRAS.IRepository.UnitOfWork;
using CHESF.COMPRAS.Repository.Base;

namespace CHESF.COMPRAS.Repository
{
    public class NotificacaoRepository : RepositoryBase<Notificacao>, INotificacaoRepository
    {
        public NotificacaoRepository(IEComprasUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}