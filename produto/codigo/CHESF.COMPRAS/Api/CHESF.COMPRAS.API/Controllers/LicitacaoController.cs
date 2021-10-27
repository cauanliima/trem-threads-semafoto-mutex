using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.E_Edital;
using Microsoft.AspNetCore.Mvc;

namespace CHESF.COMPRAS.API.Controllers
{
    [ApiController]
    // [Authorize]
    [Route("licitacoes")]
    public class LicitacaoController: ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Licitacao>>> Listar()
        {
            try
            {
                var licitacoesMock = new List<Licitacao>(new[]
                {
                    new Licitacao()
                    {
                        Codigo = 6,
                        Nome = "PG",
                        Descricao = "Implantação do Sistema de Telefonia VoIP das Usinas e Subestações da Chesf",
                        Numero = "3932/2021",
                        CriterioJulgamento = 'M',
                        DataEnvioInicial = DateTime.Now,
                        DataEnvioFinal = DateTime.Now.AddDays(10),
                        AberturaPropostas = DateTime.Now.AddDays(10).AddHours(1)
                    },
                    new Licitacao()
                    {
                        Codigo = 5,
                        Nome = "LIC",
                        Descricao = "CONSTRUÇÃO DE GUARITA COM RECEPÇÃO DE ACESSO ÀS SUBESTAÇÕES: QUIXADÁ, CAUÍPE, PICI II, SANTA CRUZ II, SANTANA DO MATOS II, NATAL II, MUSSURÉ II, CÍCERO DANTAS E XINGÓ",
                        Numero = "5009/2021",
                        CriterioJulgamento = 'M',
                        DataEnvioInicial = DateTime.Now.AddDays(1),
                        DataEnvioFinal = DateTime.Now.AddDays(15),
                        AberturaPropostas = DateTime.Now.AddDays(15).AddHours(1)
                    },
                    new Licitacao()
                    {
                        Codigo = 4,
                        Nome = "PG",
                        Descricao = "SERVIÇOS DE VIGILÂNCIA MOTORIZADA E DESARMADA NOTURNA PARA AS ÁREAS DO PROJETO JUSANTE",
                        Numero = "6364/2021",
                        CriterioJulgamento = 'M',
                        DataEnvioInicial = DateTime.Now.AddDays(2),
                        DataEnvioFinal = DateTime.Now.AddDays(20),
                        AberturaPropostas = DateTime.Now.AddDays(20).AddHours(1)
                    },
                    new Licitacao()
                    {
                        Codigo = 3,
                        Nome = "CSI",
                        Descricao = "FORNECIMENTO DE EQUIPAMENTOS, MATERIAIS E SERVIÇOS PARA AMPLIAÇÕES, REFORÇOS E MELHORIAS NA SUBESTAÇÃO ANGELIM",
                        Numero = "0832/2021",
                        CriterioJulgamento = 'M',
                        DataEnvioInicial = new DateTime(2021, 8, 1,8,0,0),
                        DataEnvioFinal = new DateTime(2021, 8, 4,8,0,0),
                        AberturaPropostas = new DateTime(2021, 8, 4, 9,0,0)
                    },
                    new Licitacao()
                    {
                        Codigo = 2,
                        Nome = "CSI",
                        Descricao = "CONTRATAÇÃO DE AMPLIAÇÕES E REFORÇOS NAS SUBESTAÇÕES SUAPE II, ANGELIM II E RECIFE II",
                        Numero = "5404/2021",
                        CriterioJulgamento = 'M',
                        DataEnvioInicial = new DateTime(2021, 8, 1,8,0,0),
                        DataEnvioFinal = new DateTime(2021, 8, 5,8,0,0),
                        AberturaPropostas = new DateTime(2021, 8, 5, 9,0,0)
                    },
                    new Licitacao()
                    {
                        Codigo = 1,
                        Nome = "PG",
                        Descricao = "AQUISIÇÃO DE CONJUNTO METÁLICO - ESTRUTURAS METÁLICAS",
                        Numero = "7304/2021",
                        CriterioJulgamento = 'M',
                        DataEnvioInicial = new DateTime(2021, 8, 1,8,0,0),
                        DataEnvioFinal = new DateTime(2021, 8, 6,8,0,0),
                        AberturaPropostas = new DateTime(2021, 8, 6, 9,0,0)
                    }
                });

                return Ok(licitacoesMock);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao buscar as informações. Erro: {ex}");
            }
        }
    }
}