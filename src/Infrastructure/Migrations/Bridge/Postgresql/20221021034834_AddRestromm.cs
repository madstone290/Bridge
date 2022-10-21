using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bridge.Infrastructure.Migrations.Bridge.Postgresql
{
    public partial class AddRestromm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Restrooms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    IsUnisex = table.Column<bool>(type: "boolean", nullable: false),
                    DiaperTableLocation = table.Column<string>(type: "text", nullable: true),
                    MaleToilet = table.Column<int>(type: "integer", nullable: true),
                    MaleUrinal = table.Column<int>(type: "integer", nullable: true),
                    MaleDisabledToilet = table.Column<int>(type: "integer", nullable: true),
                    MaleDisabledUrinal = table.Column<int>(type: "integer", nullable: true),
                    MaleKidToilet = table.Column<int>(type: "integer", nullable: true),
                    MaleKidUrinal = table.Column<int>(type: "integer", nullable: true),
                    FemaleToilet = table.Column<int>(type: "integer", nullable: true),
                    FemaleKidToilet = table.Column<int>(type: "integer", nullable: true),
                    FemaleDisabledToilet = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restrooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Restrooms_Places_Id",
                        column: x => x.Id,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Restrooms");
        }
    }
}
