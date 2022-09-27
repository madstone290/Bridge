using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bridge.Infrastructure.Migrations.Bridge.Postgresql
{
    public partial class AddPlaceTypeProductType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Places",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Places");
        }
    }
}
