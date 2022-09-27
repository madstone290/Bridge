using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bridge.Infrastructure.Migrations.Bridge.Postgresql
{
    public partial class RollbackCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaceCategoryItem");

            migrationBuilder.DropTable(
                name: "ProductCategoryItem");

            migrationBuilder.AddColumn<string>(
                name: "Categories",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Categories",
                table: "Places",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categories",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Categories",
                table: "Places");

            migrationBuilder.CreateTable(
                name: "PlaceCategoryItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "text", nullable: false),
                    PlaceId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceCategoryItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaceCategoryItem_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategoryItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategoryItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategoryItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaceCategoryItem_PlaceId",
                table: "PlaceCategoryItem",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryItem_ProductId",
                table: "ProductCategoryItem",
                column: "ProductId");
        }
    }
}
