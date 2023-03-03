namespace HackathonX.Service.Services
{
    public class TimerService : IHostedService
    {
        private readonly TimeCounter m_TimeCounter;

        public TimerService(TimeCounter tagService)
        {
            m_TimeCounter = tagService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            m_TimeCounter.Cleanup();
            return Task.CompletedTask;
        }
    }
}
