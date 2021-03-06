namespace TopTrade.Services.Data.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AlphaVantage.Net.Stocks;
    using TopTrade.Common;
    using TopTrade.Data.Common.Repositories;
    using TopTrade.Data.Models.User;
    using TopTrade.Data.Models.User.Enums;
    using TopTrade.Services.Mapping;
    using TopTrade.Web.ViewModels.User;
    using TopTrade.Web.ViewModels.User.Profile;
    using TopTrade.Web.ViewModels.User.Stock;

    public class StockService : IStockService
    {
        private readonly IAlphaVantageApiClientService alphaVantageService;
        private readonly IDeletableEntityRepository<Trade> tradeRepository;
        private readonly IDeletableEntityRepository<Stock> stockRepository;
        private readonly IDeletableEntityRepository<Watchlist> watchlistRepository;
        private readonly IDeletableEntityRepository<AccountStatistic> accountStatisticRepository;

        public StockService(
            IAlphaVantageApiClientService alphaVantageService,
            IDeletableEntityRepository<Trade> tradeRepository,
            IDeletableEntityRepository<Stock> stockRepository,
            IDeletableEntityRepository<Watchlist> watchlistRepository,
            IDeletableEntityRepository<AccountStatistic> accountStatisticRepository)
        {
            this.tradeRepository = tradeRepository;
            this.stockRepository = stockRepository;
            this.alphaVantageService = alphaVantageService;
            this.watchlistRepository = watchlistRepository;
            this.accountStatisticRepository = accountStatisticRepository;
        }

        public async Task<StockSearchResultViewModel[]> SearchStocksBySymbolAsync(string stockNameOrTicker)
        {
            return await this.alphaVantageService.SearchStockBySymbolAsync(stockNameOrTicker);
        }

        public async Task<StockViewModel> GetStockDataAsync(string stockNameOrTicker)
        {
            return await this.alphaVantageService.GetStockDataAsync(stockNameOrTicker);
        }

        public async Task<StockTimeSeries> GetStockTimeSeriesAsync(string stockNameOrTicker)
        {
            return await this.alphaVantageService.GetStockTimeSeriesAsync(stockNameOrTicker);
        }

        public async Task<UserDashboardViewModel> GetUserWatchlistWithStatisticAsync(string userId)
        {
            var userWatchlistStocks = this.watchlistRepository
                 .AllAsNoTracking()
                 .Where(x => x.UserId == userId)
                 .SelectMany(x => x.Stocks)
                 .Select(x => new StockViewModel
                 {
                     Name = x.Stock.Name,
                     Ticker = x.Stock.Ticker,
                 }).ToList();

            foreach (var stock in userWatchlistStocks)
            {
                var totalTrades = this.GetStockBuyPercentTrades(stock.Ticker);
                var stockData = await this.GetStockDataAsync(stock.Ticker);
                var tradeStatus = this.tradeRepository
                    .AllAsNoTracking()
                    .Where(x => x.UserId == userId)
                    .Any(x => x.Stock.Ticker == stock.Ticker && x.TradeStatus == TradeStatus.OPEN.ToString())
                    ? TradeStatus.OPEN.ToString() : TradeStatus.CLOSE.ToString();

                stock.Change = stockData.Change;
                stock.ChangePercent = stockData.ChangePercent;
                stock.Price = stockData.Price;
                stock.BuyPercent = totalTrades.TotalBuyPercentTrades;
                stock.LogoName = string.Join(string.Empty, stock.Name.Split(new char[] { ' ', '.' })[0]);
                stock.TradeStatus = tradeStatus;
            }

            var userAccountStatistic = this.accountStatisticRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .To<UserStatisticViewModel>()
                .FirstOrDefault();

            var dashboardViewModel = new UserDashboardViewModel
            {
                StockWatchlist = userWatchlistStocks,
                UserStatistic = userAccountStatistic,
            };

            return dashboardViewModel;
        }

        public StockBuyPercentTradesViewModel GetStockBuyPercentTrades(string ticker)
        {
            double totalBuyTrades = this.tradeRepository
                   .AllAsNoTracking()
                   .Where(x => x.Stock.Ticker == ticker && x.TradeType == TradeType.BUY.ToString())
                   .Count();

            double totalSellTrades = this.tradeRepository
                .AllAsNoTracking()
                .Where(x => x.Stock.Ticker == ticker && x.TradeType == TradeType.SELL.ToString())
                .Count();

            double totalTrades = totalBuyTrades + totalSellTrades;
            totalTrades = totalTrades == 0 ? 1 : totalTrades;

            int buyTrades = (int)Math.Round((totalBuyTrades / totalTrades) * 100);

            return new StockBuyPercentTradesViewModel { TotalBuyPercentTrades = buyTrades };
        }

        public async Task UpdateUserStocksWatchlistAsync(StockSearchResultViewModel stock, string userId)
        {
            var currentStock = this.stockRepository
                .All()
                .FirstOrDefault(x => x.Ticker == stock.Ticker);

            if (currentStock == null)
            {
                currentStock = new Stock
                {
                    Name = stock.Name,
                    Ticker = stock.Ticker,
                };

                await this.stockRepository.AddAsync(currentStock);
                await this.stockRepository.SaveChangesAsync();
            }

            var watchlistStock = this.watchlistRepository
                .All()
                .Where(x => x.UserId == userId)
                .SelectMany(x => x.Stocks)
                .ToList();

            if (watchlistStock.All(x => x.StockId != currentStock.Id))
            {
                var watchlist = this.watchlistRepository
                    .All()
                    .FirstOrDefault(x => x.UserId == userId);

                watchlist.Stocks
                    .Add(new WatchlistStocks { StockId = currentStock.Id });

                this.watchlistRepository.Update(watchlist);
                await this.watchlistRepository.SaveChangesAsync();
            }
        }

        public async Task RemoveStockFromWatchlistAsync(string input, string userId)
        {
            var currentStock = this.stockRepository
               .All()
               .FirstOrDefault(x => x.Ticker == input);

            var watchlist = this.watchlistRepository
                .All()
                .FirstOrDefault(x => x.UserId == userId);

            var watchlistStocks = this.watchlistRepository
                .All()
                .SelectMany(x => x.Stocks)
                .FirstOrDefault(x => x.StockId == currentStock.Id && x.WatchlistId == watchlist.Id);

            watchlist.Stocks.Remove(watchlistStocks);

            this.watchlistRepository.Update(watchlist);
            await this.watchlistRepository.SaveChangesAsync();
        }

        public async Task<UserPortfolioVewModel> GetPortoflioAsync(string userId)
        {
            var trades = this.tradeRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId && x.TradeStatus == TradeStatus.OPEN.ToString())
                .To<TradeInPortfolioViewModel>()
                .ToList();

            var tickers = trades.Select(x => x.StockTicker).Distinct();
            foreach (var ticker in tickers)
            {
                var stockData = await this.GetStockDataAsync(ticker);
                trades.Where(x => x.StockTicker == ticker)
                     .ToList()
                     .ForEach(x => x.CurrentPrice = stockData.Price);
            }

            foreach (var trade in trades)
            {
                trade.ProfitLossInCash = trade.TradeType == GlobalConstants.TradeTypeBuying ?
                trade.CurrentPrice - trade.OpenPrice : trade.OpenPrice - trade.CurrentPrice;

                trade.ProfitLossInPercent = trade.TradeType == GlobalConstants.TradeTypeBuying ?
                (trade.CurrentPrice - trade.OpenPrice) * 100 / trade.OpenPrice :
                (trade.OpenPrice - trade.CurrentPrice) * 100 / trade.OpenPrice;
            }

            var userStatistic = this.accountStatisticRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .To<UserStatisticViewModel>()
                .FirstOrDefault();

            var portfolioVewModel = new UserPortfolioVewModel
            {
                Trades = trades,
                UserStatistic = userStatistic,
            };

            return portfolioVewModel;
        }

        public async Task UpdateUserStockWatchlistAsync(StockSearchResultViewModel stock, string userId)
        {
            var currentStock = this.stockRepository
                .All()
                .FirstOrDefault(x => x.Ticker == stock.Ticker);

            if (currentStock == null)
            {
                currentStock = new Stock
                {
                    Name = stock.Name,
                    Ticker = stock.Ticker,
                };

                await this.stockRepository.AddAsync(currentStock);
                await this.stockRepository.SaveChangesAsync();
            }

            var watchlistStock = this.watchlistRepository
                .All()
                .Where(x => x.UserId == userId)
                .SelectMany(x => x.Stocks)
                .ToList();

            if (watchlistStock.All(x => x.StockId != currentStock.Id))
            {
                var watchlist = this.watchlistRepository
                    .All()
                    .FirstOrDefault(x => x.UserId == userId);

                watchlist.Stocks
                    .Add(new WatchlistStocks { StockId = currentStock.Id });

                this.watchlistRepository.Update(watchlist);
                await this.watchlistRepository.SaveChangesAsync();
            }
        }

        public async Task<UpdatedUserStatisticViewModel> GetActualStockDataAsync(string userId, StockTickerInputModel[] input)
        {
            var stockViewModel = new UpdatedUserStatisticViewModel();

            foreach (var asset in input)
            {
                var data = await this.GetStockDataAsync(asset.Ticker);
                var stock = new ActualStockDataViewModel
                {
                    Ticker = data.Ticker,
                    Price = data.Price,
                    Change = data.Change,
                    ChangePercent = data.ChangePercent,
                };

                stockViewModel.Stocks.Add(stock);
            }

            stockViewModel = await this.UpdateUserStatistic(userId, stockViewModel);

            return stockViewModel;
        }

        private async Task<UpdatedUserStatisticViewModel> UpdateUserStatistic(string userId, UpdatedUserStatisticViewModel updatedStockList)
        {
            var userAccountStatistic = this.accountStatisticRepository
                .All()
                .FirstOrDefault(x => x.UserId == userId);

            var allTrades = this.tradeRepository
                .All()
                .Where(x => x.UserId == userId &&
                       x.TradeStatus == TradeStatus.OPEN.ToString())
                .Select(x => new
                {
                    TradeType = x.TradeType,
                    OpenPrice = x.OpenPrice,
                    StockTicker = x.Stock.Ticker,
                })
                .ToList();

            decimal currentProfitLoss = 0;
            foreach (var stock in updatedStockList.Stocks)
            {
                var currentTrades = allTrades.Where(x => x.StockTicker == stock.Ticker);

                foreach (var trade in currentTrades)
                {
                    if (trade.TradeType == TradeType.BUY.ToString())
                    {
                        currentProfitLoss += stock.Price - trade.OpenPrice;
                    }
                    else
                    {
                        currentProfitLoss += trade.OpenPrice - stock.Price;
                    }
                }
            }

            userAccountStatistic.Profit = currentProfitLoss;

            updatedStockList.UserProfit = userAccountStatistic.Profit;
            updatedStockList.UserEquity =
                userAccountStatistic.TotalAllocated + userAccountStatistic.Available + userAccountStatistic.Profit;

            this.accountStatisticRepository.Update(userAccountStatistic);
            await this.accountStatisticRepository.SaveChangesAsync();

            return updatedStockList;
        }
    }
}
