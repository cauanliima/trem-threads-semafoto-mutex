using System;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.Domain.Exception;
using CHESF.COMPRAS.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CHESF.COMPRAS.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("otp")]
    public class OTPController : ControllerBase
    {
        private readonly IOTPService _otpService;
        private readonly ITokenService _tokenService;
        private readonly IFornecedorService _fornecedorService;

        public OTPController(IOTPService otpService, ITokenService tokenService, IFornecedorService fornecedorService)
        {
            _otpService = otpService;
            _tokenService = tokenService;
            _fornecedorService = fornecedorService;
        }
        
        [HttpPost]
        [Route("gerar")]
        public async Task<IActionResult> GerarOTP(long cnpj)    
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                var otp = _otpService.gerarOTP(cnpj);
                return Ok(otp);
            }
            catch (HttpResponseException rex)
            {
                Console.WriteLine(rex);
                return Problem(statusCode: rex.Status, detail: rex.Mensagem);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem(ex.Message);
            }
        }
        
        [HttpPost]
        [Route("veriricar")]
        public async Task<ActionResult<UsuarioDTO>> VerificarOTP([FromBody] ParametrosValidacaoOTP parametros)    
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                bool otpValido = _otpService.validarOTP(parametros.cnpj, parametros.otp);
                if (otpValido)
                {
                    var usuarioRetornado = await _fornecedorService.buscarUsuario(parametros.cnpj);
                    return Ok(new
                    {
                        usuario = usuarioRetornado,
                        token = _tokenService.GenerateToken(usuarioRetornado)
                    });
                }

                return Unauthorized("As credenciais digitadas são inválidas");
            }
            catch (HttpResponseException rex)
            {
                Console.WriteLine(rex);
                return Problem(statusCode: rex.Status, detail: rex.Mensagem);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem(ex.Message);
            }
        }
        
    }
}