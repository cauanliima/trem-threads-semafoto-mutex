using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.IService;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace CHESF.COMPRAS.Service
{
    public class EmailService : IEmailService
    {
    
        private readonly ITemplateService _templateService;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            ITemplateService templateService,
            ILogger<EmailService> logger
        )
        {
            _templateService = templateService;
            _logger = logger;
        }
        
        public async Task EnviarEmail<T>(string view, T modelo, string assunto, CustomModelDTO custom)
            {
                var nomesParametros = new List<string>
                {
                    "EMAIL_REMETENTE",
                    "SMTP_USUARIO",
                    "SMTP_SENHA",
                    "SMTP_HOST",
                    "SMTP_PORTA",
                    "SMTP_SSL",
                    "EMAIL_PARA"
                };

                foreach (var destinatario in custom.Destinatarios)
                {
                    if(destinatario != null)
                        Console.WriteLine("Está sendo enviado um e-mail para: " + destinatario?.ToLower());
                }
                
                var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
                environment ??= "PROD";
                
                var localHost = Environment.GetEnvironmentVariable("SMTP_HOST");
                var localUser = Environment.GetEnvironmentVariable("SMTP_USER");
                var localPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");

                // var parametros = await (from parametro in await _parametroRepository.GetAll()
                //         where nomesParametros.Contains(parametro.NmParametro)
                //         select parametro)
                //     .ToDictionaryAsync(parametro => parametro.NmParametro, parametro => parametro.VlParametro);
                
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("SMTP_HOST", "sandbox.smtp.mailtrap.io");
                parametros.Add("EMAIL_PARA", "joilsonabrantes@gmail.com");
                parametros.Add("SMTP_USUARIO", "81347496a5acf7");
                parametros.Add("SMTP_SENHA", "e8ecd9aee89e95");

                var para = parametros.GetValueOrDefault("EMAIL_PARA", "").Split(";").ToList();
                var html = await _templateService.GetTemplateHtmlStringAsync($"Email/{view}", modelo, custom, environment);
                var de = parametros.GetValueOrDefault("EMAIL_REMETENTE", "noreply@chesf.com.br");
                var username = parametros!.GetValueOrDefault("SMTP_USUARIO", null);
                var password = parametros!.GetValueOrDefault("SMTP_SENHA", null);
                var port = Convert.ToInt32(parametros.GetValueOrDefault("SMTP_PORTA", "465"));
                var ssl = Convert.ToInt32(parametros.GetValueOrDefault("SMTP_SSL", "0")) == 1;

                if (!parametros.TryGetValue("SMTP_HOST", out var host))
                {
                    return;
                }

                var message = new MimeMessage();

                try
                {
                    if (environment is "LOCAL" or "DEV")
                    {
                        host = localHost;
                        username = localUser;
                        password = localPassword;
                    }
                    else
                    {
                        username = null;
                        password = null;
                    }
                    
                    if (environment is "PROD")
                        para = custom.Destinatarios;

                    message.From.Add(new MailboxAddress(de, de));

                    var listaPara = para;

                    foreach (var destinatario in listaPara)
                    {
                        if(!string.IsNullOrEmpty(destinatario))
                            message.To.Add(new MailboxAddress(destinatario, destinatario));
                    }

                    message.Subject = environment switch
                    {
                        "DEV" => $"[DESENVOLVIMENTO] - {assunto}",
                        "HOMOLOG" => $"[HOMOLOGAÇÃO] - {assunto}",
                        _ => assunto
                    };

                    message.Body = new TextPart("html")
                    {
                        Text = html
                    };

                    using var client = new MailKit.Net.Smtp.SmtpClient();
                    await client.ConnectAsync(host, port, ssl);
                    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                    {
                        await client.AuthenticateAsync(username, password);
                    }

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ocorreu um problema ao tentar enviar um e-mail: \"{Assunto}\"", assunto);
                }
            }
    }
}