using Contracts;
using Marketplace;
using Marketplace.Domain.ClassifiedAds;
using Marketplace.Domain.Shared;
using Marketplace.Framework;
using Marketplace.Infrastructure;
using Marketplace.Infrastructure.ClassifiedAd;
using Marketplace.Infrastructure.UserProfiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Raven.Client.Documents;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var store = new DocumentStore
        {
            Urls = new[] { "http://localhost:8080" },
            Database = "Marketplace_Chapter8",
            Conventions =
            {
                FindIdentityProperty=x=>x.Name=="DbId"
            }
        };
        store.Initialize();

        // Add services to the container.
        var purgomalumClient = new PurgomalumClient();
        builder.Services.AddSingleton<ICurrencyLookup,FixedCurrencyLookup>();
        builder.Services.AddScoped(c=>store.OpenAsyncSession());
        builder.Services.AddScoped<IUnitofWork, RavenDbUnitofWork>();        
        builder.Services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
        builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        builder.Services.AddScoped<Contracts.ClassifiedAdsApplicationService>();
        builder.Services.AddScoped(c =>
            new UserProfileApplicationService(
                c.GetService<IUserProfileRepository>(),
                c.GetService<IUnitofWork>(),
                text => purgomalumClient.CheckTextForProfanity(text).GetAwaiter().GetResult()));
            
        builder.Services.AddControllers();
        builder.Services.AddSingleton(new Marketplace.ClassifiedAdsApplicationService());
        //builder.Services.AddScoped<IEntityStore, RavenDbEntityStore>();
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ClassifiedAds", Version = "v1" });
        });

        var app = builder.Build();
        

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            c.SwaggerEndpoint("swagger/v1/swagger.json", "ClassifiedAds v1"));
        }
        
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();



        app.Run();
    }
}