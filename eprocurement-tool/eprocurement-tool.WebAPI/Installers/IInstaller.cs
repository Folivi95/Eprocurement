using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EGPS.WebAPI.Installers
{
    public interface IInstaller
    {
        void InstallerServices(IServiceCollection services, IConfiguration configuration);
    }
}
