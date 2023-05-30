using System.Collections.Generic;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.Domain.SGNF;
using CHESF.COMPRAS.IRepository;
using CHESF.COMPRAS.IService;

namespace CHESF.COMPRAS.Service
{
    public class FornecedorService : IFornecedorService
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        
        public FornecedorService( IFornecedorRepository fornecedorRepository)
        {
            _fornecedorRepository = fornecedorRepository;
        }
        
        public async Task<Fornecedor?> buscar(long cnpj)
        {
            var fornecedor = await _fornecedorRepository.FirstOrDefault(fornecedor => fornecedor!.CNPJ == cnpj);
            return fornecedor;
        }
        
        public async Task<UsuarioDTO?> buscarUsuario(long cnpj)
        {
            var fornecedor = await _fornecedorRepository.FirstOrDefault(fornecedor => fornecedor!.CNPJ == cnpj);
            return new UsuarioDTO
            {
                Nome = fornecedor.NomeFantasia,
                Cnpj = fornecedor.CNPJ.ToString(),
                Perfis = new List<PerfilDTO>
                {
                    new()
                    {
                        Nome = "fornecedor"
                    }
                }
            };
        }
    }
}