using Microsoft.AspNetCore.Diagnostics;
using TodoBack.Middlewares;

namespace TodoBack.Configurations {
    public static class MiddlewareConfiguration {

        public static WebApplication AddMiddleware(this WebApplication app) {

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();


            return app;
        }

    }
}
