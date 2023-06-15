using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.Domain.Exception;
using CHESF.COMPRAS.Domain.SGNF;
using CHESF.COMPRAS.IService;
using Microsoft.Extensions.Caching.Memory;

namespace CHESF.COMPRAS.Service
{
    public class OTPService : IOTPService
    {
        
        private readonly IOTPCache _cache;
        private readonly IFornecedorService _fornecedorService;
        private readonly IEmailService _emailService;

        
        public OTPService(IOTPCache cache, IFornecedorService fornecedorService, IEmailService emailService)
        {
            _cache = cache;
            _fornecedorService = fornecedorService;
            _emailService = emailService;
        }
        
        public async Task<string> gerarOTP(long cnpj)
        {

            var fornecedor =  _fornecedorService.buscar(cnpj);
            
            ValidarFornecedor(fornecedor);

            int tempoExpiracaoMinutos = 10;
            int otp = RandomNumberGenerator.GetInt32(100000, 999999);
            // Calcular o tempo de expiração
            DateTime dataExpiracao = DateTime.Now.AddMinutes(tempoExpiracaoMinutos);
            // Armazenar o OTP e o tempo de expiração no cache
            _cache.SetOTP(cnpj, otp.ToString(), dataExpiracao);
            var destinatarios = new List<string?> {};
            destinatarios.Add(fornecedor.Result?.Email);
            var model = new CustomModelDTO()
            {
                Destinatarios = destinatarios,
                Message = "Código OTP de Verificação - E-COMPRAS/SGNF"
            };
            await _emailService.EnviarEmail(
                "CodigoOTP",
                otp.ToString(),
                $"Código OTP de Verificação - E-COMPRAS/SGNF",
                model
            );
            return "Código OTP enviado para o e-mail: " + fornecedor.Result?.Email;;
        }

        private static void ValidarFornecedor(Task<Fornecedor?>? fornecedor)
        {
            if (fornecedor == null || fornecedor.Result == null)
            {
                throw new HttpResponseException(409,
                    "Não foi possível enviar o código de verificação para esse CNPJ. Verifique o CNPJ e tente novamente.");
            }

            if (string.IsNullOrEmpty(fornecedor.Result.Email))
            {
                throw new HttpResponseException(409,
                    "Não foi possível enviar o código de verificação para esse CNPJ. O fornecedor não possui e-mail cadastrado no SGNF.");
            }
        }

        public bool validarOTP(long cnpj, string otp)
        {

            // Recuperar o OTP e o tempo de expiração do cache
            string storedOTP = _cache.GetOTP(cnpj);
            if (storedOTP != null)
            {
                if (otp.Equals(storedOTP.ToString()))
                {
                    return true; // OTP válido
                }
            }

            return false;
        }
    }
}