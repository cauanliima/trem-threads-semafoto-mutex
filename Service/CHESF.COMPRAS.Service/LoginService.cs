using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.IService;
using ServiceReference;

namespace CHESF.COMPRAS.Service
{
    public class LoginService :ILoginService
    {

        public async Task<UsuarioDTO?> Autenticar(string usuario, string senha)
        {
            var wsf = new ServicoFornecedoresClient();
            string webService = "http://recpabv1/aplic/fornecedores.nsf/Fornecedores?WSDL";
            var ambiente = Environment.GetEnvironmentVariable("MODO_EXECUCAO_SISTEMA") ;
            if (ambiente == "DESENVOLVIMENTO")
            {
                webService = "http://recdapl1.redechesf.local/aplic/fornecedores.nsf/fornecedores?wsdl";
            }
            wsf.Endpoint.Address = new EndpointAddress(webService);
            if (ambiente != "DESENVOLVIMENTO")
            {
                var validarLogin = await wsf.AutenticaAsync(usuario, senha);

                if (!validarLogin.AutenticaReturn)
                {
                    return null;
                }
            }
           
            var fornecedor = (await wsf.RetornarUsuarioPorCNPJAsync(usuario)).RetornarUsuarioPorCNPJReturn;

            if (fornecedor?.CNPJ == null)
            {
                return null;
            }

            return new UsuarioDTO
            {
                Nome = fornecedor.Nome,
                Cnpj = fornecedor.CNPJ.Trim(),
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