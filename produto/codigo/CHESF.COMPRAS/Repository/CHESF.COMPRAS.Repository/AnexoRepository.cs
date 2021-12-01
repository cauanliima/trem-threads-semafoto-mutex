using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.Domain.E_Edital;
using CHESF.COMPRAS.IRepository;
using CHESF.COMPRAS.IRepository.UnitOfWork;
using CHESF.COMPRAS.Repository.Base;

namespace CHESF.COMPRAS.Repository
{
    public class AnexoRepository : RepositoryBase<Anexo>, IAnexoRepository
    {
        public AnexoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IQueryable<Anexo> TodasDaLicitacao(long licitacaoId)
        {
            return _entities.Where(x => x.CodigoLicitacao == licitacaoId).Select(x => new Anexo()
            {
                Codigo = x.Codigo,
                CodigoLicitacao = x.CodigoLicitacao,
                Nome = x.Nome,
                Descricao = x.Descricao,
                DataCriacao = x.DataCriacao,
            });
        }
    }
}