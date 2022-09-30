using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bridge.Infrastructure.Migrations.Bridge.Postgresql
{
    public partial class UpdateAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Places",
                newName: "Address_SiGuGun");

            migrationBuilder.AddColumn<string>(
                name: "Address_Details",
                table: "Places",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_EupMyeonDong",
                table: "Places",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_JibunAddress",
                table: "Places",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_PostalCode",
                table: "Places",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_RoadAddress",
                table: "Places",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_RoadName",
                table: "Places",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_SiDo",
                table: "Places",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_Details",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "Address_EupMyeonDong",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "Address_JibunAddress",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "Address_PostalCode",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "Address_RoadAddress",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "Address_RoadName",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "Address_SiDo",
                table: "Places");

            migrationBuilder.RenameColumn(
                name: "Address_SiGuGun",
                table: "Places",
                newName: "Address");
        }
    }
}
