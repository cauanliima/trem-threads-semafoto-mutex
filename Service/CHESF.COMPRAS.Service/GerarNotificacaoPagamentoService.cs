using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.IRepository;
using CHESF.COMPRAS.IService;
using Microsoft.Extensions.Logging;

namespace CHESF.COMPRAS.Service
{
    public class GerarNotificacaoPagamentoService : IGerarNotificacaoPagamentoService
    {
        private readonly ILogger<GerarNotificacaoPagamentoService> _logger;

        private readonly INotaFiscalRepository _notaFiscalRepository;
        
        private readonly INotificationService _notificationService;
        public async Task GerarPagamentos()
        {
            var notasFiscais = await _notaFiscalRepository.ListarNotaFiscalPagasNaoNotificadas();
            foreach (var notaFiscal in notasFiscais)
            {
                NotificarDTO dto = new NotificarDTO();
                var dataPagamento = String.Format("{0:dd/MM/yyyy}", notaFiscal.DataPagamento);
                dto.Tipo = "PAGAMENTO_CONTRATO";
                dto.Titulo = "E-Compras - CHESF - Pagamento Realizado";
                dto.Texto = $"O pagamento da nota fiscal de número {notaFiscal.Numero}, no valor de R$ {notaFiscal.Valor?.ToString("C")}, " +
                            $"foi realizado em {dataPagamento}.";
                dto.Payload = new Dictionary<string, string>();
                dto.Payload.Add("numeroContrato", notaFiscal.Contrato.Numero);
                dto.Cnpj = notaFiscal.Fornecedor.CNPJ.ToString();
                //var resultado = await _notificationService.NotificarAsync(dto);
            }
        }
    }
}