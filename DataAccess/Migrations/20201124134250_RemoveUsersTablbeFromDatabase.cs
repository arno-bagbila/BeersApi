using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DataAccess.Migrations
{
   public partial class RemoveUsersTablbeFromDatabase : Migration
   {
      protected override void Up(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.DropTable(
             name: "Users");
      }

      protected override void Down(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.CreateTable(
             name: "Users",
             columns: table => new
             {
                Id = table.Column<int>(type: "int", nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                UId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_Users", x => x.Id);
             });
      }
   }
}
