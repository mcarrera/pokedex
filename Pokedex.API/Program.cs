using PokeApiNet;
using Pokedex.API.Handlers;
using Pokedex.API.Services;
using System.Reflection;

namespace Pokedex.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add health checks
            builder.Services.AddHealthChecks();


            // inject items
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            
            // adding the client as scoped, for perfomance and to avoid any unknown side effects using singleton
            builder.Services.AddScoped<PokeApiClient>();

            builder.Services.AddScoped<IPokeApiClientWrapper, PokeApiClientWrapper>();

            builder.Services.AddHttpClient<IFunTranslationService, FunTranslationService>(client =>
            {
                // in a production environment, I would get the base address from a configuration file
                // or perhaps an environment variable if there are different environments (DEV, QA, ..., PROD)
                client.BaseAddress = new Uri("https://api.funtranslations.com/");
            });

            var app = builder.Build();

            app.MapHealthChecks("/health");


            // Configure the HTTP request pipeline.


            // normally, for security reasons, swagger would be shown only if the environment is development.
            // I removed the if condition so the swagger UI can be used on the azure deployment, which is considered a production slot
            app.UseSwagger();
            app.UseSwaggerUI();


            app.UseHttpsRedirection();
           

            app.UseRouting();
            app.MapControllers();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //    endpoints.MapHealthChecks("/health");
            //});

            app.Run();
        }
    }
}
