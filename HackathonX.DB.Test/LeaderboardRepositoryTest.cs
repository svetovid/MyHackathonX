using HackathonX.DB.Model;
using HackathonX.DB.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HackathonX.DB.Test
{
    public class LeaderboardRepositoryTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<HackathonXContext> _contextOptions;
        private readonly HackathonXContext _hackathonXContext;

        public LeaderboardRepositoryTest()
        {
            _connection = new SqliteConnection("Data Source=HackathonX.db;");//new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _contextOptions = new DbContextOptionsBuilder<HackathonXContext>()
                .UseSqlite(_connection)
                .Options;

            _hackathonXContext = new HackathonXContext(_contextOptions);
        }

        public void Dispose()
        {
            _hackathonXContext?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }

        [Fact]
        public async Task SaveUserScore_Success()
        {
            // Arrange
            int score = 123;
            TimeSpan time = TimeSpan.FromMinutes(8);
            string userName = Guid.NewGuid().ToString("N");
            var user = await AddUserToDbHelper(_hackathonXContext, userName);

            // Act
            using var leaderboardRepository = new LeaderboardRepository(_hackathonXContext);
            await leaderboardRepository.SaveUserScore(user.Id, score, time);

            // Assert
            var leaderboard = await GetLeaderboardFromDbHelper(_hackathonXContext, user.Id);
            Assert.NotNull(leaderboard);
            Assert.Equal(score, leaderboard?.Score);
            Assert.Equal(time.Ticks, leaderboard?.Time);

            await RemoveLeaderboardFromDbHelper(_hackathonXContext, new List<Guid> { user.Id });
        }

        [Fact]
        public async Task GetLeaderboard_TakeTen_ReturnsCorrectData()
        {
            // Arrange
            List<User> users = new List<User>();

            string repetitiveName = Guid.NewGuid().ToString("N");
            var repetetiveUser = await AddUserToDbHelper(_hackathonXContext, repetitiveName);
            users.Add(repetetiveUser);

            for (int i = 0; i < 5; i++)
            {
                await AddLeadersToDbHelper(_hackathonXContext, repetetiveUser.Id, (i + 1) * 50, i + 7);

                string userName = Guid.NewGuid().ToString("N");
                var user = await AddUserToDbHelper(_hackathonXContext, userName);
                users.Add(user);
                await AddLeadersToDbHelper(_hackathonXContext, user.Id, (i + 1) * 100, i + 5);
            }

            // Act
            using var leaderboardRepository = new LeaderboardRepository(_hackathonXContext);
            IEnumerable<Leaderboard> result = await leaderboardRepository.GetLeaderboard(10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Count());
            Assert.True(result.All(x => x.User != null));
            Assert.Single(result.Where(x => x.User.Name == repetitiveName));
            Assert.Equal(500, result.First().Score);
            Assert.Equal(TimeSpan.FromMinutes(9).Ticks, result.First().Time);

            await RemoveLeaderboardFromDbHelper(_hackathonXContext, users.Select(x => x.Id).ToList());
        }

        private static async Task<User> AddUserToDbHelper(HackathonXContext dbContext, string name)
        {
            var user = dbContext.Users.Add(new User { Name = name }).Entity;
            await dbContext.SaveChangesAsync();

            return user;
        }

        private static async Task<Leaderboard?> GetLeaderboardFromDbHelper(HackathonXContext dbContext, Guid userId)
        {
            return await dbContext.Leaderboards.SingleOrDefaultAsync(x => x.UserId == userId);
        }

        private static async Task AddLeadersToDbHelper(HackathonXContext dbContext, Guid userId, int score, int time)
        {
            dbContext.Leaderboards.Add(
                new Leaderboard
                {
                    UserId = userId,
                    Score = score,
                    Time = TimeSpan.FromMinutes(time).Ticks,
                    Timestamp = DateTime.Now
                });

            await dbContext.SaveChangesAsync();
        }

        private static async Task RemoveLeaderboardFromDbHelper(HackathonXContext dbContext, List<Guid> userIds)
        {
            var leaderboards = await dbContext.Leaderboards.ToListAsync();
            dbContext.Leaderboards.RemoveRange(leaderboards.Where(x => userIds.Contains(x.UserId)));
            await dbContext.SaveChangesAsync();
        }
    }
}