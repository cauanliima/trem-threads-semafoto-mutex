using System.Threading.Tasks;

namespace CHESF.COMPRAS.IService
{
    public interface IOTPService
    {
        Task<string>  gerarOTP(long cnpj);
        bool validarOTP(long cnpj, string otp);
    }
}