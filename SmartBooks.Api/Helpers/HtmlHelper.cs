using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace SmartBooks.Api.Helpers
{
    public class HtmlHelper
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITempDataProvider _tempDataProvider;
        
        public HtmlHelper(
            IRazorViewEngine engine,
            IServiceProvider serviceProvider,
            ITempDataProvider tempDataProvider)
        {
            _razorViewEngine = engine;
            _serviceProvider = serviceProvider;
            _tempDataProvider = tempDataProvider;
        }
        public async Task<string>
            GetTemplateHtmlAsStringAsync<T>(string viewName, T model)
        {
            var httpContext = new DefaultHttpContext()
            {
                RequestServices = _serviceProvider
            };
            var actionContext = new ActionContext(
                    httpContext, new RouteData(), new ActionDescriptor());

            using StringWriter sw = new StringWriter();

            var viewResult = _razorViewEngine.GetView(viewName, viewName, false);

            if (viewResult.View == null)
                return string.Empty;

            var viewDataDictionary = new ViewDataDictionary(
                    new EmptyModelMetadataProvider(),
                    new ModelStateDictionary()
                )
            {
                Model = model
            };

            var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDataDictionary,
                    new TempDataDictionary(
                            actionContext.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

            await viewResult.View.RenderAsync(viewContext);

            return sw.ToString();
        }
    }
}
