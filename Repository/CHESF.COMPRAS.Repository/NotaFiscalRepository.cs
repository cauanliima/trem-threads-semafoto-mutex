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
    public class NotaFiscalRepository : RepositoryBase<NotaFiscal>, INotaFiscalRepository
    {
        public NotaFiscalRepository(ISGNFUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
       
        public async Task<IList<NotaFiscal>> ListarParaContrato(int idContrato, int pagina, int total)
        {
            var situacoesPagamento = new List<int> {31, 35};
            return await _entities
                .AsNoTracking()
                .Include(notaFiscal => notaFiscal.StatusNotaFiscal)
                .ThenInclude(statusNotaFiscal => statusNotaFiscal.Status)
                .Where(notaFiscal => notaFiscal.IdContrato == idContrato)
                .OrderByDescending(notaFiscal => notaFiscal.DataEmissao)
                .Skip(pagina > 0 ? (pagina - 1) * total : 0)
                .Take(total)
                .Select(notaFiscal => new NotaFiscal
                {
                    Codigo = notaFiscal.Codigo,
                    Numero = notaFiscal.Numero,
                    IdFornecedor = notaFiscal.IdFornecedor,
                    IdContrato = notaFiscal.IdContrato,
                    Valor = notaFiscal.Valor,
                    Mes = notaFiscal.Mes,
                    Ano = notaFiscal.Ano,
                    IdAdministrador = notaFiscal.IdAdministrador,
                    DataEmissao = notaFiscal.DataEmissao,
                    DataInclusao = notaFiscal.DataInclusao,
                    IdStatus = notaFiscal.IdStatus,
                    Contrato = notaFiscal.Contrato,
                    StatusNotaFiscal = notaFiscal.StatusNotaFiscal,
                    HistoricoNotaFiscal = notaFiscal.HistoricoNotaFiscal,
                    DataPagamento = notaFiscal.HistoricoNotaFiscal.Where(
                            historico => situacoesPagamento.Contains(historico.IdStatus)).
                        OrderBy(historico => historico.DataInclusao).
                        Select( historico => historico.DataInclusao).FirstOrDefault()
                })
                .ToListAsync();
        }
        
        public async Task<IList<NotaFiscal>> ListarNotaFiscalPagasNaoNotificadas()
        {
            var situacoesPagamento = new List<int> {31, 35};
            return await _entities
                .AsNoTracking()
                .Include(notafiscal => notafiscal.Fornecedor)
                .Include(notaFiscal => notaFiscal.StatusNotaFiscal)
                .ThenInclude(statusNotaFiscal => statusNotaFiscal.Status)
                .Where(notaFiscal => !notaFiscal.PagamentoNotificado 
                                     && notaFiscal.HistoricoNotaFiscal.Any( historico => situacoesPagamento.Contains(historico.IdStatus)))
                .OrderByDescending(notaFiscal => notaFiscal.DataEmissao)
                .Select(notaFiscal => new NotaFiscal
                {
                    Codigo = notaFiscal.Codigo,
                    Numero = notaFiscal.Numero,
                    IdFornecedor = notaFiscal.IdFornecedor,
                    IdContrato = notaFiscal.IdContrato,
                    Valor = notaFiscal.Valor,
                    Mes = notaFiscal.Mes,
                    Ano = notaFiscal.Ano,
                    IdAdministrador = notaFiscal.IdAdministrador,
                    DataEmissao = notaFiscal.DataEmissao,
                    DataInclusao = notaFiscal.DataInclusao,
                    IdStatus = notaFiscal.IdStatus,
                    Contrato = notaFiscal.Contrato,
                    StatusNotaFiscal = notaFiscal.StatusNotaFiscal,
                    Fornecedor = notaFiscal.Fornecedor,
                    PagamentoNotificado = notaFiscal.PagamentoNotificado,
                    HistoricoNotaFiscal = notaFiscal.HistoricoNotaFiscal,
                    DataPagamento = notaFiscal.HistoricoNotaFiscal.Where(
                            historico => situacoesPagamento.Contains(historico.IdStatus)).
                        OrderBy(historico => historico.DataInclusao).
                        Select( historico => historico.DataInclusao).FirstOrDefault()
                })
                .ToListAsync();
        }
        
        
    }
}