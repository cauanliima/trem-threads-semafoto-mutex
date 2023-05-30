using CHESF.COMPRAS.Domain.SGNF;
using CHESF.COMPRAS.IRepository;
using CHESF.COMPRAS.IRepository.UnitOfWork;
using CHESF.COMPRAS.Repository.Base;

namespace CHESF.COMPRAS.Repository
{
    public class FornecedorRepository: RepositoryBase<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(ISGNFUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}