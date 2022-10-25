using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.APP;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.Domain.E_Edital;
using CHESF.COMPRAS.Domain.QueryParams;

namespace CHESF.COMPRAS.IService
{
    public interface INotificationService
    {
        Task<bool> AtualizarRegistroDispositivoAsync(DispositivoDTO dispositivo, CancellationToken token);
        Task<NotificarResultadoDTO> NotificarAsync(NotificarDTO dto);
        Task<bool> RequestNotificationAsync(NotificationRequest notificationRequest, CancellationToken token);

        Task<List<Notificacao>> ListarAsync(
            Dispositivo dispositivo,
            NotificacoesQueryParams queryParams,
            CancellationToken cancellationToken = default
        );
    }
}