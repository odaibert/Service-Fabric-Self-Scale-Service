# Service-Fabric-Self-Scale-Service
Service Fabric use the approach to create new instances of service type. Using this approach, this project create a new instance of a service type based on some metrics inside of the running service. 

This first version uses a simple way to create a new instance of service type based on the number of calls to the running service. 

Project 
SaaSManagerService
  SaaSManager2
    - This is a Stateless service who create a new instance of an another ServiceType 
    
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


Nexts steps: 
Today the running service needs to invoke the SaaSManager2 service to ask it to create a new instance. The main go is to use the metrics generated to create a new instance with no coding on the running service. Implement Service Fabric metrics on the running service to change the way the service will scale.

private const int ServiceRequestStartEventId = 99;
        [Event(ServiceRequestStartEventId, Level = EventLevel.Informational, Message = "Service request '{0}' has '{1}' requests and will create another instance", Keywords = Keywords.Requests)]
        public void ServiceRequestCount(string requestTypeName)
        {
            WriteEvent(ServiceRequestCountEventId, requestTypeName);
        }



