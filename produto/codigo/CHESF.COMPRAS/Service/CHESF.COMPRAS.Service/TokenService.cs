using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.Domain.Enum;
using Microsoft.IdentityModel.Tokens;

namespace CHESF.COMPRAS.Service
{
    public class TokenService
    {
          public static string GenerateToken(UsuarioDTO usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("fedaf7d8863b48e197b9287d492b708e");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, usuario.Nome),
            }),
            Expires = DateTime.UtcNow.AddHours(4),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            NotBefore = DateTime.UtcNow
        };

        foreach (var perfil in usuario.Perfis)
        {
            tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, perfil.Nome));
        }
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public static string GenerateToken(ClaimsPrincipal principal)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("fedaf7d8863b48e197b9287d492b708e");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(),
            Expires = DateTime.UtcNow.AddHours(4),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            NotBefore = DateTime.UtcNow
        };

        foreach (var claim in principal.Claims)
        {
            tokenDescriptor.Subject.AddClaim(claim);
        }
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static StatusToken ValidarToken(string token)
    {
        var securityToken = GetSecurityToken(token);
        if (securityToken != null)
        {
            var dataAtual = DateTime.UtcNow;
            var dataRefresh = securityToken.ValidFrom.AddMinutes(30);
            var dataExpiracao = securityToken.ValidTo;
        
            if (dataAtual > dataExpiracao) {
                return StatusToken.Expirado;
            } else if (dataAtual > dataRefresh) {
                return StatusToken.Refresh;
            }
            return StatusToken.Valido;
        }

        return StatusToken.Expirado;
    }

    public static SecurityToken GetSecurityToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token);
        var securityToken = jsonToken as JwtSecurityToken;

        return securityToken;
    }

    public static ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fedaf7d8863b48e197b9287d492b708e")),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Token inválido");

        return principal;
    }
    }
}