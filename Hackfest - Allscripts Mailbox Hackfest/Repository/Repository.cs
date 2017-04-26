using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class Repository<T> : IRepository<T> where T : Message
    {
        public Message CreateMessage(string partnerid, string messagebody)
        {
            var message = new Message(); 

            message.PartnerID = partnerid;
            message.Body = messagebody;

            return message;
        }

        public Task<T> CreateMessageAsync(Message message)
        {
            throw new NotImplementedException();
        }

        public List<Message> GetAll()
        {
            var m = new List<Message>();

            var _m = new Message();

            for (var i = 0; i <= 5; i++)
            {
                //_m.Id = Guid.NewGuid();
                m.Add(_m);
            }

            return m;
        }

        public Task<List<T>> GetAllByHubIdAsync(Guid hubId)
        {
            throw new NotImplementedException();
        }

        public Message GetMessageById(Guid hubid)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetUnreadByHubIdAsync(Guid hubId)
        {
            throw new NotImplementedException();
        }
    }
}
