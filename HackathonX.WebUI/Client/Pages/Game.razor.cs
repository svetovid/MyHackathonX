using Grpc.Core;
using Grpc.Net.Client;
using HackathonX.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace HackathonX.WebUI.Client.Pages
{
    public partial class Game : IDisposable
    {
        [Inject]
        protected GrpcChannel Channel { get; set; }
        [Inject]
        protected NavigationManager NavManager { get; set; }

        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        private XGame.XGameClient Client;
        private Service.Counter.CounterClient Client2;

        private static User? _user;
        private static IEnumerable<Question>? _questionnaire;

        private int _timeCounter = 0;
        private int _score;
        private Question? _currentQuestion;
        private string _bonusWord = string.Empty;

        private bool QuestFinished => _questionnaire != null && _questionnaire.All(x => x.Answered);

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Client = new XGame.XGameClient(Channel);
                Client2 = new Service.Counter.CounterClient(Channel);
                var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
                if (!QueryHelpers.ParseQuery(uri.Query).TryGetValue("user", out var userName))
                {
                    NavManager.NavigateTo("/");
                }

                _user = await Client.GetOrCreateUserAsync(new User { Name = userName });

                var questionnaire = await Client.GetQuestionnaireAsync(new RecordsToReturn { Records = 3 });
                _questionnaire = questionnaire.Questions.Select(x => x);
                _bonusWord = GetBonusWord();

                using var call = Client2.SetTimer(new UserRequest { Name = _user.Name }, cancellationToken: cts.Token);
                await foreach (var response in call.ResponseStream.ReadAllAsync())
                {
                    _timeCounter = response.Count;

                    StateHasChanged();
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                Console.WriteLine("Operation Cancelled.");
            }
        }

        private async Task AnswerTheQuestion(Question? question)
        {
            if (question == null)
            {
                return;
            }

            if (question.Useranswerid == question.Answers.Single(x => x.Iscorrect).Id)
            {
                _score += question.Score;
                question.Iscorrect = true;
            }

            question.Answered = true;

            if (QuestFinished)
              cts.Cancel();

            StateHasChanged();
        }

        private void SelectPuzzle(int? questionId)
        {
            var question = _questionnaire?.SingleOrDefault(x => x.Id == questionId && !x.Answered);
            if (question != null)
            {
                _currentQuestion = question;

                StateHasChanged();
            }
        }

        private void BonusQuestionAnswered(string answer)
        {
            if (_bonusWord.Equals(answer, StringComparison.OrdinalIgnoreCase))
                _score += 50;

            StateHasChanged();
        }

        private string GetBonusWord()
        {
            var words = new[] { "mechanics", "recognize", "automated", "technical", "visualize", "dashboard", "worldwide", "execution", "landscape", "arguments", "interface", "construct", "workgroup", "bilateral", "analyzers", "lifecycle", "indicator", "generator", "functions", "equipment", "inventory", "real-time", "customize", "consumers", "detection", "warehouse", "resources", "computing", "optimized", "component", "dimension", "framework", "analytics", "extension", "invention", "computers", "diffusion", "processor", "high-tech", "education", "implement", "addressee", "bandwidth", "protocols", "ecosystem", "evolution", "actuators", "platforms", "logistics", "connected" };
            var y = new Random().Next(50);
            return words[y - 1];
        }

        private async Task Finish()
        {
            if (QuestFinished)
            {
              await Client.SaveUserScoreAsync(new UserScore { User = _user, Score = _score, Time = TimeSpan.FromSeconds(_timeCounter).Ticks });
              NavManager.NavigateTo("/results");
            }
        }

        public void Dispose()
        {
            // Add clean-up
        }
    }
}