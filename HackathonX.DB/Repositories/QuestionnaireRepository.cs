using HackathonX.DB.Model;
using Microsoft.EntityFrameworkCore;

namespace HackathonX.DB.Repositories
{
    public class QuestionnaireRepository : IDisposable
    {
        private readonly HackathonXContext m_DbContext;

        public QuestionnaireRepository(HackathonXContext dbContext)
        {
            m_DbContext = dbContext;
        }

        public void Dispose()
        {
            m_DbContext.Dispose();
        }

        public async Task<IEnumerable<Question>> GetQuestionnaire(int takefromEachGroup = 3)
        {
            var allQuestions = await m_DbContext.Questions
                .Include(q => q.Answers)
                .ToListAsync();

            var groupedByScore = allQuestions.GroupBy(x => x.Score)
              .Select(y =>
                  new
                  {
                      k = y.Key,
                      v = y.OrderBy(r => Guid.NewGuid()).Take(takefromEachGroup)
                  });
                
            return groupedByScore.SelectMany(x => x.v);
        }


        #region Test data
        private IEnumerable<Question> GetBuisnessLogic()
        {
            var something = GetTestData()
                .GroupBy(x => x.Score)
                .Select(y =>
                    new
                    {
                        k = y.Key,
                        v = y.OrderBy(r => Guid.NewGuid()).Take(3)
                    });
            return something.SelectMany(x => x.v);
        }

        private IList<Question> GetTestData()
        {
            return new List<Question>
            {
                new Question { Id = 1, Score = 10, Text = $"A: {Guid.NewGuid()}",
                    Answers = new List<Answer> 
                    {
                        new Answer { Id = 1, IsCorrect = true, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" },
                        new Answer { Id = 1, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" }
                    }
                },
                new Question { Id = 2, Score = 10, Text = $"B: {Guid.NewGuid()}",
                    Answers = new List<Answer>
                    {
                        new Answer { Id = 1, IsCorrect = true, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" },
                        new Answer { Id = 1, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" }
                    } },
                new Question { Id = 3, Score = 10, Text = $"C: {Guid.NewGuid()}",
                    Answers = new List<Answer>
                    {
                        new Answer { Id = 1, IsCorrect = true, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" },
                        new Answer { Id = 1, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" }
                    } },
                new Question { Id = 4, Score = 10, Text = $"D: {Guid.NewGuid()}",
                    Answers = new List<Answer>
                    {
                        new Answer { Id = 1, IsCorrect = true, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" },
                        new Answer { Id = 1, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" }
                    } },
                new Question { Id = 5, Score = 20, Text = $"A: {Guid.NewGuid()}",
                    Answers = new List<Answer>
                    {
                        new Answer { Id = 1, IsCorrect = true, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" },
                        new Answer { Id = 1, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" }
                    } },
                new Question { Id = 6, Score = 20, Text = $"B: {Guid.NewGuid()}",
                    Answers = new List<Answer>
                    {
                        new Answer { Id = 1, IsCorrect = true, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" },
                        new Answer { Id = 1, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" }
                    } },
                new Question { Id = 7, Score = 20, Text = $"C: {Guid.NewGuid()}",
                    Answers = new List<Answer>
                    {
                        new Answer { Id = 1, IsCorrect = true, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" },
                        new Answer { Id = 1, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" }
                    } },
                new Question { Id = 8, Score = 20, Text = $"D: {Guid.NewGuid()}",
                    Answers = new List<Answer>
                    {
                        new Answer { Id = 1, IsCorrect = true, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" },
                        new Answer { Id = 1, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" }
                    } },
                new Question { Id = 9, Score = 30, Text = $"A: {Guid.NewGuid()}",
                    Answers = new List<Answer>
                    {
                        new Answer { Id = 1, IsCorrect = true, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" },
                        new Answer { Id = 1, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" }
                    } },
                new Question { Id = 10, Score = 30, Text = $"B: {Guid.NewGuid()}",
                    Answers = new List<Answer>
                    {
                        new Answer { Id = 1, IsCorrect = true, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" },
                        new Answer { Id = 1, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" }
                    } },
                new Question { Id = 11, Score = 30, Text = $"C: {Guid.NewGuid()}",
                    Answers = new List<Answer>
                    {
                        new Answer { Id = 1, IsCorrect = true, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" },
                        new Answer { Id = 1, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" }
                    } },
                new Question { Id = 12, Score = 30, Text = $"D: {Guid.NewGuid()}",
                    Answers = new List<Answer>
                    {
                        new Answer { Id = 1, IsCorrect = true, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" },
                        new Answer { Id = 1, QuestionId = 1, Text = $"A: {Guid.NewGuid()}" }
                    } }
            };
        }
        #endregion
    }
}
