using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bridge.Infrastructure.Migrations.Bridge.Postgresql
{
    public partial class AddEntityPkstep1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpeningTime_Places_PlaceId",
                table: "OpeningTime");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Places_PlaceId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Restrooms_Places_Id",
                table: "Restrooms");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Restrooms",
                newName: "Pk");

            migrationBuilder.RenameColumn(
                name: "PlaceId",
                table: "Products",
                newName: "PlacePk");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "Pk");

            migrationBuilder.RenameIndex(
                name: "IX_Products_PlaceId",
                table: "Products",
                newName: "IX_Products_PlacePk");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Places",
                newName: "Pk");

            migrationBuilder.RenameColumn(
                name: "PlaceId",
                table: "OpeningTime",
                newName: "PlacePk");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "OpeningTime",
                newName: "Pk");

            migrationBuilder.RenameIndex(
                name: "IX_OpeningTime_PlaceId",
                table: "OpeningTime",
                newName: "IX_OpeningTime_PlacePk");

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningTime_Places_PlacePk",
                table: "OpeningTime",
                column: "PlacePk",
                principalTable: "Places",
                principalColumn: "Pk",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Places_PlacePk",
                table: "Products",
                column: "PlacePk",
                principalTable: "Places",
                principalColumn: "Pk",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Restrooms_Places_Pk",
                table: "Restrooms",
                column: "Pk",
                principalTable: "Places",
                principalColumn: "Pk",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpeningTime_Places_PlacePk",
                table: "OpeningTime");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Places_PlacePk",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Restrooms_Places_Pk",
                table: "Restrooms");

            migrationBuilder.RenameColumn(
                name: "Pk",
                table: "Restrooms",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PlacePk",
                table: "Products",
                newName: "PlaceId");

            migrationBuilder.RenameColumn(
                name: "Pk",
                table: "Products",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Products_PlacePk",
                table: "Products",
                newName: "IX_Products_PlaceId");

            migrationBuilder.RenameColumn(
                name: "Pk",
                table: "Places",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PlacePk",
                table: "OpeningTime",
                newName: "PlaceId");

            migrationBuilder.RenameColumn(
                name: "Pk",
                table: "OpeningTime",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_OpeningTime_PlacePk",
                table: "OpeningTime",
                newName: "IX_OpeningTime_PlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningTime_Places_PlaceId",
                table: "OpeningTime",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Places_PlaceId",
                table: "Products",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Restrooms_Places_Id",
                table: "Restrooms",
                column: "Id",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
