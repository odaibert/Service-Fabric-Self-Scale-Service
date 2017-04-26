using Models;
using System;
using System.Diagnostics;
// ReSharper disable RedundantCatchClause

namespace Tester
{
    class Program
    {
        private const string DatabaseId = "mailbox";
        private const string CollectionId = "mail";
        private const string AccountKey = "[AccountKey]";
        private const string EndpointUri = "[EndpointUri]";
        
        static void Main(string[] args)
        {
            var docdbRepo = new DocumentDBRepository.MessageRepository<Message>(EndpointUri, AccountKey, DatabaseId, CollectionId);
            var hubId = Guid.NewGuid();
            int count = 20;

            while (count > 0)
            {
                Message m = new Message { HubId = hubId };
                var valid = m.IsValid();

                m = new Message { HubId = hubId, MessageType = "foo", Body = "{'bah': 'bah black sheep'}", PartnerID = "P1", Ttl = 604800 };
                valid = m.IsValid();
                Stopwatch s = Stopwatch.StartNew();
                try
                {
                    var doc = docdbRepo.CreateMessageAsync(m).GetAwaiter().GetResult();
                    s.Stop();
                    Console.WriteLine(doc.ToString());
                    Console.WriteLine("Elapsed Time to store: {0} ms", s.ElapsedMilliseconds);
                }
                catch (Exception)
                {

                    throw;
                }

                try
                {
                    s = Stopwatch.StartNew();
                    var messages = docdbRepo.GetUnreadByHubIdAsync(hubId).GetAwaiter().GetResult();
                    s.Stop();
                    Console.WriteLine($"We found {messages.Count} unread messages for hub-id {hubId}");
                    Console.WriteLine("Elapsed Time to read: {0} ms", s.ElapsedMilliseconds);
                }
                catch (Exception e)
                {

                }
                count--;
            }

            Console.ReadLine();
        }
    }
}
