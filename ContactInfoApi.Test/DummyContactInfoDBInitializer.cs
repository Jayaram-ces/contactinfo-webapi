using AutoFixture;
using ContactInfoApi.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactInfoApi.Test
{
    public static class DummyContactInfoDBInitializer
    {
        public static void Seed(ContactInfoContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Contacts.AddRange(
                    new ContactInfo() { Id = 1, FirstName = "Willam", LastName = "Shakes", MobileNumber = "+4142353246", EmailId = "Willam@outlook.com" },
                    new ContactInfo() { Id = 2, FirstName = "Johnny", LastName = "Deep", MobileNumber = "+9842353246", EmailId = "Johnny@outlook.com" },
                    new ContactInfo() { Id = 3, FirstName = "Vin", LastName = "Disel", MobileNumber = "+3142353246", EmailId = "Vin@outlook.com" },
                    new ContactInfo() { Id = 4, FirstName = "Robert", LastName = "Downey", MobileNumber = "+7142353246", EmailId = "Robert@outlook.com" },
                    new ContactInfo() { Id = 5, FirstName = "Will", LastName = "Buyers", MobileNumber = "+9142353246", EmailId = "Will@outlook.com" }
              );
            context.SaveChanges();
        }
    }
}
