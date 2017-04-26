namespace Mailbox.Options
{
    public class MyOptions
    {
        public MyDocumentDbConfig DocumentDbConfig { get; set; }
    }

    public class MyDocumentDbConfig
    {
        public string EndpointUri { get; set; }
        public string AccountKey { get; set; }
        public string DatabaseId { get; set; }
        public string CollectionId { get; set; }
        public long TimeToLiveDefault { get; set; }
    }
}
