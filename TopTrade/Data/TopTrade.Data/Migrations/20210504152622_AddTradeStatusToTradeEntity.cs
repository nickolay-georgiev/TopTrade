using Microsoft.EntityFrameworkCore.Migrations;

namespace TopTrade.Data.Migrations
{
    public partial class AddTradeStatusToTradeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TradeStatus",
                table: "Trades",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TradeStatus",
                table: "Trades");
        }
    }
}
