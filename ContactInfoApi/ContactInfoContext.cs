using ContactInfoApi.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactInfoApi
{
    public class ContactInfoContext : DbContext
    {
        public ContactInfoContext(DbContextOptions<ContactInfoContext> options) : base(options)
        {
        }

        public DbSet<ContactInfo> Contacts { get; set; }
    }
}
