using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository<T> where T: Message
    {
        Task<T> CreateMessageAsync(Message message);
        Task<List<T>> GetAllByHubIdAsync(Guid hubId);
        Task<List<T>> GetUnreadByHubIdAsync(Guid hubId);

        //void Init();
    }
}
