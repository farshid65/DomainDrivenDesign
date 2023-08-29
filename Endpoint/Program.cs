using Core.Domain.ClassifiedAds;
using Core.Domain.Shared;
using Endpoint;
using EventStore.ClientAPI;
using Framework;
using Infrastructure;
using Infrastructure.ClassifiedAd;
using Infrastructure.UserProfiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Data.Common;





    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())   
    .AddJsonFile("appsettings.json", false, false)
    .Build();




    const string connectionString =
                "Host=localhost;Database=Marketplace_chapter9;Username=ddd;Password=book";   
    var connStr = builder.Configuration["eventStore:connectionString"];
    var esConnection = EventStoreConnection.Create(
        connStr,
        ConnectionSettings.Create().DisableServerCertificateValidation().KeepReconnecting(),
        builder.Environment.ApplicationName);
    var store = new ESAggregate(esConnection);

    // Add services to the container.
    builder.Services.AddEntityFrameworkNpgsql();
    builder.Services.AddDbContext<MarketplacrDbContext>(options => options.UseNpgsql(connectionString));
    builder.Services.AddScoped<DbConnection>(c => new NpgsqlConnection(connectionString));
    builder.Services.AddControllers();
    builder.Services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
    builder.Services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
    builder.Services.AddScoped<EFClassifiedAdsApplicationService>();

    var purgomalumClient = new PurgomalumClient();//EventStore & Postgresql
    builder.Services.AddScoped(c =>
    new UserProfileApplicationService(
        c.GetService<IUserProfileRepository>(),
        c.GetService<IUnitofWork>(),
       text => purgomalumClient.CheckTextForProfanity(text).GetAwaiter().GetResult()));
    builder.Services.AddScoped<IUnitofWork, EfUnitofWork>();
    builder.Services.AddEndpointsApiExplorer();
    //  Event Store
    builder.Services.AddSingleton(esConnection);
    builder.Services.AddSingleton<IAggregateStore>(store);
    builder.Services.AddSingleton(new EFClassifiedAdsApplicationService(
       store,new FixedCurrencyLookup()));
    builder.Services.AddSingleton(new UserProfileApplicationService(
       store, t => purgomalumClient.CheckTextForProfanity()));
    builder.Services.AddSingleton<IHostedService,HostedService>();


    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "ClassifiedAds",
                Version = "v1"

            });


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









