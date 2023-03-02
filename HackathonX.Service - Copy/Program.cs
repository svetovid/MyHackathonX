using HackathonX.DB.Model;
using HackathonX.Service.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

//builder.Services.AddDbContextFactory<HackathonXContext>(options =>
//                options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

var builder = WebApplication.CreateBuilder(args);

string AllowSpecificOrigins = "webAndHostedOrigins";

// Add services to the container.
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(AllowSpecificOrigins, corsPolicyBuilder =>
    {
        corsPolicyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
    });
});
builder.Services.AddGrpc();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
});
builder.Services.AddDbContextFactory<HackathonXContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

var app = builder.Build();

/*---------------------------*/
app.UseCors(AllowSpecificOrigins);

app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.MapGrpcService<XGameService>()
    .EnableGrpcWeb()
    .RequireCors(AllowSpecificOrigins);
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Logger.LogInformation($"Start time: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");

app.Run();
