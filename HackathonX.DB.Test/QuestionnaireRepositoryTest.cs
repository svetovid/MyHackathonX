using HackathonX.DB.Model;
using HackathonX.DB.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace HackathonX.DB.Test
{
    public class QuestionnaireRepositoryTest : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<HackathonXContext> _contextOptions;
        private readonly HackathonXContext _hackathonXContext;

        public QuestionnaireRepositoryTest(ITestOutputHelper output)
        {
            _output = output;

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
        public async Task GetQuestionnaire_TakeThreeForEachGroup_ReturnsCorrrectdata()
        {
            // Arrange

            // Act
            using var questionnaireRepository = new QuestionnaireRepository(_hackathonXContext);
            IEnumerable<Question> result = await questionnaireRepository.GetQuestionnaire(3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(9, result.Count());
            Assert.Equal(3, result.Where(x => x.Score == 10).Count());
            Assert.Equal(3, result.Where(x => x.Score == 20).Count());
            Assert.Equal(3, result.Where(x => x.Score == 30).Count());

            foreach (Question question in result)
            {
                _output.WriteLine(question.Text);
                Assert.NotEmpty(question.Answers);
                Assert.Single(question.Answers, x => x.IsCorrect);
            }
        }
    }
}