using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text;
using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Full_Application_Blazor.API.Middleware;
using Full_Application_Blazor.DependencyInjection;
using Full_Application_Blazor.Utils.Configuration;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Prometheus.Client.Collectors;
using Prometheus.Client.DependencyInjection;
using Prometheus.Client.MetricPusher;
using GraphQL;
using GraphiQl;

var builder = WebApplication.CreateBuilder(args);

var metricsConfig = builder.Configuration.GetSection("MetricsConfig").Get<MetricsConfig>();
var databaseConfig = builder.Configuration.GetSection("DatabaseConfig").Get<DatabaseConfig>();
var seedConfig = builder.Configuration.GetSection("SeedConfig").Get<SeedConfig>();
var jwtConfig = builder.Configuration.GetSection("JWTConfig").Get<JWTConfig>();

builder.Services.Configure<Config>(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
    xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));
});

builder.Services.AddMetricFactory();

builder.Services.AddSingleton<IMongoClient, MongoClient>();
builder.Services.AddSingleton<IRepository<RequestLog>, Repository<RequestLog>>();

builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseMongoStorage(new MongoClient(databaseConfig.ConnectionString), databaseConfig.DatabaseName, new MongoStorageOptions
        {
            MigrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy()
            },
            Prefix = "hangfire",
            CheckConnection = true,
            InvisibilityTimeout = TimeSpan.FromMinutes(30),
            CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
        })
    );

builder.Services.AddHangfireServer(serverOptions =>
{
    serverOptions.ServerName = "Hangfire.Mongo server";
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        RequireExpirationTime = true
    };
});

builder.Services.AddAuthorization();

var mongoDbIdentityConfiguration = new MongoDbIdentityConfiguration
{
    MongoDbSettings = new MongoDbSettings
    {
        ConnectionString = databaseConfig.ConnectionString,
        DatabaseName = databaseConfig.DatabaseName
    },
    IdentityOptionsAction = options =>
    {
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
    }
};

builder.Services.ConfigureMongoDbIdentity<User, Role, string>(mongoDbIdentityConfiguration)
        .AddDefaultTokenProviders();

InjectServices.InjectAllServices(builder.Services);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
    app.UseGraphiQl("/graphql");
}
app.UseRouting();
app.UseGraphQL<GraphQL.Types.ISchema>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<MetricsLogger>();
app.UseMiddleware<RequestLogger>();

var registry = app.Services.GetService<ICollectorRegistry>();
var worker = new MetricPushServer(
    new MetricPusher(
        new MetricPusherOptions
        {
            CollectorRegistry = registry,
            Endpoint = metricsConfig.Endpoint,
            Job = metricsConfig.Name,
            AdditionalHeaders = new Dictionary<string, string> { { "Authorization", "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(metricsConfig.Username + ":" + metricsConfig.Password)) } }
        }));

worker.Start();

using (var scope = app.Services.CreateScope())
{
    var seed = scope.ServiceProvider.GetRequiredService<ISeed>();
    seed.SeedAll(databaseConfig, seedConfig);
}

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program { }