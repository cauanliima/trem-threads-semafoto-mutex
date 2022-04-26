using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;

namespace CHESF.COMPRAS.IService
{
    public interface ILoginService
    {
        Task<UsuarioDTO?> Autenticar(string usuario, string senha);
    }
}