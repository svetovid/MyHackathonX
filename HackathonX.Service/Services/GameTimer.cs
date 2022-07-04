using System.Timers;

namespace HackathonX.Service
{
    internal class GameTimer : IDisposable
    {
        private readonly System.Timers.Timer m_Timer;

        internal int Count { get; private set; }

        internal GameTimer()
        {
            m_Timer = new System.Timers.Timer(1000);
            m_Timer.Elapsed += Timer_Elapsed;
            m_Timer.AutoReset = true;
            m_Timer.Enabled = true;
        }

        ~GameTimer()
        {
            Dispose(false);
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Count++;
        }

        private void ReleaseUnmanagedResources()
        {
             m_Timer.Elapsed -= Timer_Elapsed;
        }

        private void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                m_Timer?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}