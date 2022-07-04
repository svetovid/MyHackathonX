using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using HackathonX.Service;
using Microsoft.AspNetCore.Components;

namespace HackathonX.WebUI.Client.Pages
{
    public partial class Results : IDisposable
    {
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        [Inject]
        protected GrpcChannel Channel { get; set; }
        [Inject]
        protected NavigationManager NavManager { get; set; }

        private XGame.XGameClient Client;
        private IEnumerable<UserScore> _leaderboards =
        new List<UserScore>
        {
            new UserScore
            {
                Rank = 1, User = new User { Name ="Alfred Hitchckock" }, Score = 500, Time = 2390000000
            },
            new UserScore
            {
                Rank = 2, User = new User { Name ="James Cameron" }, Score = 400, Time = 1880000000
            },
            new UserScore
            {
                Rank = 3, User = new User { Name ="George Lucas" }, Score = 460, Time = 2500000000
            },
            new UserScore
            {
                Rank = 4, User = new User { Name ="Steven Spielberg" }, Score = 400, Time = 1490000000
            },
            new UserScore
            {
                Rank = 5, User = new User { Name ="Martin Scorsese" }, Score = 360, Time = 2400000000
            },
            new UserScore
            {
                Rank = 6, User = new User { Name ="Guy Ritchie" }, Score = 300, Time = 2090000000
            }
        };

        protected override async Task OnInitializedAsync()
        {
            //Client = new XGame.XGameClient(Channel);
            var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());
            var channel = GrpcChannel.ForAddress("http://localhost:5292", new GrpcChannelOptions { HttpHandler = httpHandler });
            Client = new XGame.XGameClient(channel);
            //var client = new TagHubService.TagHub.TagHubClient(channel);

            var leaderboards = await Client.GetLeaderboardAsync(new RecordsToReturn { Records = 10 });
            //_leaderboards = leaderboards.Points.Select(x => x);
            //var tagResponse = await client.GetScreenTagsAsync(new TagHubService.ScreenTagsRequest { Name = "screen1" }, cancellationToken: cts.Token);

        }

        public void Dispose()
        {
            // Add clean-up
        }
    }
}