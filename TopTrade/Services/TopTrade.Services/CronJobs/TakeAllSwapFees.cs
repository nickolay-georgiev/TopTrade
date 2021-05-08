namespace TopTrade.Services.CronJobs
{
    using System.Threading.Tasks;

    using TopTrade.Services.Data.User;

    public class TakeAllSwapFees
    {
        private readonly ITradeService tradeService;

        public TakeAllSwapFees(ITradeService tradeService)
        {
            this.tradeService = tradeService;
        }

        public async Task Work()
        {
            await this.tradeService.TakeAllSwapFeesAsync();
        }
    }
}
