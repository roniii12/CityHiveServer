using CityHiveInfrastructure.Exceptions;
using CityHiveInfrastructure.Logger;

namespace CityHiveServer.Middlewares
{
    public class RequestLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAppLogger<RequestLoggerMiddleware> logger;

        public RequestLoggerMiddleware(AppGeneralLogger<RequestLoggerMiddleware> logger, RequestDelegate next)
        {
            _next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            catch (Exception ex)
            {
                logger.Error(new ManagedException(ex, $"General error: {ex.Message}", AppModule.GENERAL_HANDLER, AppLayer.WEB_API));
                throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
            }

        }
    }
}
