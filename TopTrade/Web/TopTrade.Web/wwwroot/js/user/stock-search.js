window.onload = function () {

    function init() {
        let stockName;
        let stockTicker;
        let stockPrice;
        let stockQuantity;
        let domElement;

        var lastClick = 0;
        var delay = 20;

        const domElements = {
            token: () => document.querySelector('[name=__RequestVerificationToken]').value,
            searchIconButton: () => document.querySelector('.search-icon-button'),
            searchMenu: () => document.querySelector('div.search-bar div.form-group'),
            searchInputValue: () => domElements.searchMenu().querySelector('input'),
            searchMenuSpinner: () => document.querySelector('div.spinner'),
            searchMenuIcon: () => document.querySelector('i.fa-search'),
            closeSearchBar: () => document.querySelector('div.close-search-bar'),
            configOrderInputPrice: () => document.querySelector('.input-stock-price'),
            configOrderQuantity: () => document.querySelector('.stock-quantity'),
            configOrderTotalAmount: () => document.querySelector('.total-amount'),
            configOrderStockName: () => document.querySelector('.order-stock-name'),
            configOrderErrorMessage: () => document.querySelector('.trade-error'),
            configOrderSummaryPrice: () => document.querySelector('span.order-details-price'),
            userAvailableFunds: () => document.querySelector('.available-funds > span'),
            userTotalAllocatedFunds: () => document.querySelector('.allocated-funds > span'),
            userProfit: () => document.querySelector('.profit-funds span'),
            userEquity: () => document.querySelector('.equity-funds > span'),
        };

        const endpoints = {
            stockSearch: () => 'searchStock',
            stockList: () => 'searchList',
            stockBuyPercent: () => 'buyPercent',
        }

        domElements.searchIconButton().addEventListener('click', stockSearch);
        domElements.closeSearchBar().addEventListener('click', closeSearchBarAndClearSearchResult);

        async function stockSearch() {
            const inputValue = domElements.searchInputValue().value.trim();

            if (!inputValue) { return };

            activateSpinner();
            //closeSearchBarAndClearSearchResult();

            const response = await makeAjax(endpoints.stockList(), { ticker: inputValue });
            let searchResult = await response.json();

            searchResult.map(stock => {
                stock.logoName = stock.name.split(/[ .]+/)[0];

                const closeSearchBarIcon = document.querySelector('div.close-search-bar');
                closeSearchBarIcon.removeAttribute("hidden");

                const logo = stock.logoName;

                const searchResultDiv = document.createElement('div');
                searchResultDiv.classList.add('search-result-div');

                const stockNameDiv = document.createElement('div');
                stockNameDiv.classList.add('d-flex', 'flex-row');

                const stockLogo = document.createElement('img');
                stockLogo.src = `//logo.clearbit.com/${logo}.com`;
                stockLogo.classList.add('company-logo');

                const stockDiv = document.createElement('div');
                stockDiv.classList.add('ml-2');

                const stockTickerParagraph = document.createElement('p');
                stockTickerParagraph.textContent = stock.ticker;
                stockTickerParagraph.classList.add('mb-0');
                stockTickerParagraph.classList.add('font-weight-bold');

                const stockNameParagraph = document.createElement('p');
                stockNameParagraph.textContent = stock.name;
                stockNameParagraph.classList.add('mb-0');
                stockNameParagraph.classList.add('small');

                const addStockToWatchlistDiv = document.createElement('div');
                addStockToWatchlistDiv.style.cursor = "pointer";
                addStockToWatchlistDiv.style.background = 'green';
                addStockToWatchlistDiv.style.color = 'white';

                const addIcon = document.createElement('i');
                addIcon.classList.add('fas', 'fa-plus');

                const icon = document.createElement('i');
                icon.classList.add('fas', 'fa-check');

                addStockToWatchlistDiv.append(icon);

                stockDiv.append(stockTickerParagraph, stockNameParagraph);
                stockNameDiv.append(stockLogo, stockDiv);

                searchResultDiv.append(stockNameDiv, addStockToWatchlistDiv);
                domElements.searchMenu().append(searchResultDiv);
                removeSpinner();

                addStockToWatchlistDiv.addEventListener('click', addStockToWatchList.bind(this, stock));
            });
        }

        async function addStockToWatchList(stock) {

            stockName = stock.name;
            stockTicker = stock.ticker;

            const htmlElement = [...document.querySelectorAll('.table-div tbody tr')]
                .find(x => x.querySelector('.stock-ticker').textContent == stockTicker);

            if (htmlElement) { return };

            closeSearchBarAndClearSearchResult();

            const response = await makeAjax(endpoints.stockSearch(), stock);
            const searchResult = await response.json();

            const dataset = searchResult.dataSet;
            const logo = stock.logoName;
            const ticker = stockTicker;
            const companyName = stockName;
            const changeInCash = searchResult.change;
            const changeInPercents = searchResult.changePercent;
            const sellPrice = searchResult.price;
            const buyPrice = searchResult.price * 1.005;
            stockPrice = sellPrice;

            const color = 0 > changeInCash ? 'stock-price-down' : 'stock-price-up';

            const trContent = `
              <td>
                  <img class="company-logo" src="//logo.clearbit.com/${logo}.com" alt="">
                  <div class="d-flex justify-content-between align-items-center w-100">
                  <div>
                      <p class="mb-0 ml-2 text-uppercase font-weight-bold stock-ticker">${ticker}</p>
                      <p class="mb-0 ml-2 small stock-name">${companyName}</p>
                  </div>
                  <div class="open-position-icon" hidden>
                      <a href="/User/Portfolio" asp-action="Index">
                          <i class="fas fa-chart-pie"></i>
                      </a>
                  </div>
                  </div>
              </td>
              <td>
                  <p class="mb-0 ml-2 font-weight-bold ${color}">${changeInCash.toFixed(2)}</p>
                  <p class="mb-0 ml-2 font-weight-bold small ${color}">(${changeInPercents.toFixed(2)})&#37</p>
              </td>
              <td style="width: 230px">
                  <div class="stock-chart" style="height: 60px;">
                       <canvas id="line"></canvas>
                  </div>
              </td>
              <td>
                  <div class="input-group sell-button order-type-button">
                      <div class="input-group-prepend">
                          <div class="input-group-text font-weight-bold" id="btnGroupAddon">S</div>
                      </div>
                      <button class="form-control font-weight-bold price" data-target="#buy-button"  data-toggle="modal">${sellPrice.toFixed(2)}</button>
                  </div>
              </td>
              <td>
                  <div class="input-group buy-button order-type-button">
                      <div class="input-group-prepend">
                          <div class="input-group-text font-weight-bold" id="btnGroupAddon">B</div>
                      </div>
                      <button class="form-control font-weight-bold" data-target="#buy-button"
                          data-toggle="modal">${buyPrice.toFixed(2)}</button>
                  </div>
              </td>
              <td>
                 <div class="row">
                     <div class="col-10">
                         <p class="mb-0 total-buyers">90% <span class="small">BUYING</span></p>
                         <div class="progress h-50">
                             <div class="progress-bar bg-success" role="progressbar" style="width: 70%" aria-valuenow="@stock.BuyPercent" aria-valuemin="0" aria-valuemax="100"></div>
                             <div class="progress-bar bg-danger" role="progressbar" style="width: 30%" aria-valuenow="@(100 - stock.BuyPercent)" aria-valuemin="0" aria-valuemax="100"></div>
                         </div>
                     </div>
                     <div class="position-relative">
                         <i class="fas fa-ellipsis-v remove-stock-button">
                         </i>
                          <div class="remove-stock position-absolute" hidden>
                              <div class="d-flex align-items-md-center px-3 py-2 shadow" hidden>
                                  <i class="far fa-calendar-times"></i>
                                  <button class="btn btn-light">Remove</button>
                              </div>
                          </div>
                     </div>
                 </div>
             </td>`

            const tr = document.createElement('tr');
            tr.innerHTML = trContent;

            domElement = tr;

            await getBuyPercent(ticker, domElement);
            generateChart(dataset, tr);

            tr.querySelector('i.remove-stock-button').addEventListener('click', showRemoveStockButton);
            document.querySelector('tbody').appendChild(tr);

            [...tr.querySelectorAll('.order-type-button')].forEach(x => {
                x.addEventListener('click', configureOrder);
            });
        };

        [...document.querySelectorAll('.order-type-button')].forEach(x => {
            x.addEventListener('click', configureOrder);
        });

        function configureOrder(event) {
            event.preventDefault();
            domElement = event.target.parentElement.parentElement.parentElement;
            domElements.configOrderQuantity().value = '';
            domElements.configOrderErrorMessage().hidden = true;
            domElements.configOrderSummaryPrice().textContent = 0;

            if (!isNaN(event.target.parentElement.lastElementChild.textContent)) {
                stockTicker = event.target.parentElement.parentElement.parentElement.querySelector('.stock-ticker').textContent;
                stockName = event.target.parentElement.parentElement.parentElement.querySelector('.stock-name').textContent;
            }

            const inputPrice = domElements.configOrderInputPrice();
            const totalAmount = domElements.configOrderTotalAmount();
            const stockQuantityElement = domElements.configOrderQuantity();
            const companyLogo = `//logo.clearbit.com/${stockName.split(/[ .]+/)[0]}.com`;

            const orderStockName = domElements.configOrderStockName();
            orderStockName.textContent = stockName;

            let orderType;
            if (event.target.classList.contains('configure-order-button')) {
                orderType = event.target.textContent[0].toLowerCase();
                if (isNaN(event.target.textContent)) {
                    stockPrice = Number(event.target.parentElement.parentElement.nextElementSibling.querySelector('.input-stock-price').placeholder);
                }
            } else {
                orderType = event.target.previousElementSibling.firstElementChild.textContent;
                stockPrice = Number(event.target.textContent);
            }

            if (orderType.toLowerCase() == 'b') {
                [...document.querySelectorAll('.order-type')].forEach(x => {
                    x.textContent = 'buy';
                });
                document.querySelector(".order-buy-btn").classList.add('active');
                document.querySelector(".order-sell-btn").classList.remove('active');
            } else {
                [...document.querySelectorAll('.order-type')].forEach(x => {
                    x.textContent = 'sell';
                });
                document.querySelector(".order-sell-btn").classList.add('active');
                document.querySelector(".order-buy-btn").classList.remove('active');
            }

            document.querySelector('.order-company-logo').src = companyLogo;
            var currentQuantity = domElements.configOrderQuantity().value;
            if (!currentQuantity) {
                currentQuantity.value = 0;
            };

            domElements.configOrderTotalAmount().placeholder = currentQuantity * stockPrice;
            document.querySelector('span.order-details-price').textContent = currentQuantity * stockPrice;

            [...document.querySelectorAll('.order-stock-ticker')].forEach(x => {
                x.textContent = stockTicker;
            });

            stockQuantityElement.addEventListener('input', (e) => {
                stockQuantity = Number(e.target.value);
                if (stockQuantity < 0) { return };

                const executeOrderButton = document.querySelector('.execute-order-button');
                executeOrderButton.removeAttribute('disabled');

                const totalPrice = stockPrice * stockQuantity;
                totalAmount.placeholder = `$ ${totalPrice.toFixed(2)}`

                const orderDetailsPrice = domElements.configOrderSummaryPrice();
                orderDetailsPrice.textContent = `${totalPrice.toFixed(2)}`;

                const orderDetailsQuantity = document.querySelector('.order-details-quantity');
                orderDetailsQuantity.textContent = stockQuantity;

                if (stockQuantity == 0) { executeOrderButton.disabled = true; }

                const userAvailableFunds = Number(document.querySelector('.available-amount > h5 > span').textContent);
                const error = domElements.configOrderErrorMessage();
                if (userAvailableFunds < totalPrice) {
                    error.removeAttribute('hidden');
                    error.textContent = `Deposit $${(totalPrice - userAvailableFunds).toFixed(2)} in order to set this order`;
                    executeOrderButton.disabled = true;
                } else {
                    error.hidden = true;
                }

                executeOrderButton.addEventListener('click', () => {

                    const endpoint = executeOrderButton.firstElementChild.textContent;
                    const tradeDetails = {
                        price: stockPrice,
                        quantity: stockQuantity,
                        ticker: stockTicker,
                        tradeType: endpoint
                    };

                    doClick();
                    async function doClick() {
                        if (lastClick >= (Date.now() - delay)) { return }
                        lastClick = Date.now();

                        let response = await makeAjax("trade", tradeDetails);
                        let result = await response.json();

                        if (response.ok) {
                            domElements.userAvailableFunds().textContent = result.available.toFixed(2);
                            domElements.userTotalAllocatedFunds().textContent = result.totalAllocated.toFixed(2);
                            domElements.userProfit().textContent = `$${result.profit.toFixed(2)}`;
                            domElements.userEquity().textContent = result.equity.toFixed(2);
                            domElements.configOrderQuantity().value = '';

                            error.hidden = true;
                            orderDetailsPrice.textContent = 0;
                            document.querySelector('.close').click();

                            var htmlElement = [...document.querySelectorAll('.table-div tbody tr')]
                                .find(x => x.querySelector('.stock-ticker').textContent == stockTicker);

                            htmlElement.querySelector('.open-position-icon').hidden = false;
                            getBuyPercent(stockTicker, htmlElement);
                        }
                    }
                });
            })

            inputPrice.placeholder = `${stockPrice.toFixed(2)}`;
        };

        function closeSearchBarAndClearSearchResult() {
            [...domElements.searchMenu().children].slice(1).forEach(x => x.remove());
            domElements.closeSearchBar().setAttribute('hidden', true);
            domElements.searchInputValue().value = '';
        }

        function activateSpinner() {
            domElements.searchMenuSpinner().style.display = 'block';
            domElements.searchMenuIcon().style.display = 'none';
        }

        function removeSpinner() {
            domElements.searchMenuSpinner().style.display = 'none';
            domElements.searchMenuIcon().style.display = 'block';
        }

        async function getBuyPercent(ticker, htmlElement) {
            const response = await makeAjax(endpoints.stockBuyPercent(), { ticker });
            if (response.ok) {
                const result = await response.json();

                htmlElement.querySelector('.total-buyers').innerHTML = `${result.totalBuyPercentTrades}% <span class="small">BUYING</span>`;

                htmlElement.querySelector('div.progress-bar.bg-success').style.width = `${result.totalBuyPercentTrades}%`;
                htmlElement.querySelector('div.progress-bar.bg-success').setAttribute('aria-valuenow', `${result.totalBuyPercentTrades}`);

                htmlElement.querySelector('div.progress-bar.bg-danger').style.width = `${100 - result.totalBuyPercentTrades}%`;
                htmlElement.querySelector('div.progress-bar.bg-danger').setAttribute('aria-valuenow', `${100 - result.totalBuyPercentTrades}`);
            }
        }

        async function makeAjax(endpoint, argument) {
            return await fetch(`api/stock/${endpoint}`, {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    "X-CSRF-TOKEN": domElements.token(),
                },
                body: JSON.stringify(argument)
            });
        }

        function generateChart(dataset, selector) {

            var ctx = selector.querySelector('#line').getContext('2d');
            var chart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",],
                    datasets: [{
                        backgroundColor: 'lightblue',
                        data: dataset,
                        pointRadius: 0,
                    }]
                },
                options: {
                    plugins: {
                        elements: {
                            point: {
                                radius: 0
                            }
                        },
                        legend: {
                            display: false
                        },
                        datalabels: {
                            display: false,
                        },
                        hidden: true,
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                display: false
                            },
                            gridLines: {
                                display: false,
                            },
                        }],
                        xAxes: [{
                            ticks: {
                                display: false
                            },
                            gridLines: {
                                display: false,
                            },
                        }],
                    },
                    legend: {
                        display: false
                    },
                },
            });
        }

    }
    init();
}