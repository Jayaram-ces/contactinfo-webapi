using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContactInfoApi.Migrations.ContactInfo
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "varchar(45)", nullable: false),
                    LastName = table.Column<string>(type: "varchar(45)", nullable: false),
                    MobileNumber = table.Column<string>(type: "varchar(45)", nullable: true),
                    EmailId = table.Column<string>(type: "varchar(45)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CES_Contacts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");
        }
    }
}
