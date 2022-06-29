using System.Text.Json;
using HackathonX.DB.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HackathonX.DB.Init
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddDbContext<HackathonXContext>(options =>
               options.UseSqlite(connectionString));

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            {
                var context = scope.ServiceProvider.GetService<HackathonXContext>();
                context?.Database.Migrate();

                // Add data
                var questionnaireJson = File.ReadAllText("questionnaire.json");
                var serializeOptions = new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
                    Formatting = Formatting.Indented
                };
                var questions = JsonConvert.DeserializeObject<Question[]>(questionnaireJson, serializeOptions);
                context?.Questions.AddRange(questions);

                context?.SaveChanges();
            }
        }
    }
}
