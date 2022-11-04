using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bridge.Infrastructure.Migrations.Bridge.Postgresql
{
    public partial class RenameOpeningTimeProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TwentyFourHours",
                table: "OpeningTime",
                newName: "IsDayoff");

            migrationBuilder.RenameColumn(
                name: "Dayoff",
                table: "OpeningTime",
                newName: "Is24Hours");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDayoff",
                table: "OpeningTime",
                newName: "TwentyFourHours");

            migrationBuilder.RenameColumn(
                name: "Is24Hours",
                table: "OpeningTime",
                newName: "Dayoff");
        }
    }
}
