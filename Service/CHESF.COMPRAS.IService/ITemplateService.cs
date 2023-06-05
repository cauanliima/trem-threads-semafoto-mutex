using System.Threading.Tasks;

namespace CHESF.COMPRAS.IService
{
    public interface ITemplateService
    {
        Task<string> GetTemplateHtmlStringAsync<T>(string nomeView, T modelo, dynamic custom, string environment);
    }
}