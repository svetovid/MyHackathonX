namespace HackathonX.Service.Services
{
    public class TimeCounter
    {
        private object _lock = new object();
        private Dictionary<string, GameTimer> _counters = new Dictionary<string, GameTimer>();

        internal void SetUserCounter(string userName)
        {
            if (_counters.ContainsKey(userName))
            {
                throw new Exception($"User {userName} is already playing!");
            }

            var counter = new GameTimer();
            _counters.Add(userName, counter);
        }

        internal int GetCounter(string userName)
        {
            if (_counters.TryGetValue(userName, out GameTimer counter))
            {
                return counter.Count;
            }

            throw new Exception($"User {userName} hasn't started to play yet!");
        }

        internal void RemoveCounter(string userName)
        {
            _counters.Remove(userName);
        }

        internal void Cleanup()
        {
            lock(_lock)
            {
                foreach (var cntr in _counters.Values)
                {
                    cntr.Dispose();
                }

                _counters.Clear();
            }
        }
    }
}
