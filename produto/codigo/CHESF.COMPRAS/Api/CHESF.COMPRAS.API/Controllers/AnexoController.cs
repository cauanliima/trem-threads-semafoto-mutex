using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.Domain.E_Edital;
using CHESF.COMPRAS.IService;
using CHESF.COMPRAS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CHESF.COMPRAS.API.Controllers
{
    [ApiController]
    [Route("anexos")]
    public class AnexoController : ControllerBase
    {
        private readonly IAnexoService _anexoService;

        public AnexoController(IAnexoService anexoService)
        {
            _anexoService = anexoService;
        }

        [HttpGet]
        [Route("licitacao/{id:long}")]
        public async Task<ActionResult<List<Anexo>>> Baixar(long id)
        {
            try
            {
                var anexos = await _anexoService.TodosDaLicitacao(id);
                return Ok(anexos);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception($"Ocorreu um erro ao buscar anexos.");
            }
        }

        [HttpGet]
        [Route("{id:int}/baixar")]
        public async Task<FileResult> Baixar(int id)
        {
            var arquivo = await _anexoService.Baixar(id);

            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(arquivo.nome, out contentType);

            contentType = string.IsNullOrWhiteSpace(contentType)
                ? (arquivo.nome.EndsWith(".kmz") ? "application/vnd.google-earth" : null)
                : contentType;

            return File(arquivo.arquivo, contentType, arquivo.nome);
        }
    }
}