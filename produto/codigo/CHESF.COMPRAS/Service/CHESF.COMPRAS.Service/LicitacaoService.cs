using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.E_Edital;
using CHESF.COMPRAS.Domain.QueryParams;
using CHESF.COMPRAS.IRepository;
using CHESF.COMPRAS.IService;
using Microsoft.EntityFrameworkCore;

namespace CHESF.COMPRAS.Service
{
    public class LicitacaoService : ILicitacaoService
    {
        private readonly ILicitacaoRepository _repository;

        public LicitacaoService(ILicitacaoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Licitacao> Buscar(long id)
        {
            return await _repository.FirstOrDefault(l => l.Codigo.Equals(id));
        }

        public async Task<IEnumerable<Licitacao>> Listar(ListaQueryParams queryParams)
        {
            return await _repository.GetLicitacoesOrdenadas(l => l.Status.Equals("PU"), queryParams.pagina * queryParams.total,
                queryParams.total);
        }

        public async Task<IEnumerable<Licitacao>> Listar(LicitacaoFiltroQueryParams filtro)
        {
            var licitacoes = await _repository.GetAll();

            if (filtro.situacao != null)
            {
                licitacoes =
                    licitacoes.Where(l => l.Status.ToUpper().Equals(filtro.situacao.ToUpper()));
            }

            if (filtro.modalidade != null)
            {
                licitacoes =
                    licitacoes.Where(l => l.Modalidade.ToUpper().Equals(filtro.modalidade.ToUpper()));
            }

            if (filtro.inicio > DateTime.MinValue && filtro.fim > DateTime.MinValue)
            {
                licitacoes =
                    licitacoes.Where(l => filtro.inicio <= l.AberturaPropostas && l.AberturaPropostas <= filtro.fim);
            }


            if (filtro.texto != null)
            {
                licitacoes = licitacoes.Where(l =>
                    l.Codigo.ToString().Contains(filtro.texto) ||
                        l.Descricao.ToUpper().Contains(filtro.texto.ToUpper()));
            }

            return licitacoes.Skip(filtro.pagina * filtro.total).Take(filtro.total);
        }
    }
}