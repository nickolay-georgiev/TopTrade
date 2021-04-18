document.querySelectorAll(".remove-stock-button")
    .forEach(x => x.addEventListener('click', showRemoveStockButton));

function showRemoveStockButton(event) {
    if (event.target.tagName.toLowerCase() === 'i'
        && event.target.childElementCount > 0) {
        [...event.target.children].forEach(x => x.remove());
        return;
    }
    if (event.target.tagName.toLowerCase() === 'div') { return };

    const div = document.createElement('div');
    div.classList.add('remove-stock-div');

    const span = document.createElement('span');
    span.textContent = 'Remove';

    div.appendChild(span);
    event.target.appendChild(div);

    div.addEventListener('click', removeStockButton);
}

function removeStockButton() {
    //TODO write functionality to remove stock from watchlist
    this.parentElement.parentElement.parentElement.remove();
}

document.querySelector('.search-icon-button').addEventListener('click', stockSearch);

async function stockSearch(event) {
    const token = document.querySelector('[name=__RequestVerificationToken]').value

    let response  = await fetch('api/stockSearch', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "X-CSRF-TOKEN": token
        },
        body: JSON.stringify("test")
    });

    let result = await response.json();

    console.log(result)

    const searchMenu = document.querySelector('div.search-bar div.form-group');
    const closeSearchBarIcon = document.querySelector('div.close-search-bar');
    closeSearchBarIcon.removeAttribute("hidden");

    const input = searchMenu.querySelector('input');

    const logo = 'spotify';

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
    stockTickerParagraph.textContent = 'AAPL';
    stockTickerParagraph.classList.add('mb-0');
    stockTickerParagraph.classList.add('font-weight-bold');

    const stockNameParagraph = document.createElement('p');
    stockNameParagraph.textContent = 'APPLE';
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

    addStockToWatchlistDiv.addEventListener('click', addStockToWatchList);

    stockDiv.append(stockTickerParagraph, stockNameParagraph);
    stockNameDiv.append(stockLogo, stockDiv);

    searchResultDiv.append(stockNameDiv, addStockToWatchlistDiv);
    searchMenu.append(searchResultDiv);
}

function addStockToWatchList(event) {
    const logo = 'spotify';
    const ticker = 'RR.L';
    const companyName = 'Rolls-Royce';
    const changeInCash = 25;
    const changeInPercents = 4;
    const chart = './img/chart.jpg';
    const sellPrice = 10;
    const buyPrice = 20;

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

document.querySelector('div.close-search-bar').addEventListener('click', closeSearchBar);

function closeSearchBar(event) {
    const searchMenu = document.querySelector('div.search-bar div.form-group');
    const closeSearchBarIcon = document.querySelector('div.close-search-bar');
    console.log(searchMenu);
    [...searchMenu.children].slice(1).forEach(x => x.remove());
    closeSearchBarIcon.setAttribute('hidden', true);
}


[...document.querySelectorAll('.order-type-button')].forEach(x => {
    x.addEventListener('click', configureOrder);
})
function configureOrder(event) {
    let stockPrice;
    const inputPrice = document.querySelector('.input-stock-price');
    const totalAmount = document.querySelector('.total-amount');
    const stockQuantity = document.querySelector('.stock-quantity');
    const stockTicker = document.querySelector('.stock-ticker').textContent;
    const stockName = document.querySelector('.stock-name').textContent;
    const companyLogo = document.querySelector('.company-logo').src;

    const orderStockName = document.querySelector('.order-stock-name');
    orderStockName.textContent = stockName;

    let orderType;
    if (event.target.classList.contains('configure-order-button')) {
        event.preventDefault()
        orderType = event.target.textContent[0].toLowerCase();
        stockPrice = Number(document.querySelector('.price').textContent);
    } else {
        orderType = event.target.previousElementSibling.firstElementChild.textContent;
        stockPrice = stockPrice = Number(event.target.textContent);
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

        const orderDetailsPrice = document.querySelector('.order-details-price');
        orderDetailsPrice.textContent = `${totalPrice.toFixed(2)}`;

        const orderDetailsQuantity = document.querySelector('.order-details-quantity');
        orderDetailsQuantity.textContent = quantity;
        if (quantity == 0) { executeOrderButton.disabled = true; }
    })

    inputPrice.placeholder = `${stockPrice.toFixed(2)}`
}


document.querySelector('.make-deposit-button').addEventListener('click', makeDeposit);

function makeDeposit(event) {
    event.preventDefault();

    const depositAmount = document.querySelector('.deposit-amount').value.trim();
    const cardNumber = document.querySelector('.card-number').value;
    const cardExpiryDate = document.querySelector('.card-expiry-date').value;
    const cardCvvNumber = document.querySelector('.card-cvv-number').value;
    const cardFirstName = document.querySelector('.card-first-name').value.trim();
    const cardMiddleName = document.querySelector('.card-middle-name').value.trim();
    const cardLastName = document.querySelector('.card-last-name').value.trim();
    const token = document.querySelector('[name=__RequestVerificationToken]').value

    var card = {
        depositAmount, cardNumber, cardExpiryDate, cardCvvNumber, cardFirstName, cardMiddleName, cardLastName
    }

    fetch('api/deposit', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "X-CSRF-TOKEN": token
        },
        body: JSON.stringify(card)
    }).then(res => {
        console.log(res);
    })

}

document.querySelector('.card-number').addEventListener('input', function (e) {
    var cardNumber = this.value.split("-").join("");
    if (cardNumber.length > 0) {
        cardNumber = cardNumber.match(new RegExp('.{1,4}', 'g')).join("-");
    }
    this.value = cardNumber;
});

document.querySelector('.card-expiry-date').addEventListener('input', function (e) {
    var expiryDate = this.value;
    if (expiryDate.length == 2) {
        expiryDate += ' / ';
    }
    this.value = expiryDate;
});
