using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Repository;
using DocumentDBRepository;
using Mailbox.Options;
using Microsoft.Extensions.Options;
using Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mailbox.Controllers
{
    [Route("api/hub")]
    public class MailboxController : Controller
    {
        private static IRepository<Message> _repository;
        private static MyOptions _options;
        private static int _activeRequest = 0;

        public MailboxController(IOptionsSnapshot<MyOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;

            if (_repository == null)
            {
                var accountKey = _options.DocumentDbConfig.AccountKey;
                var endpointUri = _options.DocumentDbConfig.EndpointUri;
                var databaseId = _options.DocumentDbConfig.DatabaseId;
                var collectionId = _options.DocumentDbConfig.CollectionId;

                _repository = new MessageRepository<Message>(endpointUri, accountKey, databaseId, collectionId);
            }
        }

        [HttpGet("{id}/messages")]
        public async System.Threading.Tasks.Task<IActionResult> GetMessagesAsync(string id)
        {
            Stopwatch sw = new Stopwatch();

            _activeRequest++;

            if (_activeRequest > 100) await CreateNewServiceInstance(); // The calls are Async, and as example I choose 100 concurrent calls. 

            Guid hubId = Guid.Parse(id);

            string activityId = Guid.NewGuid().ToString();
            ServiceEventSource.Current.ServiceRequestStart("MailboxController.GetMessages", activityId);

            sw.Reset();
            sw.Start();

            var messages = _repository.GetAllByHubIdAsync(hubId).GetAwaiter().GetResult();

            sw.Stop();

            ServiceEventSource.Current.WriteInfoMessage($"Query to repository took {sw.ElapsedMilliseconds} ms", activityId);
            this.Response.Headers["x-fabricdemo-db-ellapsed"] = $"{sw.ElapsedMilliseconds} ms";

            ServiceEventSource.Current.ServiceRequestStop("MailboxController.GetMessages", activityId);
            _activeRequest--;

            return Ok(messages);

        }

        /// <summary>
        /// Calls the SaaSManager2 service to create a new instance of THIS service. 
        /// In this version, all the information of THIS service, Application Name, ServiceType and Service Name are hard coded on the SaaSManager2. 
        /// On vNext the SaaSManager2 will receive the context parameters from THIS service. 
        /// </summary>
        /// <returns></returns>
        private static async Task CreateNewServiceInstance()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8865/");
            HttpResponseMessage response = await client.GetAsync("/api/ServiceManager");

            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine(response.StatusCode.ToString());
            }
        }
    }
}


