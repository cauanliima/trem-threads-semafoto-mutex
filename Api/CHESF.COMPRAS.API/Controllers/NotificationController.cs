using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CHESF.COMPRAS.API.Config.Security;
using CHESF.COMPRAS.Domain.APP;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.Domain.E_Edital;
using CHESF.COMPRAS.Domain.QueryParams;
using CHESF.COMPRAS.IService;
using Microsoft.AspNetCore.Mvc;

namespace CHESF.COMPRAS.API.Controllers
{
    [ApiController]
    [ApiKey]
    [Route("notificacoes")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IGerarNotificacaoPagamentoService _gerarNotificacaoPagamentoService;

        public NotificationsController(
            INotificationService notificationService,
            IGerarNotificacaoPagamentoService gerarNotificacaoPagamentoService
        )
        {
            _notificationService = notificationService;
            _gerarNotificacaoPagamentoService = gerarNotificacaoPagamentoService;
        }

        [ServiceFilter(typeof(DispositivoUidAttribute))]
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Notificacao>>> Listar([FromQuery] NotificacoesQueryParams queryParams)
        {
            var dispositivo = (Dispositivo) HttpContext.Items["Dispositivo"];

            var notificacoes = await _notificationService.ListarAsync(dispositivo, queryParams);
            
            return Ok(notificacoes);
        }
        
        [HttpPost]
        [Route("gerar-notificacoes-pagamento")]
        public async Task<IActionResult> GerarNotificacoesPagamento()
        {
            await _gerarNotificacaoPagamentoService.GerarPagamentos();
            return Ok();
        }

        [HttpPut]
        [Route("dispositivo")]
        public async Task<IActionResult> AtualizarRegistroDispositivo([FromBody] DispositivoDTO dispositivo)
        {
            var success = await _notificationService.AtualizarRegistroDispositivoAsync(dispositivo, default);

            if (!success)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [Route("notificar")]
        public async Task<ActionResult<NotificarResultadoDTO>> Notificar([FromBody] NotificarDTO dto)
        {
            var resultado = await _notificationService.NotificarAsync(dto);
            return Ok(resultado);
        }

        [HttpPost]
        [Route("requests")]
        public async Task<IActionResult> RequestPush(
            [Required] NotificationRequest notificationRequest)
        {
            if ((notificationRequest.Silent &&
                 string.IsNullOrWhiteSpace(notificationRequest?.Action)) ||
                (!notificationRequest.Silent &&
                 string.IsNullOrWhiteSpace(notificationRequest?.Texto)) ||
                string.IsNullOrWhiteSpace(notificationRequest.CodigoLicitacao) ||
                string.IsNullOrWhiteSpace(notificationRequest.NumeroLicitacao))
                return new BadRequestResult();

            var success = await _notificationService
                .RequestNotificationAsync(notificationRequest, HttpContext.RequestAborted);

            if (!success)
                return new UnprocessableEntityResult();

            return new OkResult();
        }
    }
}