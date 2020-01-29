using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeBank3.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Baseline",
                columns: table => new
                {
                    baselineId = table.Column<Guid>(nullable: false),
                    recordId = table.Column<string>(nullable: true),
                    originType = table.Column<string>(nullable: true),
                    originModifier = table.Column<decimal>(nullable: false),
                    usdModifier = table.Column<decimal>(nullable: false),
                    gdpModifier = table.Column<decimal>(nullable: false),
                    createdTs = table.Column<DateTime>(nullable: false),
                    version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baseline", x => x.baselineId);
                });

            migrationBuilder.CreateTable(
                name: "UserInput",
                columns: table => new
                {
                    UserInputId = table.Column<Guid>(nullable: false),
                    requestType = table.Column<string>(nullable: true),
                    tradeId = table.Column<string>(nullable: true),
                    sourceCurrency = table.Column<string>(nullable: true),
                    PPU = table.Column<decimal>(nullable: false),
                    purchaseAmount = table.Column<decimal>(nullable: false),
                    purchaseCurrency = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInput", x => x.UserInputId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Baseline");

            migrationBuilder.DropTable(
                name: "UserInput");
        }
    }
}
