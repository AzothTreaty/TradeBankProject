using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeBank3.Migrations
{
    public partial class SecondAttemptToMakeBaselinePPUTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaselinePPUs",
                columns: table => new
                {
                    PaselinePPUID = table.Column<Guid>(nullable: false),
                    UserInputId = table.Column<Guid>(nullable: false),
                    sourceCurrency = table.Column<string>(nullable: true),
                    purchaseCurrency = table.Column<string>(nullable: true),
                    PPU = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaselinePPUs", x => x.PaselinePPUID);
                    table.ForeignKey(
                        name: "FK_BaselinePPUs_UserInput_UserInputId",
                        column: x => x.UserInputId,
                        principalTable: "UserInput",
                        principalColumn: "UserInputId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaselinePPUs_UserInputId",
                table: "BaselinePPUs",
                column: "UserInputId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaselinePPUs");
        }
    }
}
