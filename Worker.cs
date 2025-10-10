using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AudioMonitorService
{
    class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Audio Monitor Service start.");
            string start = Control.Init();
            if (!string.IsNullOrWhiteSpace(start))
            {
                _logger.LogInformation("Init: "+start);
            }
            while (!stoppingToken.IsCancellationRequested)
            {
                string res = await Control.MonitoringDeviceAsync(stoppingToken);
                if (!string.IsNullOrWhiteSpace(res)) 
                {
                    _logger.LogInformation(res);
                }
            }
            _logger.LogInformation("Audio Monitor Service stop.");
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
