namespace TopTrade.Web.ViewModels.User.Stock
{
    using System;

    using AutoMapper;
    using TopTrade.Data.Models.User;
    using TopTrade.Services.Mapping;

    public class TradeInPortfolioViewModel : IMapFrom<Trade>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string StockName { get; set; }

        public string StockTicker { get; set; }

        public int Quantity { get; set; }

        public decimal OpenPrice { get; set; }

        public decimal CurrentPrice { get; set; }

        public string TradeType { get; set; }

        public decimal ProfitLossInCash { get; set; }

        public decimal ProfitLossInPercent { get; set; }

        public string Logo => string.Join(string.Empty, this.StockName.Split(new char[] { ' ', '.' })[0]);

        public decimal TotalInvested => this.OpenPrice * this.Quantity;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Trade, TradeInPortfolioViewModel>()
               .ForMember(x => x.TradeType, opt =>
                   opt.MapFrom(x => x.TradeType == "BUY" ? "BUYING" : "SELLING"));
        }
    }
}
