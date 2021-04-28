using Microsoft.EntityFrameworkCore.Migrations;

namespace TopTrade.Data.Migrations
{
    public partial class RemoveRelationBetweenUserAndStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Stocks_StockId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_StockId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AccountsStatistics",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "AccountsStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_StockId",
                table: "AspNetUsers",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Stocks_StockId",
                table: "AspNetUsers",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
