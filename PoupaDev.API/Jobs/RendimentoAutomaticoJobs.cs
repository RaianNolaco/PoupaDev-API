using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PoupaDev.API.Persistence;

namespace PoupaDev.API.Jobs
{
    public class RendimentoAutomaticoJobs : IHostedService
    {
        private Timer _timer;
        public IServiceProvider ServiceProvider { get; set; }

        public RendimentoAutomaticoJobs(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        

        public void RendarSaldo(object? state){
            using (var scope = ServiceProvider.CreateScope()){
                var context = scope
                .ServiceProvider
                .GetRequiredService<PoupaDevContext>();

                var objetivos = context
                .Objetivos
                .Include(o => o.Operacoes);

                foreach (var objetivo in objetivos)
                {
                    objetivo.Render();
                }
                context.SaveChanges();

            }
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(RendarSaldo, null, 0,10000);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}