using System;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.IService;
using CHESF.COMPRAS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CHESF.COMPRAS.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILoginService _loginService;

        public AuthController( IConfiguration configuration, ILoginService loginService)
        {
            _configuration = configuration;
            _loginService = loginService;
        }


        [HttpPost]
        [Route("")]
        public async Task<ActionResult<UsuarioDTO>> Autenticar([FromBody] ParametrosLoginDTO parametros)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                var usuarioRetornado = await _loginService.Autenticar(parametros.usuario, parametros.senha);
                if(usuarioRetornado == null) return Unauthorized("As credenciais digitadas são inválidas");
                
                return Ok(new
                {
                    usuario = usuarioRetornado,
                    token = TokenService.GenerateToken(usuarioRetornado)
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem(detail: ex.Message);
            }
        }

    }
}