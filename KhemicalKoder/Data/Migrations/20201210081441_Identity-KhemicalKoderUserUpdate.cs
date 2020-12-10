using Microsoft.EntityFrameworkCore.Migrations;

namespace KhemicalKoder.Data.Migrations
{
    public partial class IdentityKhemicalKoderUserUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                "IsAdmin",
                "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "Name",
                "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "Discriminator",
                "AspNetUsers",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "IsAdmin",
                "AspNetUsers");

            migrationBuilder.DropColumn(
                "Name",
                "AspNetUsers");

            migrationBuilder.DropColumn(
                "Discriminator",
                "AspNetUsers");
        }
    }
}