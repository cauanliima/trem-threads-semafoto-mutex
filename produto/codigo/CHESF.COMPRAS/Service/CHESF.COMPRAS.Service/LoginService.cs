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

        public async Task<dynamic> Autenticar(string usuario, string senha)
        {
            var wsf = new ServicoFornecedoresClient();
            var webService = "http://recdapl1.redechesf.local/aplic/fornecedores.nsf/fornecedores?wsdl";
            var ambiente = "DESENVOLVIMENTO";
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

            return new UsuarioDTO()
            {
                Nome = fornecedor.Nome,
                Cnpj = fornecedor.CNPJ.Trim(),
                Perfis = new List<PerfilDTO>
                {
                    new PerfilDTO
                    {
                        Nome = "fornecedor"
                    }
                }
            };
        }
    }
}