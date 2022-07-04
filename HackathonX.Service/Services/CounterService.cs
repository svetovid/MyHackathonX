using Grpc.Core;

namespace HackathonX.Service.Services
{
    public class CounterService : Counter.CounterBase
    {
        private readonly TimeCounter m_TimeCounter;

        public CounterService(TimeCounter timeCounter)
        {
            m_TimeCounter = timeCounter;
        }

        public override async Task SetTimer(UserRequest request, IServerStreamWriter<Timer> responseStream, ServerCallContext context)
        {
            try
            {
                m_TimeCounter.SetUserCounter(request.Name);

                while (!context.CancellationToken.IsCancellationRequested)
                {
                    await responseStream.WriteAsync(new Timer { Count = m_TimeCounter.GetCounter(request.Name) });
                    await Task.Delay(1000, context.CancellationToken);
                }
            }
            catch (TaskCanceledException ex)
            {
                // Do nothing
            }
            catch (Exception ex)
            {
                // log error
            }

            m_TimeCounter.RemoveCounter(request.Name);
        }
    }
}
