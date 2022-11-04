using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bridge.Infrastructure.Migrations.Bridge.Postgresql
{
    public partial class AddEntityPkstep2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlaceIdTemp",
                table: "Products",
                newName: "PlaceId");

            migrationBuilder.RenameColumn(
                name: "IdTemp",
                table: "Products",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IdTemp",
                table: "Places",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IdTemp",
                table: "OpeningTime",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlaceId",
                table: "Products",
                newName: "PlaceIdTemp");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "IdTemp");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Places",
                newName: "IdTemp");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "OpeningTime",
                newName: "IdTemp");
        }
    }
}
