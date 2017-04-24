using System;
using System.Collections.Generic;
using System.Fabric.Health;
using System.Fabric.Query;
using System.IO;
using System.Net;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Fabric;
using System.Threading.Tasks;
using System.Fabric.Description;
using System.Text;
using Microsoft.ServiceFabric;
using ServiceFabricServiceModel;

namespace ServicesManager.Controllers
{
    [Route("api/ServiceManager")]
    public class ServiceManagerController : ApiController
    {
        private readonly StatelessServiceContext context;

        public ServiceManagerController()
        {
          
        }

        public ServiceManagerController(FabricClient fabricClient, StatelessServiceContext context)
        {
            this.context = context;
        }

        public IEnumerable<string> Get()
        {
            CreateService("fabric:/MailboxService"); //this.context.CodePackageActivationContext.ApplicationName;

            yield return "200";
        }
        private static FabricClient _fabricClient =new FabricClient();

        private Task CreateService(string appName)
        {
            StatelessServiceDescription serviceDescription = new StatelessServiceDescription()
            {
                ApplicationName = new Uri(appName),
                PartitionSchemeDescription = new SingletonPartitionSchemeDescription(),
                InstanceCount = 1,
                ServiceTypeName = "MailboxType", // Just an example
                ServiceName = new Uri($"{appName}/Mailbox" + new Random(7).Next(100).ToString() ) // Improve the method to generate the service name. 
            };
            return _fabricClient.ServiceManager.CreateServiceAsync(serviceDescription);
        }

        public string Get(int instances)
        {
            var fc = new FabricClient();
            string appName = this.context.CodePackageActivationContext.ApplicationName;

            StatelessServiceDescription serviceDescription = new StatelessServiceDescription()
            {
                ApplicationName = new Uri(appName),
                PartitionSchemeDescription = new SingletonPartitionSchemeDescription(),
                InstanceCount = 1,
                ServiceTypeName = "MailboxType",
                ServiceName = new Uri($"{appName}/test1")
            };
            return fc.ServiceManager.CreateServiceAsync(serviceDescription).ToString();
        }

        // POST api/values 
        public void Post([FromBody]string value)
        {
        }

        // PUT api/ServiceManager/{serviceName}
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/ServiceManager/delete/{serviceName} 
        public void Delete(int id)
        {
        }

    }

    /// <summary>  
    /// Designed for use with JavaScriptSerializer.  
    /// </summary>  
    public class ServiceDefinition
    {
        public string id { get; set; }
        public ServiceKind ServiceKind { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string ManifestVersion { get; set; }
        public HealthState HealthState { get; set; }
        public ServiceStatus ServiceStatus { get; set; }
        public bool IsServiceGroup { get; set; }
        public bool HasPersistedState { get; set; }
    }


}
