using ContactInfoApi.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactInfoApi.Repository
{
    public class ContactReposistory : IContactReposistory
    {
        public ContactInfoContext db;
        public ContactReposistory(ContactInfoContext _db)
        {
            db = _db;
            if (!db.Contacts.Any())
            {
                AddTestData();
            }
        }

        public async Task<int> AddContact(ContactInfo contact)
        {
            if (db != null)
            {
                await db.Contacts.AddAsync(contact);
                await db.SaveChangesAsync();

                return contact.Id;
            }

            return 0;
        }

        public async Task<ContactInfo> GetContact(int? id)
        {
            if (db != null)
            {
                return await(from c in db.Contacts
                             where c.Id == id
                             select new ContactInfo
                             {
                                 Id = c.Id,
                                 FirstName = c.FirstName,
                                 LastName = c.LastName,
                                 EmailId = c.EmailId,
                                 MobileNumber = c.MobileNumber
                             }).FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<List<ContactInfo>> GetContacts()
        {
            if (db != null)
            {
                return await db.Contacts.ToListAsync();
            }

            return null;
        }

        public async Task UpdateContact(ContactInfo contact)
        {
            if (db != null)
            {
                var searchContact = await db.Contacts.FirstOrDefaultAsync(x => x.Id == contact.Id);
                searchContact.EmailId = contact.EmailId;
                searchContact.FirstName = contact.FirstName;
                searchContact.LastName = contact.LastName;
                searchContact.MobileNumber = searchContact.MobileNumber;
                db.Contacts.Update(searchContact);
                await db.SaveChangesAsync();
            }
        }
        public async Task<int> DeleteContact(int? id)
        {
            int result = 0;

            if (db != null)
            {
                var contact = await db.Contacts.FirstOrDefaultAsync(x => x.Id == id);

                if (contact != null)
                {
                    db.Contacts.Remove(contact);

                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }
        #region TestData
        private void AddTestData()
        {
            var contact1 = new ContactInfo() { Id = 1, FirstName = "Willam", LastName = "Shakes", MobileNumber = "+4142353246", EmailId = "Willam@outlook.com" };
            var contact2 = new ContactInfo() { Id = 2, FirstName = "Johnny", LastName = "Deep", MobileNumber = "+9842353246", EmailId = "Johnny@outlook.com" };
            var contact3 = new ContactInfo() { Id = 3, FirstName = "Vin", LastName = "Disel", MobileNumber = "+3142353246", EmailId = "Vin@outlook.com" };
            var contact4 = new ContactInfo() { Id = 4, FirstName = "Robert", LastName = "Downey", MobileNumber = "+7142353246", EmailId = "Robert@outlook.com" };
            var contact5 = new ContactInfo() { Id = 5, FirstName = "Will", LastName = "Buyers", MobileNumber = "+9142353246", EmailId = "Will@outlook.com" };
            db.Add(contact1);
            db.Add(contact2);
            db.Add(contact3);
            db.Add(contact4);
            db.Add(contact5);
            db.SaveChanges();

        }
        #endregion
    }
}
