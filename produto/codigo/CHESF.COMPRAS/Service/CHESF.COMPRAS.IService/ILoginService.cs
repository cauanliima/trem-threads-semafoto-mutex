using System.Threading.Tasks;

namespace CHESF.COMPRAS.IService
{
    public interface ILoginService
    {
        Task<dynamic> Autenticar(string usuario, string senha);
    }
}