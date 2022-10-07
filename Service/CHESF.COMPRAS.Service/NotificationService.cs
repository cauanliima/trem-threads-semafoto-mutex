using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.APP;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.Domain.E_Edital;
using CHESF.COMPRAS.IRepository;
using CHESF.COMPRAS.IService;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CHESF.COMPRAS.Service
{
    public class NotificationService : INotificationService
    {
        readonly ILogger<NotificationService> _logger;
        private readonly IDispositivoRepository _dispositivoRepository;
        private readonly IDispositivoMetadadoRepository _dispositivoMetadadoRepository;

        public NotificationService(
            ILogger<NotificationService> logger,
            IDispositivoRepository dispositivoRepository,
            IDispositivoMetadadoRepository dispositivoMetadadoRepository
        )
        {
            _logger = logger;
            _dispositivoRepository = dispositivoRepository;
            _dispositivoMetadadoRepository = dispositivoMetadadoRepository;
        }

        public async Task<bool> AtualizarRegistroDispositivoAsync(DispositivoDTO dto, CancellationToken token)
        {
            var query = from dispositivoBanco in (await _dispositivoRepository.GetAll())
                where dispositivoBanco.UidFirebaseInstallation == dto.FirebaseId
                select dispositivoBanco;

            var dispositivo = query.FirstOrDefault();

            if (dispositivo == null)
            {
                dispositivo = await _dispositivoRepository.Insert(new Dispositivo
                {
                    UidFirebaseInstallation = dto.FirebaseId,
                    CnpjVinculado = dto.Cnpj,
                    DataAtualizacaoFcmToken = DateTime.UtcNow,
                    FcmToken = dto.FcmToken
                });
            }
            else
            {
                dispositivo.FcmToken = dto.FcmToken;
                dispositivo.CnpjVinculado = dto.Cnpj;
                dispositivo.DataAtualizacaoFcmToken = DateTime.UtcNow;

                await _dispositivoRepository.Update(dispositivo);
            }

            await _dispositivoRepository.SaveChanges();

            await AtualizarMetadadosAsync(dispositivo, dto.Metadados, token);

            _dispositivoRepository.DetachAll();

            return true;
        }

        private async Task<bool> AtualizarMetadadosAsync(Dispositivo dispositivo, Dictionary<string, string> metadados,
            CancellationToken token)
        {
            var metadadosEmBanco = await (from metadado in await _dispositivoMetadadoRepository.GetAll()
                where metadado.IdDispositivo == dispositivo.Id
                select metadado).ToListAsync(cancellationToken: token);

            foreach (var metadado in metadadosEmBanco)
            {
                await _dispositivoMetadadoRepository.Delete(metadado);
            }

            await _dispositivoMetadadoRepository.SaveChanges();
            _dispositivoMetadadoRepository.DetachAll();

            foreach (var (identificador, valor) in metadados)
            {
                if (identificador.StartsWith("LISTA:"))
                {
                    foreach (var valorUnico in valor.Split(";"))
                    {
                        await _dispositivoMetadadoRepository.Insert(new DispositivoMetadado
                        {
                            IdDispositivo = dispositivo.Id,
                            Identificador = identificador,
                            Valor = valorUnico
                        });
                    }
                }
                else
                {
                    await _dispositivoMetadadoRepository.Insert(new DispositivoMetadado
                    {
                        IdDispositivo = dispositivo.Id,
                        Identificador = identificador,
                        Valor = valor
                    });
                }
            }

            await _dispositivoMetadadoRepository.SaveChanges();

            return true;
        }

        public async Task<bool> RequestNotificationAsync(NotificationRequest notificationRequest,
            CancellationToken token)
        {
            var metadados = new Dictionary<string, string>();
            var payload = new Dictionary<string, string>();

            metadados.Add("LISTA:LICITACOES_FAVORITAS", notificationRequest.CodigoLicitacao);
            payload.Add("codigoLicitacao", notificationRequest.CodigoLicitacao);
           
            return (await NotificarAsync(new NotificarDTO
            {
                Titulo = null,
                Tipo = "ANEXO_LICITACAO",
                Texto = notificationRequest.Texto,
                Cnpj = null,
                Metadados = metadados,
                Payload = payload
            })).SucessoContagem > 0;
        }

        public async Task<NotificarResultadoDTO> NotificarAsync(NotificarDTO dto)
        {
            var query = from dispositivoBanco in await _dispositivoRepository.GetAll() select dispositivoBanco;

            if (dto.Cnpj != null)
            {
                query = from dispositivoBanco in query
                    where dispositivoBanco.CnpjVinculado == dto.Cnpj
                    select dispositivoBanco;
            }

            foreach (var (identificador, valor) in dto.Metadados)
            {
                if (identificador.StartsWith("LISTA:"))
                {
                    var valores = valor.Split(";");
                    query = from dispositivoBanco in query
                        where dispositivoBanco.Metadados.Any(metadado =>
                            metadado.Identificador == identificador && valores.Contains(metadado.Valor))
                        select dispositivoBanco;
                }
                else
                {
                    query = from dispositivoBanco in query
                        where dispositivoBanco.Metadados.Any(metadado =>
                            metadado.Identificador == identificador && metadado.Valor == valor)
                        select dispositivoBanco;
                }
            }

            var dispositivos = await query.ToListAsync();

            dto.Payload.Add("tipoNotificacao", dto.Tipo);

            var mensagens = dispositivos.Select(dispositivo => new Message
            {
                Token = dispositivo.FcmToken,
                Notification = new Notification
                {
                    Title = dto.Titulo,
                    Body = dto.Texto
                },
                Data = dto.Payload
            });

            var response = await FirebaseMessaging.DefaultInstance.SendAllAsync(mensagens);

            var dispositivosComFalhas = response.Responses.Select((r, i) => (r, i)).Aggregate(
                new List<NotificarResultadoFalhaDispositivoDTO>(),
                (acc, r) =>
            {
                if (r.r.IsSuccess)
                {
                    return acc;
                }
                
                acc.Add(new NotificarResultadoFalhaDispositivoDTO
                {
                    Id = dispositivos[r.i].Id,
                    MensagemFalha = r.r.Exception.Message
                });
               
                return acc;
            });
            
            return new NotificarResultadoDTO
            {
                FalhaContagem = response.FailureCount,
                SucessoContagem = response.SuccessCount,
                DispositivosComFalha = dispositivosComFalhas
            };
        }
    }
}