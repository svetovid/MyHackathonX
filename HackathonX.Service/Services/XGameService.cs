using Grpc.Core;
using HackathonX.DB.Model;
using HackathonX.DB.Repositories;

namespace HackathonX.Service.Services
{
    public class XGameService : XGame.XGameBase
    {
        private readonly ILogger<XGameService> _logger;
        private readonly HackathonXContext _dbContext;

        public XGameService(HackathonXContext dbContext, ILogger<XGameService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public override async Task<User> GetOrCreateUser(User request, ServerCallContext context)
        {
            try
            {
                //var context = _dbFactory.CreateDbContext();
                using var repo = new UserRepository(_dbContext);
                var user = await repo.GetOrAddUser(request.Name);
                return new User { Id = user.Id.ToString(), Name = user.Name };
            }
            catch (Exception ex)
            {
                _logger.LogError("GetOrCreateUser failed", ex);
                throw new Exception("GetOrCreateUser failed");
            }
        }

        public override async Task<QuestionnareReply> GetQuestionnaire(RecordsToReturn request, ServerCallContext context)
        {
            try
            {
                //var context = _dbFactory.CreateDbContext();
                using var repo = new QuestionnaireRepository(_dbContext);
                var questions = await repo.GetQuestionnaire();

                var reply = new QuestionnareReply();
                foreach (var qdb in questions)
                {
                    var q = new Question
                    {
                        Id = qdb.Id,
                        Text = qdb.Text,
                        Score = qdb.Score
                    };
                    q.Answers.AddRange(qdb.Answers.Select(y =>
                        new Answer
                        {
                            Id = y.Id,
                            Text = y.Text,
                            Iscorrect = y.IsCorrect,
                            Questionid = y.QuestionId
                        }));
                    reply.Questions.Add(q);
                }

                return reply;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetQuestionnaire failed", ex);
                throw new Exception("GetQuestionnaire failed");
            }
        }

        public override async Task<SaveResult> SaveUserScore(UserScore request, ServerCallContext context)
        {
            try
            {
                //var context = _dbFactory.CreateDbContext();
                using var repo = new LeaderboardRepository(_dbContext);
                await repo.SaveUserScore(Guid.Parse(request.User.Id), request.Score, request.Time);
                return new SaveResult { Success = true };
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveUserScore failed", ex);
                throw new Exception("SaveUserScore failed");
            }
        }

        public override async Task<LeaderboardReply> GetLeaderboard(RecordsToReturn request, ServerCallContext context)
        {
            try
            {
                //var context = _dbFactory.CreateDbContext();
                using var repo = new LeaderboardRepository(_dbContext);
                var leaderboards = await repo.GetLeaderboard();

                var i = 0;
                var result = new LeaderboardReply();
                foreach (var item in leaderboards)
                {
                    i++;
                    result.Points.Add(
                        new UserScore
                        {
                            Rank = 1,
                            Score = item.Score,
                            User = new HackathonX.Service.User { Id = item.User.Id.ToString(), Name = item.User.Name },
                            Time = item.Time
                        });
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetLeaderboard failed", ex);
                throw new Exception("GetLeaderboard failed");
            }
        }
    }
}
