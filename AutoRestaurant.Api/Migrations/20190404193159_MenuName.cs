using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoRestaurant.Api.Migrations
{
    public partial class MenuName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Menus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Menus");
        }
    }
}
