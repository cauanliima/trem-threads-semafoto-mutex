using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.E_Edital;
using CHESF.COMPRAS.IRepository.Base;

namespace CHESF.COMPRAS.IRepository
{
    public interface ILicitacaoRepository : IRepositoryBase<Licitacao>
    {
        Task<IList<Licitacao>> GetLicitacoesOrdenadas(Expression<Func<Licitacao, bool>> expression, int offset,
            int total);
    }
}