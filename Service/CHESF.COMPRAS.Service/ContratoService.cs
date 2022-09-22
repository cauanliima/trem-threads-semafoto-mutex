using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.QueryParams;
using CHESF.COMPRAS.Domain.SGNF;
using CHESF.COMPRAS.IRepository;
using CHESF.COMPRAS.IService;
using Microsoft.EntityFrameworkCore;

namespace CHESF.COMPRAS.Service
{
    public class ContratoService : IContratoService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IContratoFonecedorRepository _contratoFornecedorRepository;
        private readonly INotaFiscalRepository _notaFiscalRepository;
        private readonly ITokenService _tokenService;

        public ContratoService(
            IUsuarioRepository usuarioRepository,
            IContratoFonecedorRepository contratoFonecedorRepository,
            ITokenService tokenService,
            INotaFiscalRepository notaFiscalRepository
        )
        {
            _usuarioRepository = usuarioRepository;
            _contratoFornecedorRepository = contratoFonecedorRepository;
            _notaFiscalRepository = notaFiscalRepository;
            _tokenService = tokenService;
        }

        public async Task<IEnumerable<Contrato>> Listar(ListaQueryParams queryParams)
        {
            var cnpj = _tokenService.GetTokenCNPJ();

            if (cnpj == null) return Enumerable.Empty<Contrato>();

            var agora = DateTime.UtcNow;

            var contratos = await (await _contratoFornecedorRepository.GetAll())
                .Where(
                    contratoFornecedor => contratoFornecedor.Fornecedor!.CNPJ == cnpj
                    && (contratoFornecedor.DataInicio == null || contratoFornecedor.DataInicio <= agora)
                    && (contratoFornecedor.DataFim == null || contratoFornecedor.DataFim!.Value.AddDays(90) >= agora)
                    && !(contratoFornecedor.DataInicio == null && contratoFornecedor.DataFim == null)
                )
                .OrderBy(contratoFornecedor => contratoFornecedor.Contrato.Numero)
                .Include(contratoFornecedor => contratoFornecedor.Contrato)
                .ThenInclude(contrato => contrato.NotasFiscais)
                .Select(contratoFornecedor => contratoFornecedor.Contrato)
                .Skip(queryParams.pagina > 0 ? (queryParams.pagina - 1) * queryParams.total : 0)
                .Take(queryParams.total)
                .ToListAsync();

            foreach (var contrato in contratos.Where(contrato => (contrato.NotasFiscais?.Count ?? 0) > 0))
            {
                var ultimaNotaFiscal = contrato.NotasFiscais!.OrderByDescending(nota => nota.DataEmissao).First();

                if (ultimaNotaFiscal.IdAdministrador == null) continue;
               
                contrato.Administrador = await _usuarioRepository.FirstOrDefault(usuario =>
                    usuario.Codigo == ultimaNotaFiscal.IdAdministrador
                );
            }

            return contratos;
        }

        public async Task<IEnumerable<NotaFiscal>> ListarNotasFiscais(int idContrato, ListaQueryParams queryParams)
        {
            var cnpj = _tokenService.GetTokenCNPJ();

            if (cnpj == null) return Enumerable.Empty<NotaFiscal>();

            return await _notaFiscalRepository.ListarParaContrato(idContrato, queryParams.pagina, queryParams.total);
        }
    }
}