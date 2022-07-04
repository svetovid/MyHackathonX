using Grpc.Net.Client;
using HackathonX.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace HackathonX.WebUI.Client.Pages
{
    public partial class Index : IDisposable
    {
        [Inject]
        protected GrpcChannel Channel { get; set; }
        [Inject]
        protected NavigationManager NavManager { get; set; }

        private XGame.XGameClient Client;
        private User _user = new User();

        protected override void OnInitialized()
        {
            Client = new XGame.XGameClient(Channel);

            base.OnInitialized();
        }

        private async Task Submit()
        {
            try
            {
                var tagResponse = await Client.GetOrCreateUserAsync(/*_user*/new User { Id = "867d8309-c22e-4750-9128-c2e30d578ea5", Name = "John Doe" });
                
                var url = QueryHelpers.AddQueryString(NavManager.ToAbsoluteUri("/game").AbsolutePath, new Dictionary<string, string?> { { "user", _user.Name } });
                NavManager.NavigateTo(url);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public void Dispose()
        {
            // Add clean-up
        }
    }
}