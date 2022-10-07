using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CHESF.COMPRAS.API.Config.Security;
using CHESF.COMPRAS.Domain.APP;
using CHESF.COMPRAS.Domain.DTOs;
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

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
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