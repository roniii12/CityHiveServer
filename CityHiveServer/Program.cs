using CityHiveInfrastructure.BackgroundTasks;
using CityHiveInfrastructure.Messaging;
using CityHiveServer;
using NLog;
using NLog.Web;

var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);
    //SetNlogConnectionString(builder.Configuration.GetMainConnectionString());

    builder.Services.AddSingleton<ReceiveObserver>();

    builder.Services.AddHostedService<MassTransitHostedService>();
    AppHostConfiguration.ConfigureServices(builder.Services, builder.Configuration);
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
        });
    });

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Host.ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        logging.AddConsole();
        logging.AddDebug();
    }).UseNLog();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseMiddleware<CityHiveServer.Middlewares.RequestLoggerMiddleware>();

    app.UseRouting();

    app.UseAuthorization();
    app.UseCors();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    //NLog: catch setup errors
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();

}
static void SetNlogConnectionString(string connectionString)
{
    GlobalDiagnosticsContext.Set("connectionString", connectionString);
}