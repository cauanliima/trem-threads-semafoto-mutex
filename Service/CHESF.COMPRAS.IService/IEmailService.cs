using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;

namespace CHESF.COMPRAS.IService
{
    public interface IEmailService
    {
        Task EnviarEmail<T>(string nomeView, T modelo, string assunto, CustomModelDTO custom);
    }
}