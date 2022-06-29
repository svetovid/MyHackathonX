namespace HackathonX.DB.Model
{
    public partial class User
    {
        public User()
        {
            Leaderboards = new HashSet<Leaderboard>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Leaderboard> Leaderboards { get; set; }
    }
}
