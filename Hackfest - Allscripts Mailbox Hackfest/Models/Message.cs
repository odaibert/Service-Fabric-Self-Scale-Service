using Newtonsoft.Json;
using System;

namespace Models
{
    
    public class Message
    {
        public Message()
        {
            this.Id = Guid.NewGuid();
            this.CreateDateTime = DateTime.UtcNow.AddDays(-1); //TODO: stop creating date in the past!
        }

        [JsonProperty(PropertyName = "isRead")]
        public bool IsRead { get; set; }

        [JsonProperty(PropertyName = "readDateTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ReadDateTime { get; set; }

        [JsonProperty(PropertyName = "createDateTime")]
        public DateTime CreateDateTime { get; internal set; }

        [JsonProperty(PropertyName = "ttl", NullValueHandling = NullValueHandling.Ignore)]
        public long? Ttl { get; set; }

        [JsonProperty(PropertyName = "readBy", NullValueHandling = NullValueHandling.Ignore)]
        public string ReadBy { get; set; }

        [JsonProperty(PropertyName = "messageType")]
        public string MessageType { get; set; }
    
        [JsonProperty(PropertyName = "hubId")]
        public Guid HubId { get; set; }

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; internal set; }

        [JsonProperty(PropertyName = "partnerId")]
        public string PartnerID { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }
        
        public bool IsValid()
        {
            return ! (HubId == null
                || Ttl == null || Ttl <= 0
                || String.IsNullOrEmpty(MessageType)
                || String.IsNullOrEmpty(Body)
                || String.IsNullOrEmpty(PartnerID)
             );
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
