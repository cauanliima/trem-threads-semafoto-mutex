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
    public class ContratoRepository : RepositoryBase<Contrato>, IContratoRepository
    {
        public ContratoRepository(ISGNFUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IList<Contrato>> ListarParaCNPJ(long cnpj, int pagina, int total)
        {
            return await _entities
                .AsNoTracking()
                .Where(contrato =>
                    contrato.ContratoFornecedores.Any(contratoFornecedor => contratoFornecedor.Fornecedor.CNPJ == cnpj))
                .OrderBy(contrato => contrato.Codigo)
                .Skip(pagina > 0 ? (pagina - 1) * total : 0)
                .Take(total)
                .ToListAsync();
        }
    }
}