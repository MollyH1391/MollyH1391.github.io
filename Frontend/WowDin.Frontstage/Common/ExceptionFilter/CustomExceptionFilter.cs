using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace WowDin.Frontstage.Common
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger, IModelMetadataProvider modelMetadataProvider)
        {
            _logger = logger;
            _modelMetadataProvider = modelMetadataProvider;
        }

        public void OnException(ExceptionContext context)
        {
            var errorMessage = $"{DateTime.Now.ToLongTimeString()} | {context.Exception}";
            _logger.LogError(errorMessage);

            var result = new ViewResult { ViewName = "Views/ErrorHandler/ErrorMessage.cshtml" };
            result.ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState);

            result.ViewData.Add("Exception", "系統忙碌中，請稍後再試");

        }
    }

}
