using System.Diagnostics.CodeAnalysis;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Repositories;
using Full_Application_Blazor.Utils.Services;

namespace Full_Application_Blazor.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class StatupServices
	{
		public static void InjectStartupServices(IServiceCollection services)
		{
            services.AddScoped<IHangfireWrapper, HangfireWrapper>();
            services.AddScoped<ITwilioWrapper, TwilioClientWrapper>();
            services.AddScoped<ISmtpWrapper, SmtpClientWrapper>();
            services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
            services.AddScoped<IRepository<Email>, Repository<Email>>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IRepository<SMS>, Repository<SMS>>();
            services.AddScoped<IRepository<SMSTemplate>, Repository<SMSTemplate>>();
            services.AddScoped<ISMSService, SMSService>();
            services.AddScoped<IRepository<Plan>, Repository<Plan>>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<ISeed, Seed>();
            services.AddTransient<IRepository<Log>, Repository<Log>>();
            services.AddTransient<ILoggerService, LoggerService>();
        }
	}
}

