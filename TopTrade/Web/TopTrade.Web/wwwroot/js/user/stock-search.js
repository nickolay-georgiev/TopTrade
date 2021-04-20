document.querySelector('.search-icon-button').addEventListener('click', stockSearch);
document.querySelector('div.close-search-bar').addEventListener('click', closeSearchBarAndClearSearchResult);
const token = document.querySelector('[name=__RequestVerificationToken]').value

async function stockSearch() {
    const searchMenu = document.querySelector('div.search-bar div.form-group');
    const inputValue = searchMenu.querySelector('input').value;

    closeSearchBarAndClearSearchResult();
    const response = await fetch('api/stockSearch/list', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "X-CSRF-TOKEN": token
        },
        body: JSON.stringify(inputValue)
    });

    let searchResult = await response.json();

    searchResult.map(async stock => {
        stock.logoName = stock.name.split(/[ .]+/)[0];
        console.log(searchResult)

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

        addStockToWatchlistDiv.addEventListener('click', addStockToWatchList.bind(this, stock));
    });
}

async function addStockToWatchList(stock) {

    const response = await fetch('api/stockSearch/stock', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "X-CSRF-TOKEN": token
        },
        body: JSON.stringify(stock.ticker)
    });
    let searchResult = await response.json();

    const logo = stock.logoName;
    const ticker = searchResult.ticker;
    const companyName = stock.name.split(/[ .]+/)[0];
    const changeInCash = searchResult.change;
    const changeInPercents = searchResult.changePercent;
    const chart = './img/chart.jpg';
    const sellPrice = searchResult.price;
    const buyPrice = searchResult.price * 1.05;

    // const i = document.createElement('i');
    // i.classList.add('fas', 'fa-ellipsis-v', 'remove-stock-button');

    const trContent = `
              <td>
                  <img class="company-logo" src="//logo.clearbit.com/${logo}.com" alt="">
                  <div>
                      <p class="mb-0 ml-2 text-uppercase font-weight-bold stock-ticker">${ticker}</p>
                      <p class="mb-0 ml-2 small stock-name">${companyName}</p>
                  </div>
              </td>
              <td>
                  <p class="mb-0 ml-2 font-weight-bold stock-price-up">${changeInCash.toFixed(2)}</p>
                  <p class="mb-0 ml-2 font-weight-bold small stock-price-down">(${changeInPercents.toFixed(2)})&#37</p>
              </td>
              <td>
                  <img class="stock-line-chart" src="${chart}" alt="">
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
                  <span>mdo</span>
                  <i class="fas fa-ellipsis-v remove-stock-button"></i>
              </td>`

    const tr = document.createElement('tr');
    tr.innerHTML = trContent;
    tr.lastElementChild.lastElementChild.addEventListener('click', showRemoveStockButton);
    document.querySelector('tbody').appendChild(tr);
}

function closeSearchBarAndClearSearchResult() {
    const searchMenu = document.querySelector('div.search-bar div.form-group');
    const closeSearchBarIcon = document.querySelector('div.close-search-bar');
    console.log(searchMenu);
    [...searchMenu.children].slice(1).forEach(x => x.remove());
    closeSearchBarIcon.setAttribute('hidden', true);
}