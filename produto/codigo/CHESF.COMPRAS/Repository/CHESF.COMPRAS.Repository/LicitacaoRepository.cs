using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.E_Edital;
using CHESF.COMPRAS.IRepository;
using CHESF.COMPRAS.IRepository.UnitOfWork;
using CHESF.COMPRAS.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace CHESF.COMPRAS.Repository
{
    public class LicitacaoRepository : RepositoryBase<Licitacao>, ILicitacaoRepository
    {
        public LicitacaoRepository(IEComprasUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IList<Licitacao>> GetLicitacoesOrdenadas(Expression<Func<Licitacao, bool>> expression,
            int offset, int total)
        {
            return await _entities.AsNoTracking()
                .Where(expression)
                .OrderByDescending(t => t.AberturaPropostas)
                .Skip(offset)
                .Take(total)
                .ToListAsync();
        }

        public IQueryable<Licitacao> GetTodasOrdenadas()
        {
            return _entities.AsNoTracking()
                .OrderByDescending(t => t.AberturaPropostas);
        }
    }
}