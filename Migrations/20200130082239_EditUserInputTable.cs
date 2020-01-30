using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeBank3.Migrations
{
    public partial class EditUserInputTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "UserInput",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "timestampCreated",
                table: "UserInput",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "UserInput");

            migrationBuilder.DropColumn(
                name: "timestampCreated",
                table: "UserInput");
        }
    }
}
