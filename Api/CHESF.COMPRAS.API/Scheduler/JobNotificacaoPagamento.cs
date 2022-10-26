﻿using System;
using System.Threading.Tasks;
using CHESF.COMPRAS.IService;
using Microsoft.Extensions.Logging;
using Quartz;

namespace CHESF.COMPRAS.API.Scheduler
{
    public class JobNotificacaoPagamento : Rotina
    {
        private readonly ILogger<JobNotificacaoPagamento> _logger;
        private readonly IGerarNotificacaoPagamentoService _gerarNotificacaoPagamentoService;

        public JobNotificacaoPagamento(
            ILogger<JobNotificacaoPagamento> logger,
            IGerarNotificacaoPagamentoService gerarNotificacaoPagamentoService
        )
        {
            _logger = logger;
            _gerarNotificacaoPagamentoService = gerarNotificacaoPagamentoService;
        }

        protected override string NomeRotina => "JobNotificacaoPagamento";

        protected override async Task ProcessarRotina(IJobExecutionContext context)
        {
            try
            {
                await _gerarNotificacaoPagamentoService.GerarPagamentos();
            }
            catch (Exception)
            {
                _logger.LogError("Não foi possível gerar as notificações de pagamento => Hora: {Hora}", DateTime.Now);
                throw;
            }
        }
    }
}