using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.Domain.SGNF;
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

        public GerarNotificacaoPagamentoService(
            ILogger<GerarNotificacaoPagamentoService> logger,
            INotaFiscalRepository notaFiscalRepository,
            INotificationService notificationService
        )
        {
            _logger = logger;
            _notaFiscalRepository = notaFiscalRepository;
            _notificationService = notificationService;
        }
       
        public async Task GerarPagamentos()
        {
            var ptBrCulture = new CultureInfo("pt-BR");
            _logger.LogInformation("Iniciando envio de notificações de pagamento das notas fiscais");
           
            var notasFiscais = await _notaFiscalRepository.ListarNotaFiscalPagasNaoNotificadas();
            foreach (var notaFiscal in notasFiscais)
            {
                _logger.LogInformation("Iniciando o envio da notificação da nota fiscal de número {numero}", notaFiscal.Numero);
                try
                {
                    var dto = new NotificarDTO();
                    var dataPagamento = String.Format("{0:dd/MM/yyyy}", notaFiscal.DataPagamento);
                    dto.Tipo = "PAGAMENTO_CONTRATO";
                    dto.Titulo = "E-Compras - CHESF - Pagamento Realizado";
                    dto.Texto =
                        $"O pagamento da nota fiscal de número {notaFiscal.Numero}, no valor de R$ {notaFiscal.Valor?.ToString("C", ptBrCulture)}, " +
                        $"foi realizado em {dataPagamento}.";
                    dto.Payload = new Dictionary<string, string>();
                    dto.Metadados = new Dictionary<string, string>();

                    if (notaFiscal.Contrato != null)
                    {
                        dto.Payload.Add("numeroContrato", notaFiscal.Contrato.Numero);
                    }

                    dto.Cnpj = notaFiscal.Fornecedor?.CNPJ.ToString();

                    _logger.LogInformation(JsonSerializer.Serialize(dto));
                    
                    var resultado = await _notificationService.NotificarAsync(dto);
                    
                    _logger.LogInformation("Resultado para o envio da notificação da nota fiscal de número {numero}", notaFiscal.Numero);
                    _logger.LogInformation(JsonSerializer.Serialize(resultado));

                    notaFiscal.IndicadorPagamentoNotaFiscal = (int)NotaFiscal.StatusNotificacao.PagaENotificada;

                    var entry = _notaFiscalRepository.GetEntry(notaFiscal);
                    entry.Property(nf => nf.IndicadorPagamentoNotaFiscal).IsModified = true;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Houve um problema ao notificar");
                }
            }

            await _notaFiscalRepository.SaveChanges();
            
            _logger.LogInformation("Finalizando o envio de notificações de pagamento das notas fiscais");
        }
    }
}