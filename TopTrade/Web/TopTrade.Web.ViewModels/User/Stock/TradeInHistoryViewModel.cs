namespace TopTrade.Web.ViewModels.User.Stock
{
    using System;

    using AutoMapper;
    using TopTrade.Data.Models.User;
    using TopTrade.Services.Mapping;

    public class TradeInHistoryViewModel : IMapFrom<Trade>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string TradeType { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string StockName { get; set; }

        public DateTime CreatedOn { get; set; }

        public decimal TotalPrice => this.Price * this.Quantity;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Trade, TradeInHistoryViewModel>()
               .ForMember(x => x.StockName, opt =>
                   opt.MapFrom(x => x.Stock.Name));
        }
    }
}
