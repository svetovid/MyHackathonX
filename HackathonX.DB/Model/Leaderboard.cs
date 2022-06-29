namespace HackathonX.DB.Model
{
    public partial class Leaderboard
    {
        public Guid UserId { get; set; }
        public int Score { get; set; }
        public long Time { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
