using ContactInfoApi.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactInfoApi
{
    public partial class ContactInfoContext : DbContext
    {
        public ContactInfoContext(DbContextOptions<ContactInfoContext> options) : base(options)
        {
        }

        public DbSet<ContactInfoModel> Contacts { get; set; }

        //Uncomment when use Mysql
        #region MySql
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=JayCes251120$;database=contactinfo", ServerVersion.FromString("8.0.22-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region MySql code first approach
            //Run below command in Package Manager Console to create and update database.
            //Add-Migration DBInit
            //Update-Database

            // Map entities to tables
            modelBuilder.Entity<ContactInfoModel>().ToTable("Contacts");

            // Configure Primary Keys
            modelBuilder.Entity<ContactInfoModel>().HasKey(ug => ug.Id).HasName("CES_Contacts");

            // Configure columns
            modelBuilder.Entity<ContactInfoModel>().Property(ug => ug.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<ContactInfoModel>().Property(ug => ug.FirstName).HasColumnType("varchar(45)").IsRequired();
            modelBuilder.Entity<ContactInfoModel>().Property(ug => ug.LastName).HasColumnType("varchar(45)").IsRequired();
            modelBuilder.Entity<ContactInfoModel>().Property(ug => ug.MobileNumber).HasColumnType("varchar(45)").IsRequired(false);
            modelBuilder.Entity<ContactInfoModel>().Property(ug => ug.EmailId).HasColumnType("varchar(45)").IsRequired(true);

            #endregion

            //Created entity using scaffolding
            /*modelBuilder.Entity<ContactInfo>(entity =>
            {
                entity.ToTable("contacts");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.EmailId)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_bin");

                entity.Property(e => e.MobileNumber)
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_bin");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_bin");
                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_bin");
            });

            OnModelCreatingPartial(modelBuilder);*/
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        #endregion
    }
}
