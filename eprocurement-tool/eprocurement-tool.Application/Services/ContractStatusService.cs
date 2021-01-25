using EGPS.Domain.Enums;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EGPS.Application.Services
{
    public class ContractStatusService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer timer;
        public ContractStatusService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void Dispose()
        {
            timer?.Dispose();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }
        private async void DoWork(Object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EDMSDBContext>();
                var today = DateTime.Today;
                var contracts = await context.Contracts.Where(x => today > x.SignedDate).ToListAsync();
                if (contracts.Any())
                {
                    foreach (var contract in contracts)
                    {
                        contract.Status = EContractStatus.EXPIRED;
                    }
                    context.Contracts.UpdateRange(contracts);
                    await context.SaveChangesAsync();
                }
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
