using System;
using System.Linq;
using System.Threading.Tasks;
using CHESF.COMPRAS.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace CHESF.COMPRAS.API.Config.Security
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class DispositivoUidAttribute : Attribute, IAsyncActionFilter
    {
        private const string dispositivoUidHeader = "X-DISPOSITIVO-UID";
        private readonly IDispositivoRepository _dispositivoRepository;
        
        public DispositivoUidAttribute(IDispositivoRepository dispositivoRepository)
        {
            _dispositivoRepository = dispositivoRepository;
        }
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(dispositivoUidHeader, out var uidValues))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "UID do dispositivo não encontrado"
                };
                return;
            }

            var uid = uidValues.ToString();
            
            var dispositivo = await (from dispositivoBanco in await _dispositivoRepository.GetAll()
                where dispositivoBanco.UidFirebaseInstallation == uid
                select dispositivoBanco).FirstOrDefaultAsync();

            if (dispositivo == null)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Não foi encontrado nenhum dispositivo com o UID informado"
                };
                return;
            }

            context.HttpContext.Items["Dispositivo"] = dispositivo;

            await next();
        }
    }
}