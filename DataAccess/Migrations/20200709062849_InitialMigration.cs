using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DataAccess.Migrations
{
   public partial class InitialMigration : Migration
   {
      protected override void Up(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.CreateTable(
             name: "Category",
             columns: table => new
             {
                Id = table.Column<int>(nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                UId = table.Column<Guid>(nullable: false),
                Name = table.Column<string>(maxLength: 50, nullable: false),
                Description = table.Column<string>(maxLength: 3000, nullable: false)
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_Category", x => x.Id);
             });

         migrationBuilder.CreateTable(
             name: "Color",
             columns: table => new
             {
                Id = table.Column<int>(nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                UId = table.Column<Guid>(nullable: false),
                Name = table.Column<string>(maxLength: 50, nullable: false)
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_Color", x => x.Id);
             });

         migrationBuilder.CreateTable(
             name: "Country",
             columns: table => new
             {
                Id = table.Column<int>(nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                UId = table.Column<Guid>(nullable: false),
                Name = table.Column<string>(maxLength: 50, nullable: false),
                Code = table.Column<string>(maxLength: 10, nullable: false)
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_Country", x => x.Id);
             });

         migrationBuilder.CreateTable(
             name: "Flavour",
             columns: table => new
             {
                Id = table.Column<int>(nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(maxLength: 50, nullable: false),
                Description = table.Column<string>(maxLength: 3000, nullable: false),
                UId = table.Column<Guid>(nullable: false)
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_Flavour", x => x.Id);
             });

         migrationBuilder.CreateTable(
             name: "Users",
             columns: table => new
             {
                Id = table.Column<int>(nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                UId = table.Column<Guid>(nullable: false),
                Email = table.Column<string>(nullable: true),
                Firstname = table.Column<string>(nullable: true),
                Lastname = table.Column<string>(nullable: true),
                DateOfBirth = table.Column<DateTime>(nullable: false),
                CountryId = table.Column<Guid>(nullable: false),
                City = table.Column<string>(nullable: true),
                IsAdmin = table.Column<bool>(nullable: false),
                PasswordHash = table.Column<byte[]>(nullable: true),
                PasswordSalt = table.Column<byte[]>(nullable: true),
                JoinDate = table.Column<DateTime>(nullable: false)
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_Users", x => x.Id);
             });

         migrationBuilder.CreateTable(
             name: "Beer",
             columns: table => new
             {
                Id = table.Column<int>(nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                UId = table.Column<Guid>(nullable: false),
                Description = table.Column<string>(maxLength: 3000, nullable: false),
                Name = table.Column<string>(maxLength: 50, nullable: false),
                AlcoholLevel = table.Column<double>(nullable: false),
                TiwooRating = table.Column<double>(nullable: false),
                CategoryId = table.Column<int>(nullable: false),
                ColorId = table.Column<int>(nullable: false),
                LogoUrl = table.Column<string>(maxLength: 2048, nullable: false),
                CountryId = table.Column<int>(nullable: false),
                DateRegistered = table.Column<DateTime>(nullable: false)
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_Beer", x => x.Id);
                table.ForeignKey(
                       name: "FK_Beer_Category_CategoryId",
                       column: x => x.CategoryId,
                       principalTable: "Category",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                       name: "FK_Beer_Color_ColorId",
                       column: x => x.ColorId,
                       principalTable: "Color",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                       name: "FK_Beer_Country_CountryId",
                       column: x => x.CountryId,
                       principalTable: "Country",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
             });

         migrationBuilder.CreateTable(
             name: "BeerFlavour",
             columns: table => new
             {
                BeerId = table.Column<int>(nullable: false),
                FlavourId = table.Column<int>(nullable: false)
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_BeerFlavour", x => new { x.BeerId, x.FlavourId });
                table.ForeignKey(
                       name: "FK_BeerFlavour_Beer_BeerId",
                       column: x => x.BeerId,
                       principalTable: "Beer",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                       name: "FK_BeerFlavour_Flavour_FlavourId",
                       column: x => x.FlavourId,
                       principalTable: "Flavour",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
             });

         migrationBuilder.CreateTable(
             name: "Image",
             columns: table => new
             {
                Id = table.Column<int>(nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                UId = table.Column<Guid>(nullable: false),
                Title = table.Column<string>(maxLength: 50, nullable: false),
                ImageUrl = table.Column<string>(maxLength: 200, nullable: false),
                BeerId = table.Column<int>(nullable: false),
                DateRegistered = table.Column<DateTime>(nullable: false)
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_Image", x => x.Id);
                table.ForeignKey(
                       name: "FK_Image_Beer_BeerId",
                       column: x => x.BeerId,
                       principalTable: "Beer",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
             });

         migrationBuilder.CreateIndex(
             name: "IX_Beer_CategoryId",
             table: "Beer",
             column: "CategoryId");

         migrationBuilder.CreateIndex(
             name: "IX_Beer_ColorId",
             table: "Beer",
             column: "ColorId");

         migrationBuilder.CreateIndex(
             name: "IX_Beer_CountryId",
             table: "Beer",
             column: "CountryId");

         migrationBuilder.CreateIndex(
             name: "IX_BeerFlavour_FlavourId",
             table: "BeerFlavour",
             column: "FlavourId");

         migrationBuilder.CreateIndex(
             name: "IX_Image_BeerId",
             table: "Image",
             column: "BeerId");
      }

      protected override void Down(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.DropTable(
             name: "BeerFlavour");

         migrationBuilder.DropTable(
             name: "Image");

         migrationBuilder.DropTable(
             name: "Users");

         migrationBuilder.DropTable(
             name: "Flavour");

         migrationBuilder.DropTable(
             name: "Beer");

         migrationBuilder.DropTable(
             name: "Category");

         migrationBuilder.DropTable(
             name: "Color");

         migrationBuilder.DropTable(
             name: "Country");
      }
   }
}
