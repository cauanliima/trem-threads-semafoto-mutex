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
            return await _repository.GetLicitacoesOrdenadas(l => l.Status.Equals("PU"),
                queryParams.pagina * queryParams.total,
                queryParams.total);
        }

        public async Task<IEnumerable<Licitacao>> Listar(LicitacoesFavoritadasQueryParams filtroQuery)
        {
            return await _repository.GetByCondition(l => filtroQuery.ids.Contains(l.Codigo));
        }


        public async Task<IEnumerable<Licitacao>> Listar(LicitacaoFiltroQueryParams filtro)
        {
            var licitacoes = await _repository.GetAll();

            if (filtro.situacoes != null)
            {
                licitacoes = licitacoes.Where(l =>
                    filtro.situacoes.Select(s => s.ToUpper()).Contains(l.Status.ToUpper()));
            }

            if (filtro.modalidades != null)
            {
                licitacoes =
                    licitacoes.Where(l => filtro.modalidades.Select(s => s.ToUpper()).Contains(l.Modalidade.ToUpper()));
            }

            if (filtro.inicio > DateTime.MinValue && filtro.fim > DateTime.MinValue)
            {
                licitacoes =
                    licitacoes.Where(l => filtro.inicio <= l.AberturaPropostas && l.AberturaPropostas <= filtro.fim);
            }


            if (filtro.texto != null)
            {
                licitacoes = licitacoes.ToList().Where(l =>
                    l.Numero.ToString().Contains(filtro.texto) ||
                    l.Descricao.ToUpper().Contains(filtro.texto.ToUpper())).AsQueryable();
            }

            return licitacoes.Skip(filtro.pagina * filtro.total).Take(filtro.total);
        }
    }
}