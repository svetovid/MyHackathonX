namespace HackathonX.Service.Services
{
    public class TimerService : IHostedService
    {
        private readonly TimeCounter m_TagService;

        public TimerService(TimeCounter tagService)
        {
            m_TagService = tagService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            m_TagService.Cleanup();
            return Task.CompletedTask;
        }
    }
}
