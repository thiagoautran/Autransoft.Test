using Autransoft.ApplicationCore.Interfaces;
using Autransoft.BackgroundService.Order.Lib.Entities;
using Autransoft.BackgroundService.Order.Lib.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TAN.Core._5._0.Skokka.Pulling.Worker.Workers
{
    public class StatusInvestWorker : BackgroundServiceOrder
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public StatusInvestWorker(IServiceScopeFactory serviceScopeFactory) => _serviceScopeFactory = serviceScopeFactory;

        protected override async Task<bool> ExecuteAsync(CancellationToken stoppingToken, IEnumerable<SharedObject> sharedObjects)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var controller = scope.ServiceProvider.GetRequiredService<IStatusInvestController>();
                await controller.SyncActionAndFIIAsync();
            }

            await Task.Delay(new TimeSpan(24, 0, 0));

            return true;
        }
    }
}