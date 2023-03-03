using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using HackathonX.WebUI.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["api"]) });

builder.Services.AddSingleton(services =>
{
    // Get the service address from appsettings.json
    var backendUrl = builder.Configuration["api"];//"https://localhost:7253";//"http://localhost:5253";//config["BackendUrl"];

    // If no address is set then fallback to the current webpage URL
    if (string.IsNullOrEmpty(backendUrl))
    {
        var navigationManager = services.GetRequiredService<NavigationManager>();
        backendUrl = navigationManager.BaseUri;
    }

    // Create a channel with a GrpcWebHandler that is addressed to the backend server.
    //
    // GrpcWebText is used because server streaming requires it. If server streaming is not used in your app
    // then GrpcWeb is recommended because it produces smaller messages.
    var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());

    return GrpcChannel.ForAddress(backendUrl, new GrpcChannelOptions { HttpHandler = httpHandler });
});

await builder.Build().RunAsync();
