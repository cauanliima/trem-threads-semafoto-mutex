using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.QueryParams;
using CHESF.COMPRAS.Domain.SGNF;
using CHESF.COMPRAS.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CHESF.COMPRAS.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("contratos")]
    public class ContratoController : ControllerBase
    {
        private readonly IContratoService _contratoService;

        public ContratoController(IContratoService contratoService)
        {
            _contratoService = contratoService;
        }

        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<Contrato>>> Listar(
            [FromQuery] ListaQueryParams queryParams
        )
        {
            var contratos = await _contratoService.Listar(queryParams);
            return Ok(contratos.ToList());
        }

        [HttpGet]
        [Route("porNumeroContrato/{numeroContrato}/")]
        public async Task<ActionResult<Contrato>> Detalhar(string numeroContrato)
        {
            var contrato = await _contratoService.Detalhar(numeroContrato);

            if (contrato == null)
            {
                return NotFound();
            }

            return Ok(contrato);
        }
        
        [HttpGet]
        [Route("{id:int}/notas-fiscais/")]
        public async Task<ActionResult<List<NotaFiscal>>> ListarNotasFiscais(int id, [FromQuery] ListaQueryParams queryParams)
        {
            var notasFiscais = await _contratoService.ListarNotasFiscais(id, queryParams);
            return Ok(notasFiscais.ToList());
        }
    }
}