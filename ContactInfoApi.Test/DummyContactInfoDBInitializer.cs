using AutoFixture;
using ContactInfoApi.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactInfoApi.Test
{
    public static class DummyContactInfoDBInitializer
    {
        public static IEnumerable<ContactInfo> Contacts {get;set;}
        public static void Seed(ContactInfoContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            Contacts = new Fixture().Build<ContactInfo>().CreateMany(10);
            context.Contacts.AddRange(Contacts);
            context.SaveChanges();
        }
    }
}
