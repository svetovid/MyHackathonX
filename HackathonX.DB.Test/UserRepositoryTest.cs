using HackathonX.DB.Model;
using HackathonX.DB.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HackathonX.DB.Test
{
    public class UserRepositoryTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<HackathonXContext> _contextOptions;
        private readonly HackathonXContext _hackathonXContext;

        public UserRepositoryTest()
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
        public async Task GetOrAddUser_NewName_SavesToDb()
        {
            // Arrange
            string name = Guid.NewGuid().ToString("N");

            var userBefore = await GetUserFromDbHelper(_hackathonXContext, name);
            Assert.Null(userBefore);

            // Act
            using var userRepository = new UserRepository(_hackathonXContext);
            User result = await userRepository.GetOrAddUser(name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
            
            var userAfter = await GetUserFromDbHelper(_hackathonXContext, name);
            Assert.NotNull(userAfter);
        }

        [Fact]
        public async Task GetOrAddUser_ExistingName_GetsFromDb()
        {
            // Arrange
            string name = Guid.NewGuid().ToString("N");

            await AddUserToDbHelper(_hackathonXContext, name);

            // Act
            using var userRepository = new UserRepository(_hackathonXContext);
            User result = await userRepository.GetOrAddUser(name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
        }

        private static async Task<User?> GetUserFromDbHelper(HackathonXContext dbContext, string name)
        {
            return await dbContext.Users.SingleOrDefaultAsync(u => u.Name == name);
        }

        private static async Task AddUserToDbHelper(HackathonXContext dbContext, string name)
        {
            dbContext.Users.Add(new User { Name = name });
            await dbContext.SaveChangesAsync();
        }
    }
}