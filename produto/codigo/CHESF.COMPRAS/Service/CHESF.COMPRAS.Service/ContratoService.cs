using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.QueryParams;
using CHESF.COMPRAS.Domain.SGNF;
using CHESF.COMPRAS.IRepository;
using CHESF.COMPRAS.IService;

namespace CHESF.COMPRAS.Service
{
    public class ContratoService : IContratoService
    {
        private readonly IContratoRepository _contratoRepository;
        private readonly INotaFiscalRepository _notaFiscalRepository;
        private readonly ITokenService _tokenService;

        public ContratoService(IContratoRepository contratoRepository, ITokenService tokenService,
            INotaFiscalRepository notaFiscalRepository)
        {
            _contratoRepository = contratoRepository;
            _notaFiscalRepository = notaFiscalRepository;
            _tokenService = tokenService;
        }

        public async Task<IEnumerable<Contrato>> Listar(ListaQueryParams queryParams)
        {
            var cnpj = _tokenService.GetTokenCNPJ();

            if (cnpj == null) return Enumerable.Empty<Contrato>();

            return await _contratoRepository.ListarParaCNPJ((long)cnpj, queryParams.pagina, queryParams.total);
        }

        public async Task<IEnumerable<NotaFiscal>> ListarNotasFiscais(int idContrato, ListaQueryParams queryParams)
        {
            var cnpj = _tokenService.GetTokenCNPJ();

            if (cnpj == null) return Enumerable.Empty<NotaFiscal>();

            return await _notaFiscalRepository.ListarParaContrato(idContrato, queryParams.pagina, queryParams.total);
        }
    }
}