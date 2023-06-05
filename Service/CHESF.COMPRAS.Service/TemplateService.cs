using System;
using System.IO;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace CHESF.COMPRAS.Service
{
    public class TemplateService: ITemplateService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITempDataProvider _tempDataProvider;

        public TemplateService(
            IRazorViewEngine razorViewEngine,
            IServiceProvider serviceProvider,
            ITempDataProvider tempDataProvider
        )
        {
            _razorViewEngine = razorViewEngine;
            _serviceProvider = serviceProvider;
            _tempDataProvider = tempDataProvider;
        }
    
        public async Task<string> GetTemplateHtmlStringAsync<T>(string nomeView, T modelo, dynamic custom, string environment)
        {
            var httpContext = new DefaultHttpContext
            {
                RequestServices = _serviceProvider,
            };

            var actionContext = new ActionContext(
                httpContext, new RouteData(),
                new ActionDescriptor()
            );

            await using var stringWriter = new StringWriter();

            var viewResult = _razorViewEngine.FindView(actionContext, nomeView, false);

            if (viewResult.View == null)
            {
                return string.Empty;
            }

            var viewDataDictionary =
                new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = new EmailModelDTO<T>
                    {
                        Model = modelo,
                        CustomData = custom,
                        Environment = environment
                    }
                };

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewDataDictionary,
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                stringWriter,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);

            return stringWriter.ToString();
        }
    }
}