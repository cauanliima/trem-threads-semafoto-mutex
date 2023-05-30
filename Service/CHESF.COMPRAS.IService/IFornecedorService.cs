using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.Domain.SGNF;

namespace CHESF.COMPRAS.IService
{
    public interface IFornecedorService
    {
        Task<Fornecedor?> buscar(long cnpj);
        Task<UsuarioDTO?> buscarUsuario(long cnpj);

    }
}