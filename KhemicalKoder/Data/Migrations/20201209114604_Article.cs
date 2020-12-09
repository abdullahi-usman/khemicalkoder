using Microsoft.EntityFrameworkCore.Migrations;

namespace KhemicalKoder.Data.Migrations
{
    public partial class Article : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Article",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    Story = table.Column<string>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Article", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Article");
        }
    }
}