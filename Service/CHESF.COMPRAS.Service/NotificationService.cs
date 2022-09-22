using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.APP;
using CHESF.COMPRAS.IService;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CHESF.COMPRAS.Service
{
    public class NotificationService : INotificationService
    {
        readonly NotificationHubClient _hub;
        readonly Dictionary<string, NotificationPlatform> _installationPlatform;
        readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
            _hub = NotificationHubClient.CreateClientFromConnectionString(
                Environment.GetEnvironmentVariable("AZURE_NH_CONNECTION"),
                Environment.GetEnvironmentVariable("AZURE_NH_NAME"));

            _installationPlatform = new Dictionary<string, NotificationPlatform>
            {
                { nameof(NotificationPlatform.Apns).ToLower(), NotificationPlatform.Apns },
                { nameof(NotificationPlatform.Fcm).ToLower(), NotificationPlatform.Fcm }
            };
        }

        public async Task<bool> CreateOrUpdateInstallationAsync(DeviceInstallation deviceInstallation,
            CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(deviceInstallation?.InstallationId) ||
                string.IsNullOrWhiteSpace(deviceInstallation?.Platform) ||
                string.IsNullOrWhiteSpace(deviceInstallation?.PushChannel))
                return false;

            var installation = new Installation()
            {
                InstallationId = deviceInstallation.InstallationId,
                PushChannel = deviceInstallation.PushChannel,
                Tags = deviceInstallation.Tags
            };

            if (_installationPlatform.TryGetValue(deviceInstallation.Platform, out var platform))
                installation.Platform = platform;
            else
                return false;

            try
            {
                await _hub.CreateOrUpdateInstallationAsync(installation, token);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteInstallationByIdAsync(string installationId, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(installationId))
                return false;

            try
            {
                await _hub.DeleteInstallationAsync(installationId, token);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RequestNotificationAsync(NotificationRequest notificationRequest,
            CancellationToken token)
        {
            if ((notificationRequest.Silent &&
                 string.IsNullOrWhiteSpace(notificationRequest?.Action)) ||
                (!notificationRequest.Silent &&
                 (string.IsNullOrWhiteSpace(notificationRequest?.Texto)) ||
                 string.IsNullOrWhiteSpace(notificationRequest?.Action)))
                return false;

            var androidPushTemplate =
                notificationRequest.Silent ? PushTemplates.Silent.Android : PushTemplates.Generic.Android;

            var iOSPushTemplate = notificationRequest.Silent ? PushTemplates.Silent.iOS : PushTemplates.Generic.iOS;

            var androidPayload = PrepareNotificationPayload(
                androidPushTemplate,
                notificationRequest.Texto,
                notificationRequest.Action,
                notificationRequest.NumeroLicitacao);

            var iOSPayload = PrepareNotificationPayload(
                iOSPushTemplate,
                notificationRequest.Texto,
                notificationRequest.Action,
                notificationRequest.NumeroLicitacao);

            List<string> tags = new List<String> { notificationRequest.CodigoLicitacao };

            try
            {
                if (tags.Count == 0)
                {
                    // This will broadcast to all users registered in the notification hub
                    await SendPlatformNotificationsAsync(androidPayload, iOSPayload, token);
                }
                else if (tags.Count <= 20)
                {
                    await SendPlatformNotificationsAsync(androidPayload, iOSPayload, tags, token);
                }
                else
                {
                    var notificationTasks = tags
                        .Select((value, index) => (value, index))
                        .GroupBy(g => g.index / 20, i => i.value)
                        .Select(tags => SendPlatformNotificationsAsync(androidPayload, iOSPayload, tags, token));

                    await Task.WhenAll(notificationTasks);
                }

                _logger.LogInformation("Notificação enviada");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro inseperado ao enviar notificações");
                return false;
            }
        }

        string PrepareNotificationPayload(string template, string text, string action, string licitacao) => template
            .Replace("$(licitacao)", licitacao, StringComparison.InvariantCulture)
            .Replace("$(alertMessage)", text, StringComparison.InvariantCulture)
            .Replace("$(alertAction)", action, StringComparison.InvariantCulture);

        Task SendPlatformNotificationsAsync(string androidPayload, string iOSPayload, CancellationToken token)
        {
            var sendTasks = new Task[]
            {
                _hub.SendFcmNativeNotificationAsync(androidPayload, token),
                _hub.SendAppleNativeNotificationAsync(iOSPayload, token)
            };

            return Task.WhenAll(sendTasks);
        }

        Task SendPlatformNotificationsAsync(string androidPayload, string iOSPayload, IEnumerable<string> tags,
            CancellationToken token)
        {
            var sendTasks = new Task[]
            {
                _hub.SendFcmNativeNotificationAsync(androidPayload, tags, token),
                _hub.SendAppleNativeNotificationAsync(iOSPayload, tags, token)
            };

            return Task.WhenAll(sendTasks);
        }
    }
}