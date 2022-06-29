using HackathonX.DB.Model;
using HackathonX.DB.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HackathonX.Web.Data
{
    public class XService : IDisposable
    {
        private readonly IDbContextFactory<HackathonXContext> _dbFactory;
        //private UserRepository _userRepository;
        //private QuestionnaireRepository _questionnaireRepository;
        //private LeaderboardRepository _leaderboardRepository;

        //private UserRepository UserRepository
        //{
        //    get
        //    {
        //            if (_userRepository == null)
        //            {
        //                _userRepository = new UserRepository(_dbContext);
        //            }
        //            return _userRepository;
        //    }
        //}

        //private QuestionnaireRepository QuestionnaireRepository
        //{
        //    get
        //    {
        //        lock (_lock)
        //        {
        //            if (_questionnaireRepository == null)
        //            {
        //                _questionnaireRepository = new QuestionnaireRepository(_dbContext);
        //            }
        //            return _questionnaireRepository;
        //        }
        //    }
        //}

        //private LeaderboardRepository LeaderboardRepository
        //{
        //    get
        //    {
        //        lock (_lock)
        //        {
        //            if (_leaderboardRepository == null)
        //            {
        //                _leaderboardRepository = new LeaderboardRepository(_dbContext);
        //            }
        //            return _leaderboardRepository;
        //        }
        //    }
        //}


        private static User _user;
        private static IEnumerable<Question> _questionnaire;

        public XService(IDbContextFactory<HackathonXContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<User> GetOrCreateUser(string name)
        {
            if (_user == null)
            {
                var context = _dbFactory.CreateDbContext();
                using var repo = new UserRepository(context);
                var user = await repo.GetOrAddUser(name);
                _user = new User { Id = user.Id, Name = user.Name };
            }

            return _user;
        }

        public async Task<IEnumerable<Question>> GetQuestionnaire()
        {
            if (_questionnaire == null)
            {
                var context = _dbFactory.CreateDbContext();
                using var repo = new QuestionnaireRepository(context);
                var questions = await repo.GetQuestionnaire();

                _questionnaire = questions.Select(x => new Question
                {
                    Id = x.Id,
                    Text = x.Text,
                    Score = x.Score,
                    Answers = x.Answers.Select(y => new Answer { Id = y.Id, Text = y.Text, IsCorrect = y.IsCorrect, QuestionId = y.QuestionId })
                });
            }

            return _questionnaire;

            //var questions = await QuestionnaireRepository.GetQuestionnaire();

            //return questions.Select(x => new Question
            //{
            //    Id = x.Id,
            //    Text = x.Text,
            //    Score = x.Score,
            //    Answers = x.Answers.Select(y => new Answer { Id = y.Id, Text = y.Text, IsCorrect = y.IsCorrect, QuestionId = y.QuestionId })
            //});
        }

        public async Task SaveUserScore(Guid userId, int score, TimeSpan timeSpent)
        {
            var context = _dbFactory.CreateDbContext();
            using var repo = new LeaderboardRepository(context);
            await repo.SaveUserScore(userId, score, timeSpent);
        }

        public async Task<IEnumerable<Leaderboard>> GetLeaderboards()
        {
            var context = _dbFactory.CreateDbContext();
            using var repo = new LeaderboardRepository(context);
            var leaderboards = await repo.GetLeaderboard();

            var i = 0;
            var result = new List<Leaderboard>();
            //foreach (var item in leaderboards)
            //{
            //    i++; 
            //    result.Add(new Leaderboard { Rank = 1, Score = item.Score, UserName = item.User.Name, Time = item.Time });
            //}

            return result;
        }

    public void Dispose()
    {
        //_userRepository?.Dispose();
        //_questionnaireRepository?.Dispose();
        //_leaderboardRepository?.Dispose();
    }

    //public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
    //{
    //    return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //    {
    //        Date = startDate.AddDays(index),
    //        TemperatureC = Random.Shared.Next(-20, 55),
    //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //    }).ToArray());
    //}
  }
}