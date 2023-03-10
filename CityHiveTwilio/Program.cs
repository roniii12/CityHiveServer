using CityHiveInfrastructure.Messaging;
using CityHiveInfrastructure.Extensions;
using CityHiveTwilio;
using MassTransit;
using NLog;
using NLog.Web;
using CityHiveInfrastructure.Logger;
using CityHiveInfrastructure.BackgroundTasks;

var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
try
{


    var builder = WebApplication.CreateBuilder(args);
    //SetNlogConnectionString(builder.Configuration.GetMainConnectionString());
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
    // Add services to the container.

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
    //var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
    //{
    //    cfg.ReceiveEndpoint("order-created-event", e =>
    //    {
    //        e.Consumer(typeof(SmsMessageModel), ds =>
    //        {
    //            return new object();
    //        });
    //    });
    //});

    

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseMiddleware<CityHiveTwilio.Middlewares.RequestLoggerMiddleware>();

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

