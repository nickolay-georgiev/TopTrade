using Microsoft.EntityFrameworkCore.Migrations;

namespace TopTrade.Data.Migrations
{
    public partial class AddBalancePropToTradeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CashResult",
                table: "Trades",
                newName: "Balance");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "Trades",
                newName: "CashResult");
        }
    }
}
