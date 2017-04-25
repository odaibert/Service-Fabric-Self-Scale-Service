# Service-Fabric-Self-Scale-Service (SaaSManager2)

This first version uses a simple way to create a new instance of one serviceType available on the Service Fabric ImageStore, based on the number of calls to the running service. 

# PROBLEM STATEMENT 
1.How to increase the computational density of a node?

In a hyper scale service scenario, where a single service can receive millions of requests, a service must be able to self-scale. This is necessary because of external constraints to the service itself, such as a synchronous access time to a database can make your service not use a lot of CPU or memory, but have a delay to respond to requests. So, it is necessary to have one or N new instances of the same service running in a few seconds to be able to answer all requests, and when the number of requests decrease delete them.  

In this case, the trigger to choose when a new instance will be created is not necessary the CPU or memory usage, but is more oriented to requests, transactions, even service response time, or any other custom application metric an application / service have. 

2.Why not choose to deploy new instances of the service using the common deployment usage, like create a new node / machine and deploy the service there? 

This approach work fine for services that don’t need to scale freak, but in hyper scale computing, when you must create and delete services all the time, that’s not the best way to achieve this goal because the time to create new nodes, machines even containers, and deploy the application there doesn’t meet the time requirements.

So, for this approach I decide to use the Service Fabric API’s to create a service (https://docs.microsoft.com/en-us/rest/api/servicefabric/create-a-service) who create a new instance of other services. 


# Code Example


<b>SaasManager2</b> - The core project who use the Service Fabric API's to create a new instance of one serviceType available on the ImageStore.  
> This is a Stateless service who create a new instance of an another ServiceType 


```<language>
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
```
 <b>MailboxService</b> - A simple service who calls SaaSManager2 with a custom metric as bellow:

```C#
 [HttpGet("{id}/messages")]
        public async System.Threading.Tasks.Task<IActionResult> GetMessagesAsync(string id)
        {
           Requests++;
 
			Stopwatch sw = new Stopwatch();

            
            if (Requests > [CUSTOM_VALUE]) await ScaleService();

		[SOME CODE HERE]
	 }

     private static async Task ScaleService()
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8865/");
            HttpResponseMessage response = await client.GetAsync("/api/ServiceManager/CreateService/[TYPE_OF_SERVICE]");
            
		[SOME CODE HERE]
	}
```

 <b>SFMessageSimulator</b> - Console app who send messagens to MailboxService to test the service communication and scale.  

# Nexts steps: 
Today the running service needs to invoke the SaaSManager2 service to ask it to create a new instance. The main go is to use the metrics generated to create a new instance with no coding on the running service. Implement Service Fabric metrics on the running service to change the way the service will scale.


```C#
 private const int ServiceRequestStartEventId = 99;
        [Event(ServiceRequestStartEventId, Level = EventLevel.Informational, Message = "Service request '{0} achieved the maximum and the service will be scalled' ", Keywords = Keywords.Requests)]
        public void ServiceRequestStart(string requestTypeName)
        {
            WriteEvent(ServiceRequestStartEventId, requestTypeName);
        }


```

# Know issues to fix or improve:
1 - Sometimes the new instance is created using the same port from the running service. Create a new instance of the serviceType with a dynamic port
 
2 - Create a better way to name the new instance of the service. Get the list of service names running and add a "short-GUID" to it. 

# License

This project is licensed under the MIT License - see the this link https://choosealicense.com/licenses/mit/ for details
