using Microsoft.Extensions.Configuration;

namespace Mailbox.Config
{
    public class ServiceFabricConfigSource : IConfigurationSource
    {
        public string PackageName { get; set; }

        public ServiceFabricConfigSource(string packageName)
        {
            PackageName = packageName;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ServiceFabricConfigurationProvider(PackageName);
        }
    }
}