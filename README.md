# TopTrade

Demo project for trading stocks on NYSE. Provided data is refreshing on 2 min. series.


## Roles

* Visitor
* Administrator
* Account manager
* User with not confirmed account
* User with confirmed account

## Getting Started

Site guest (**visitor**) 
* Can only visit Info pages (*Home*, *Register*, *Login*)

Every user has to register, send documents for identity verification, in order to start trading.
After his account is confirmed by account manager, he can deposit funds to his/her account and use the full functionality of the platform.
Every user with active trades pays overnight swap fee for every trade.
Every registered user,administrator and account manager has own profile page.


**Administrator**
* Manage account managers.
* Create, update and delete account manager.
* Manage Hangfire dashboard.
* Sees statistics
	- Count of newly registered users for current month 
	- Count of total registered users
	- Count of total account managers
	- Total profit from overnight fee comision	


**Account manager** - created from administrator
* Manage new accounts. 
	- approve withdraw requests
	- approve user identity 
	- activate, deactivate their accounts
* Sees statistics
	- Count of newly registered users for current month 
	- Count of verification documents waiting approval
	- Count of withdraw requests waiting approval	
		
**User with not confirmed account**
* Can search stocks.
* Can save stocks in wathclist.
* Can update profile data.
* Can upload documents for account verification.
* Can upload avatar picture.
* Can read/search stock news.


**User with confirmed account**
* Can search stocks.
* Can save stocks in wathclist.
* Can see current stocks price (2 min series) -live updated data
* Can see live updated daily chart for every stock in watchlist
* Can see percentage of total buying/selling trades in the platform for each stock in watchlist
* Can see change in cash (from previous trading day) for each stock in watchlist
* Can see change in percent (from previous trading day) for each stock in watchlist
* Can remove stock from watchlist
* Can buy or sell stocks (depending of his account balance)
* Can see updated position profit/loss depending of market direction

* Can deposit funds
* Can withdraw funds

* Can see deposits history
* Can see withdraws history

* Can read stock news
* Can search for stock news by stock symbol or name

* Can see transactions history
* Can see chart of account performance for each month (for current year)
* Can see chart for count of buy/sell trades per month (for current year)

* Can generate report with closed positions
* Can see statistics of his paid trade and monthly fees (can choose interval for them) (graphics)
* Can see his profit/loss from closed positions (can choose interval for them) (graphics)

## Background processes

**Hangfire** has one registered job
* Every day to collect overnight swap fee commission from every active trade

**SignalR**
* Collect new stocks price data, updates stocks price in user's dashboard and portfolio pages, update user current profit/loss and equity


## Template authors

- [Nikolay Kostov](https://github.com/NikolayIT)


## Data providers

* **Alpha Vantage API** - https://www.alphavantage.co
* **NewsApi API** - https://newsapi.org/

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Author

- [Nikolay Georgiev](https://github.com/nickolay-georgiev)
