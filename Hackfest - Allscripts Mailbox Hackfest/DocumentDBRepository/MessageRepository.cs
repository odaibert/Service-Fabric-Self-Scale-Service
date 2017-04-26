using Repository;
using Models;
using System;
using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace DocumentDBRepository
{
    public class MessageRepository<T> : IRepository<T> where T: Message
    {
        private DocumentClient client;
        private string databaseId;
        private string collectionId;
        
        public MessageRepository(string EndpointUri, string AccountKey, string DatabaseId, string CollectionId)
        {
            this.client = new DocumentClient(new Uri(EndpointUri), AccountKey,
                new ConnectionPolicy
                {
                    ConnectionMode = ConnectionMode.Direct,
                    ConnectionProtocol = Protocol.Tcp
                });
            client.OpenAsync().GetAwaiter();
            this.databaseId = DatabaseId;
            this.collectionId = CollectionId;
        }


        public async Task<List<T>> GetAllByHubIdAsync(Guid hubId)
        {
            return await GetAsync(m => m.HubId == hubId);
        }

        public async Task<List<T>> GetUnreadByHubIdAsync(Guid hubId)
        {
            var messages = await GetAsync(m => m.HubId == hubId && !m.IsRead);
            var tasks = new List<Task>();
            var readDatetime = DateTime.UtcNow;

            foreach (var message in messages)
            {
                tasks.Add(MarkAsReadAsync(message, readDatetime));
            }

            Task.WaitAll(tasks.ToArray());
            return messages; 
        }

        private async Task MarkAsReadAsync(Message message, DateTime readDatetime)
        {
            var docLink = UriFactory.CreateDocumentUri(this.databaseId, this.collectionId, message.Id.ToString());
            message.ReadDateTime = readDatetime;
            message.IsRead = true;
            long age = (long)(readDatetime.Subtract(message.CreateDateTime).TotalSeconds);
            message.Ttl = 604800 - age;
            
            await client.ReplaceDocumentAsync(docLink, message);
        }

        private async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var collLink = UriFactory.CreateDocumentCollectionUri(this.databaseId, this.collectionId);

            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(collLink, new FeedOptions { MaxItemCount = -1 })
                    .Where(predicate)
                    .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }
        public async Task<T> CreateMessageAsync(Message message)
        {
            message.IsRead = false;
            var doc = await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId), message);
            return (dynamic)doc.Resource;
        }
    }
}
