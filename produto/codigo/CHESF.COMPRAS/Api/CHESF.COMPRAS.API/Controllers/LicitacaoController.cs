using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.Domain.E_Edital;
using CHESF.COMPRAS.Domain.QueryParams;
using CHESF.COMPRAS.IService;
using Microsoft.AspNetCore.Mvc;

namespace CHESF.COMPRAS.API.Controllers
{
    [ApiController]
    [Route("licitacoes")]
    public class LicitacaoController : ControllerBase
    {
        private readonly ILicitacaoService _service;

        public LicitacaoController(ILicitacaoService licitacaoService)
        {
            _service = licitacaoService;
        }

        [HttpGet]
        [Route("recentes")]
        public async Task<ActionResult<List<LicitacaoDTO>>> ListarRecentes(
            [FromQuery] LicitacoesRecentesQueryParams queryParams)
        {
            try
            {
                var licitacoes = await _service.Listar(queryParams);
                return Ok(licitacoes);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao buscar as informações. Erro: {ex}");
            }
        }

        [HttpGet]
        [Route("filtro")]
        public async Task<ActionResult<List<LicitacaoDTO>>> ListarComFiltro(
            [FromQuery] LicitacaoFiltroQueryParams queryParams)
        {
            try
            {
                var licitacoes = await _service.Listar(queryParams);
                return Ok(licitacoes);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao buscar as informações. Erro: {ex}");
            }
        }

        [HttpGet]
        [Route("favoritadas")]
        public async Task<ActionResult<List<LicitacaoDTO>>> ListarComFiltro(
            [FromQuery] LicitacoesFavoritadasQueryParams queryParams)
        {
            try
            {
                var licitacoes = await _service.Listar(queryParams);
                return Ok(licitacoes);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao buscar as informações. Erro: {ex}");
            }
        }

        [HttpGet]
        [Route("{id:long}")]
        public async Task<ActionResult<Licitacao>> Buscar(long id)
        {
            try
            {
                var licitacao = await _service.Buscar(id);
                if (licitacao == null) return NotFound();
                return Ok(licitacao);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception($"Ocorreu um erro ao buscar a licitação. Erro: {e}");
            }
        }
    }
}