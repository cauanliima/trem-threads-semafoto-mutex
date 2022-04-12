using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CHESF.COMPRAS.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public long? GetTokenCNPJ()
        {
            if (_httpContextAccessor.HttpContext == null) return null;
            var user = _httpContextAccessor.HttpContext.User;
            var sid = ((ClaimsIdentity)user.Identity)?.FindFirst(ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(sid)) return null;
            if (!long.TryParse(sid, out var cnpj)) return null;
            return cnpj;
        }

        public string GenerateToken(UsuarioDTO usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt").GetSection("Secret").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Sid, usuario.Cnpj),
                    new Claim(ClaimTypes.Role, usuario.Perfis.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(
                    double.Parse(_configuration.GetSection("Jwt").GetSection("ExpirationTime").Value)
                ),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}