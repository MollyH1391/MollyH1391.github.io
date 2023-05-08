using Microsoft.AspNetCore.Builder;

namespace WowDin.Backstage.Common
{
    public static class MyMiddlewareExtensions
    {

        public static IApplicationBuilder UseMyExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
