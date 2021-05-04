document.querySelector('.search-icon-button').addEventListener('click', stockSearch);
document.querySelector('div.close-search-bar').addEventListener('click', closeSearchBarAndClearSearchResult);
const token = document.querySelector('[name=__RequestVerificationToken]').value;

async function stockSearch() {
    const searchMenu = document.querySelector('div.search-bar div.form-group');
    const inputValue = searchMenu.querySelector('input').value.trim();

    if (!inputValue) { return };

    activateSpinner();
    closeSearchBarAndClearSearchResult();
    const response = await fetch('api/stock/searchList', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "X-CSRF-TOKEN": token
        },
        body: JSON.stringify({ ticker: inputValue })
    });

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
        searchMenu.append(searchResultDiv);
        removeSpinner();

        addStockToWatchlistDiv.addEventListener('click', addStockToWatchList.bind(this, stock));
    });
}

async function addStockToWatchList(stock) {

    stock.name = stock.logoName;
    const response = await fetch('api/stock/searchStock', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "X-CSRF-TOKEN": token
        },
        body: JSON.stringify(stock)
    });

    let searchResult = await response.json();

    const dataset = searchResult.dataSet;
    const logo = stock.logoName;
    const ticker = searchResult.ticker;
    const companyName = stock.logoName;
    const changeInCash = searchResult.change;
    const changeInPercents = searchResult.changePercent;
    const sellPrice = searchResult.price;
    const buyPrice = searchResult.price * 1.005;

    var color = 0 > changeInCash ? 'stock-price-down' : 'stock-price-up';

    const trContent = `
              <td>
                  <img class="company-logo" src="//logo.clearbit.com/${logo}.com" alt="">
                  <div>
                      <p class="mb-0 ml-2 text-uppercase font-weight-bold stock-ticker">${ticker}</p>
                      <p class="mb-0 ml-2 small stock-name">${companyName}</p>
                  </div>
              </td>
              <td>
                  <p class="mb-0 ml-2 font-weight-bold ${color}">${changeInCash.toFixed(2)}</p>
                  <p class="mb-0 ml-2 font-weight-bold small ${color}">(${changeInPercents.toFixed(2)})&#37</p>
              </td>
              <td style="width: 230px">
                  <div class="stock-chart">
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
                             <button class="btn btn-light">Remove</button>
                         </div>
                     </div>
                 </div>
             </td>`

    const tr = document.createElement('tr');
    tr.innerHTML = trContent;

    await getBuyPercent(ticker, tr);
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
    let stockPrice;
    const inputPrice = document.querySelector('.input-stock-price');
    const totalAmount = document.querySelector('.total-amount');
    const stockQuantity = document.querySelector('.stock-quantity');
    const stockTicker = document.querySelector('.stock-ticker').textContent;
    const stockName = document.querySelector('.stock-name').textContent;
    const companyLogo = document.querySelector('.company-logo').src;

    const orderStockName = document.querySelector('.order-stock-name');
    orderStockName.textContent = stockName;

    let test = document.querySelector('.price').textContent;

    let orderType;
    if (event.target.classList.contains('configure-order-button')) {
        orderType = event.target.textContent[0].toLowerCase();
        if (isNaN(event.target.textContent)) {
            stockPrice = Number(event.target.parentElement.parentElement.nextElementSibling.querySelector('.input-stock-price').placeholder);
        } else {
            stockPrice = Number(document.querySelector('.price').textContent);
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
    var currentQuantity = document.querySelector('.stock-quantity').value;
    if (!currentQuantity) {
        currentQuantity.value = 0;
    };

    document.querySelector('.total-amount').placeholder = currentQuantity * stockPrice;
    document.querySelector('span.order-details-price').textContent = currentQuantity * stockPrice;

    [...document.querySelectorAll('.order-stock-ticker')].forEach(x => {
        x.textContent = stockTicker;
    });

    stockQuantity.addEventListener('input', (e) => {
        const quantity = Number(e.target.value);
        if (quantity < 0) { return };

        const executeOrderButton = document.querySelector('.execute-order-button');
        executeOrderButton.removeAttribute('disabled');

        const totalPrice = stockPrice * quantity;
        totalAmount.placeholder = `$ ${totalPrice.toFixed(2)}`

        const orderDetailsPrice = document.querySelector('span.order-details-price');
        orderDetailsPrice.textContent = `${totalPrice.toFixed(2)}`;

        const orderDetailsQuantity = document.querySelector('.order-details-quantity');
        orderDetailsQuantity.textContent = quantity;

        if (quantity == 0) { executeOrderButton.disabled = true; }

        const userAvailableFunds = Number(document.querySelector('.available-amount > h5 > span').textContent);
        const error = document.querySelector('.trade-error');
        if (userAvailableFunds < totalPrice) {
            error.removeAttribute('hidden');
            error.textContent = `Deposit $${(totalPrice - userAvailableFunds).toFixed(2)} in order to set this order`;
            executeOrderButton.disabled = true;
        } else {
            error.hidden = true;
        }

        executeOrderButton.addEventListener('click', async (event) => {
            event.preventDefault();
            const endpoint = executeOrderButton.firstElementChild.textContent;

            const tradeDetails = {
                price: Number(inputPrice.placeholder),
                quantity: quantity,
                ticker: stockTicker,
                tradeType: endpoint
            };

            const response = await fetch(`api/stock/${endpoint}`, {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    "X-CSRF-TOKEN": token
                },
                body: JSON.stringify(tradeDetails)
            });

            let result = await response.json();

            if (response.ok) {
                document.querySelector('.available-funds').textContent = result.available;
                document.querySelector('.allocated-funds').textContent = result.totalAllocated;
                document.querySelector('.profit-funds span').textContent = `$${result.profit.toFixed(2)}`;
                document.querySelector('.equity-funds').textContent = result.equity;
                document.querySelector('.close').click();

                const htmlElement = [...document.querySelectorAll('tbody tr')].reduce((acc, value) => {
                    var currentTicker = value.querySelector('.stock-ticker').textContent;
                    if (currentTicker == stockTicker) {
                        return acc.push(value);
                    }
                });

                getBuyPercent(stockTicker, htmlElement);
            }
        });
    })

    inputPrice.placeholder = `${stockPrice.toFixed(2)}`;
};

function closeSearchBarAndClearSearchResult() {
    const searchMenu = document.querySelector('div.search-bar div.form-group');
    const closeSearchBarIcon = document.querySelector('div.close-search-bar');
    [...searchMenu.children].slice(1).forEach(x => x.remove());
    closeSearchBarIcon.setAttribute('hidden', true);
}

function activateSpinner() {
    const spinner = document.querySelector('div.spinner').style.display = 'block';
    const search = document.querySelector('i.fa-search').style.display = 'none';

}

function removeSpinner() {
    const spinner = document.querySelector('div.spinner').style.display = 'none';
    const search = document.querySelector('i.fa-search').style.display = 'block';
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

async function getBuyPercent(ticker, htmlElement) {

    const response = await fetch(`api/stock/buyPercent`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "X-CSRF-TOKEN": token
        },
        body: JSON.stringify({ ticker })
    });

    if (response.ok) {

        const result = await response.json();

        htmlElement.querySelector('.total-buyers').innerHTML = `${result.totalBuyPercentTrades}% <span class="small">BUYING</span>`;

        htmlElement.querySelector('div.progress-bar.bg-success').style.width = `${result.totalBuyPercentTrades}%`;
        htmlElement.querySelector('div.progress-bar.bg-success').setAttribute('aria-valuenow', `${result.totalBuyPercentTrades}`);

        htmlElement.querySelector('div.progress-bar.bg-danger').style.width = `${100 - result.totalBuyPercentTrades}%`;
        htmlElement.querySelector('div.progress-bar.bg-danger').setAttribute('aria-valuenow', `${100 - result.totalBuyPercentTrades}`);
    }
}