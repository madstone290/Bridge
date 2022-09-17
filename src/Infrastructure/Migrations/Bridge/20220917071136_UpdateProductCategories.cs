using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bridge.Infrastructure.Migrations.Bridge
{
    public partial class UpdateProductCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Categories",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categories",
                table: "Products");
        }
    }
}
