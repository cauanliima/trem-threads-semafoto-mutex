using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.APP;
using CHESF.COMPRAS.Domain.DTOs;

namespace CHESF.COMPRAS.IService
{
    public interface INotificationService
    {
        Task<bool> AtualizarRegistroDispositivoAsync(DispositivoDTO dispositivo, CancellationToken token);
        Task<bool> NotificarAsync(NotificarDTO dto);
        Task<bool> RequestNotificationAsync(NotificationRequest notificationRequest, CancellationToken token);
    }
}