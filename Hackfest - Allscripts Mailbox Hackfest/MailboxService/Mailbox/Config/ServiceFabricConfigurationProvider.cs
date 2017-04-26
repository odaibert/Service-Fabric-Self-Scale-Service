using System.Fabric;
using Microsoft.Extensions.Configuration;

namespace Mailbox.Config
{
    // As seen at https://blogs.msdn.microsoft.com/azurecat/2017/04/06/using-a-custom-service-fabric-configuration-provider-with-asp-net-core/
    public class ServiceFabricConfigurationProvider : ConfigurationProvider
    {
        private readonly string _packageName;
        private readonly CodePackageActivationContext _context;

        public ServiceFabricConfigurationProvider(string packageName)
        {
            _packageName = packageName;
            _context = FabricRuntime.GetActivationContext();
            _context.ConfigurationPackageModifiedEvent += (sender, e) =>
            {
                this.LoadPackage(e.NewPackage, reload: true);
                this.OnReload(); // Notify the change
            };
        }

        public override void Load()
        {
            var config = _context.GetConfigurationPackageObject(_packageName);
            LoadPackage(config);
        }

        private void LoadPackage(ConfigurationPackage config, bool reload = false)
        {
            if (reload)
            {
                Data.Clear();  // Clear the old keys on reload
            }
            foreach (var section in config.Settings.Sections)
            {
                foreach (var param in section.Parameters)
                {
                    Data[$"{section.Name}:{param.Name}"] = param.Value;  //param.IsEncrypted ? param.DecryptValue().ToUnsecureString() : param.Value;
                }
            }
        }
    }
}
