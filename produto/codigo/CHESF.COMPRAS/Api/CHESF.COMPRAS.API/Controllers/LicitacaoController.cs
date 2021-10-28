using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.Domain.E_Edital;
using Microsoft.AspNetCore.Mvc;

namespace CHESF.COMPRAS.API.Controllers
{
    [ApiController]
    // [Authorize]
    [Route("licitacoes")]
    public class LicitacaoController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<LicitacaoDTO>>> Listar()
        {
            try
            {
                var listaLicitacoes = new List<LicitacaoDTO>();
                ListaLicitacoesMock().ForEach(licitacao => listaLicitacoes.Add(new LicitacaoDTO()
                {
                    Codigo = licitacao.Codigo,
                    Descricao = licitacao.Descricao,
                    Nome = licitacao.Nome,
                    Numero = licitacao.Numero,
                    DataEnvioFinal = licitacao.DataEnvioFinal
                }));
                return Ok(listaLicitacoes);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao buscar as informações. Erro: {ex}");
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<LicitacaoAnexosDTO>> Buscar(int id)
        {
            try
            {
                Licitacao licitacao = ListaLicitacoesMock().Find(lic => lic.Codigo.Equals(id)) ??
                                      throw new ArgumentNullException(
                                          "ListaLicitacoesMock().Find(lic => lic.Codigo.Equals(id))");

                List<Anexo> anexos = ListaAnexosMock();
                anexos.ForEach(action: anexo => anexo.CodigoLicitacao = id);

                return Ok(new LicitacaoAnexosDTO
                {
                    Licitacao = licitacao,
                    Anexos = anexos
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception($"Ocorreu um erro ao buscar a licitação. Erro: {e}");
            }
        }

        private static List<Licitacao> ListaLicitacoesMock()
        {
            return new List<Licitacao>()
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
                    Descricao =
                        "CONSTRUÇÃO DE GUARITA COM RECEPÇÃO DE ACESSO ÀS SUBESTAÇÕES: QUIXADÁ, CAUÍPE, PICI II, SANTA CRUZ II, SANTANA DO MATOS II, NATAL II, MUSSURÉ II, CÍCERO DANTAS E XINGÓ",
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
                    Descricao =
                        "SERVIÇOS DE VIGILÂNCIA MOTORIZADA E DESARMADA NOTURNA PARA AS ÁREAS DO PROJETO JUSANTE",
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
                    Descricao =
                        "FORNECIMENTO DE EQUIPAMENTOS, MATERIAIS E SERVIÇOS PARA AMPLIAÇÕES, REFORÇOS E MELHORIAS NA SUBESTAÇÃO ANGELIM",
                    Numero = "0832/2021",
                    CriterioJulgamento = 'M',
                    DataEnvioInicial = new DateTime(2021, 8, 1, 8, 0, 0),
                    DataEnvioFinal = new DateTime(2021, 8, 4, 8, 0, 0),
                    AberturaPropostas = new DateTime(2021, 8, 4, 9, 0, 0)
                },
                new Licitacao()
                {
                    Codigo = 2,
                    Nome = "CSI",
                    Descricao = "CONTRATAÇÃO DE AMPLIAÇÕES E REFORÇOS NAS SUBESTAÇÕES SUAPE II, ANGELIM II E RECIFE II",
                    Numero = "5404/2021",
                    CriterioJulgamento = 'M',
                    DataEnvioInicial = new DateTime(2021, 8, 1, 8, 0, 0),
                    DataEnvioFinal = new DateTime(2021, 8, 5, 8, 0, 0),
                    AberturaPropostas = new DateTime(2021, 8, 5, 9, 0, 0)
                },
                new Licitacao()
                {
                    Codigo = 1,
                    Nome = "PG",
                    Descricao = "AQUISIÇÃO DE CONJUNTO METÁLICO - ESTRUTURAS METÁLICAS",
                    Numero = "7304/2021",
                    CriterioJulgamento = 'M',
                    DataEnvioInicial = new DateTime(2021, 8, 1, 8, 0, 0),
                    DataEnvioFinal = new DateTime(2021, 8, 6, 8, 0, 0),
                    AberturaPropostas = new DateTime(2021, 8, 6, 9, 0, 0)
                }
            };
        }

        private static List<Anexo> ListaAnexosMock()
        {
            return new List<Anexo>()
            {
                new Anexo()
                {
                    Codigo = 1,
                    Nome = "PG-3932.2021-Edital",
                    Descricao = "",
                    Extensao = "pdf",
                    DataCriacao = new DateTime(2021, 10, 3, 16, 37, 0),
                    CodigoLicitacao = 6,
                    Link = ""
                },
                new Anexo()
                {
                    Codigo = 2,
                    Nome = "TR-DOTT_022_2021 - VoIP",
                    Descricao = "",
                    Extensao = "pdf",
                    DataCriacao = new DateTime(2021, 10, 3, 16, 45, 0),
                    CodigoLicitacao = 6,
                    Link = ""
                },
                new Anexo()
                {
                    Codigo = 3,
                    Nome = "ANEXO I - Planilha de Preços - 3932.2021",
                    Descricao = "",
                    Extensao = "docx",
                    DataCriacao = new DateTime(2021, 10, 3, 17, 28, 0),
                    CodigoLicitacao = 6,
                    Link = ""
                },
                new Anexo()
                {
                    Codigo = 4,
                    Nome = "Anexo 02 - Relação das Localidades - Sites",
                    Descricao = "",
                    Extensao = "pdf",
                    DataCriacao = new DateTime(2021, 10, 3, 17, 37, 0),
                    CodigoLicitacao = 6,
                    Link = ""
                },
                new Anexo()
                {
                    Codigo = 5,
                    Nome = "Anexo 01 - Quant. de material e serviços",
                    Descricao = "",
                    Extensao = "xlsx",
                    DataCriacao = new DateTime(2021, 10, 3, 18, 26, 0),
                    CodigoLicitacao = 6,
                    Link = ""
                },
                new Anexo()
                {
                    Codigo = 6,
                    Nome = "INFO-DGCSA-3932.2021.01",
                    Descricao = "",
                    Extensao = "docx",
                    DataCriacao = new DateTime(2021, 10, 22, 15, 13, 0),
                    CodigoLicitacao = 6,
                    Link = ""
                },
                new Anexo()
                {
                    Codigo = 7,
                    Nome = "INFO-DGCSA-3932.2021.02",
                    Descricao = "",
                    Extensao = "docx",
                    DataCriacao = new DateTime(2021, 10, 25, 16, 46, 0),
                    CodigoLicitacao = 6,
                    Link = ""
                }
            };
        }
    }
}