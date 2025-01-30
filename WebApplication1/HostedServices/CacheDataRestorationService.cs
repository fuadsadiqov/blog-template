using GP.Infrastructure.Services;

namespace GP.API.HostedServices
{
    public class CacheDataRestorationService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public CacheDataRestorationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var translationService = scope.ServiceProvider.GetRequiredService<TranslationService>();
            await translationService.SetCacheAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
