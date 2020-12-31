using ContactInfoApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactInfoApi.Repository
{
    public interface IContactReposistory
    {
        Task<List<ContactInfo>> GetContacts();
        Task<ContactInfo> GetContact(int? id);
        Task UpdateContact(ContactInfo contact);
        Task<int> AddContact(ContactInfo contact);
        Task<int> DeleteContact(int? id);

    }
}
