using CHESF.COMPRAS.Domain.E_Edital;
using CHESF.COMPRAS.IRepository;
using CHESF.COMPRAS.IRepository.UnitOfWork;
using CHESF.COMPRAS.Repository.Base;

namespace CHESF.COMPRAS.Repository
{
    public class DispositivoMetadadoRepository : RepositoryBase<DispositivoMetadado>, IDispositivoMetadadoRepository
    {
        public DispositivoMetadadoRepository(IEComprasUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}