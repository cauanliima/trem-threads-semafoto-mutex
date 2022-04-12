using System;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CHESF.COMPRAS.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService, ILoginService loginService)
        {
            _tokenService = tokenService;
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
                if (usuarioRetornado == null) return Unauthorized("As credenciais digitadas são inválidas");

                return Ok(new
                {
                    usuario = usuarioRetornado,
                    token = _tokenService.GenerateToken(usuarioRetornado)
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem(ex.Message);
            }
        }
    }
}