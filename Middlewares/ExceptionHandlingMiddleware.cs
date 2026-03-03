namespace TodoBack.Middlewares {

    public class ExceptionHandlingMiddleware {

        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {


            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsync(ex.Message);
            }
            
        }

    }
}
