using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EGPS.WebAPI.Installers
{
    public class DbInstaller : IInstaller
    {
        public static readonly ILoggerFactory EDMSLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public void InstallerServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EDMSDBContext>(options =>
                options
                .UseLoggerFactory(EDMSLoggerFactory)
                .UseSqlServer(
                    configuration["ET_DB_CONNECTION"]));
        }
    }
}
