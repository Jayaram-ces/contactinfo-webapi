using ContactInfoApi.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactInfoApi.Repository
{
    public class ContactRepository : IContactRepository
    {
        public ContactInfoContext dataBase;
        public ContactRepository(ContactInfoContext _dataBase)
        {
            dataBase = _dataBase;
            if (!dataBase.Contacts.Any())
            {
                //Comment while run test to get accurate pass results.
                AddTestData();
            }
        }

        public async Task AddAsync(ContactInfoModel contact)
        {
            if (dataBase != null)
            {
                await dataBase.Contacts.AddAsync(contact);
                await dataBase.SaveChangesAsync();
            }
        }

        public async Task<ContactInfoModel> GetContactAsync(int id)
        {
            if (dataBase != null)
            {
                return await (from c in dataBase.Contacts where c.Id == id  
                                                          select new ContactInfoModel {   Id = c.Id,
                                                                                          FirstName = c.FirstName,
                                                                                          LastName = c.LastName,
                                                                                          EmailId = c.EmailId,
                                                                                          MobileNumber = c.MobileNumber
                                                                                      }).FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<IEnumerable<ContactInfoModel>> GetContactsAsync()
        {
            if (dataBase != null)
            {
                return await dataBase.Contacts.ToListAsync();
            }

            return null;
        }

        public async Task UpdateAsync(ContactInfoModel contact)
        {
            if (dataBase != null)
            {
                var searchContact = await dataBase.Contacts.FirstOrDefaultAsync(x => x.Id == contact.Id);

                if (searchContact == null)
                    throw new Exception("No such contact found in the directary");

                searchContact.EmailId = contact.EmailId;
                searchContact.FirstName = contact.FirstName;
                searchContact.LastName = contact.LastName;
                searchContact.MobileNumber = searchContact.MobileNumber;
                dataBase.Contacts.Update(searchContact);
                await dataBase.SaveChangesAsync();
            }
        }
        public async Task DeleteAsync(int id)
        {
            if (dataBase != null)
            {
                var contact = await dataBase.Contacts.FirstOrDefaultAsync(x => x.Id == id);
                if (contact == null)
                    throw new Exception("No such contact found in the directary");

                if (contact != null)
                {
                    dataBase.Contacts.Remove(contact);
                    await dataBase.SaveChangesAsync();
                }
            }
        }
        #region TestData
        private void AddTestData()
        {
            var contact1 = new ContactInfoModel() { Id = 1, FirstName = "Willam", LastName = "Shakes", MobileNumber = "+4142353246", EmailId = "Willam@outlook.com" };
            var contact2 = new ContactInfoModel() { Id = 2, FirstName = "Johnny", LastName = "Deep", MobileNumber = "+9842353246", EmailId = "Johnny@outlook.com" };
            var contact3 = new ContactInfoModel() { Id = 3, FirstName = "Vin", LastName = "Disel", MobileNumber = "+3142353246", EmailId = "Vin@outlook.com" };
            var contact4 = new ContactInfoModel() { Id = 4, FirstName = "Robert", LastName = "Downey", MobileNumber = "+7142353246", EmailId = "Robert@outlook.com" };
            var contact5 = new ContactInfoModel() { Id = 5, FirstName = "Will", LastName = "Buyers", MobileNumber = "+9142353246", EmailId = "Will@outlook.com" };
            dataBase.Add(contact1);
            dataBase.Add(contact2);
            dataBase.Add(contact3);
            dataBase.Add(contact4);
            dataBase.Add(contact5);
            dataBase.SaveChanges();

        }
        #endregion
    }
}
