using Microsoft.Extensions.Configuration;

namespace Mailbox.Config
{
    public static class ServiceFabricConfigExtensions
    {
        public static IConfigurationBuilder AddServiceFabricConfig(this IConfigurationBuilder builder, string packageName)
        {
            return builder.Add(new ServiceFabricConfigSource(packageName));
        }
    }
}