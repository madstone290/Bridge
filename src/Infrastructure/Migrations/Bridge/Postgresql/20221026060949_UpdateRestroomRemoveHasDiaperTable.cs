﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bridge.Infrastructure.Migrations.Bridge.Postgresql
{
    public partial class UpdateRestroomRemoveHasDiaperTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasDiaperTable",
                table: "Restrooms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasDiaperTable",
                table: "Restrooms",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
