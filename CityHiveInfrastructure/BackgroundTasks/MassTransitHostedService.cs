using CityHiveInfrastructure.Logger;
using CityHiveInfrastructure.Messaging;
using MassTransit;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CityHiveInfrastructure.BackgroundTasks
{
    public class MassTransitHostedService : IHostedService
    {
        readonly IBusControl _bus;
        readonly AppGeneralLogger<MassTransitHostedService> _logger;

        public MassTransitHostedService(IBusControl bus, AppGeneralLogger<MassTransitHostedService> logger, ReceiveObserver observer)
        {
            _bus = bus;
            _logger = logger;

            _bus.ConnectReceiveObserver(observer);
            _bus.ConnectConsumeObserver(observer);
            _bus.ConnectSendObserver(observer);

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Info("Starting bus");
            IsHealthy = true;
            await _bus.StartAsync(cancellationToken).ConfigureAwait(false);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Info("Stopping bus");
            IsHealthy = false;
            return _bus.StopAsync(cancellationToken);
        }

        public static bool IsHealthy = false;


    }
}
