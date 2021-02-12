using ContactInfoApi.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactInfoApi.Repository
{
    public interface IContactRepository
    {
        Task<IEnumerable<ContactInfoModel>> GetContactsAsync();

        Task<ContactInfoModel> GetContactAsync(int id);

        Task UpdateAsync(ContactInfoModel contact);

        Task AddAsync(ContactInfoModel contact);

        Task DeleteAsync(int id);

    }
}
