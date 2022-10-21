using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bridge.Infrastructure.Migrations.Bridge.Postgresql
{
    public partial class AddLastUpdateDT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreationDateTime",
                table: "Places",
                newName: "CreationDateTimeUtc");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDateTimeUtc",
                table: "Places",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdateDateTimeUtc",
                table: "Places");

            migrationBuilder.RenameColumn(
                name: "CreationDateTimeUtc",
                table: "Places",
                newName: "CreationDateTime");
        }
    }
}
