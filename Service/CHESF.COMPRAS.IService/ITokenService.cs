using CHESF.COMPRAS.Domain.DTOs;

namespace CHESF.COMPRAS.IService
{
    public interface ITokenService
    {
        long? GetTokenCNPJ();
        string GenerateToken(UsuarioDTO usuario);
    }
}