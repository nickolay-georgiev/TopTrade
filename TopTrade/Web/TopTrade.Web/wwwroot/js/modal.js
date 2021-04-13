document.querySelectorAll(".remove-stock-button")
    .forEach(x => x.addEventListener('click', showRemoveStockButton))

function showRemoveStockButton(event) {

    if (event.target.tagName.toLowerCase() === 'i'
        && event.target.childElementCount > 0) {
        [...event.target.children].forEach(x => x.remove());
        return;
    }
    if (event.target.tagName.toLowerCase() === 'div') { return };

    const div = document.createElement('div');
    div.classList.add('remove-stock-div')
    div.textContent = 'Remove';


    event.target.appendChild(div);

    div.addEventListener('click', removeStockButton)
}

function removeStockButton() {
    //TODO write functionality to remove stock from watchlist
    console.log(1);
}


document.querySelector('.search-icon-button').addEventListener('click', searchStock)

function searchStock(event) {
    const searchMenu = document.querySelector('div.search-bar div.form-group');
    const closeSearchBarIcon = document.querySelector('div.close-search-bar');
    closeSearchBarIcon.removeAttribute("hidden");

    const input = searchMenu.querySelector('input');


    const searchResultDiv = document.createElement('div');
    searchResultDiv.classList.add('search-result-div');


    const stockNameDiv = document.createElement('div');

    const stockTickerSpan = document.createElement('span');
    stockTickerSpan.textContent = 'AAPL';

    const stockNameSpan = document.createElement('span');
    stockNameSpan.textContent = 'APPLE';

    const addStockToWatchlistDiv = document.createElement('div');
    const addIcon = document.createElement('i');
    addIcon.classList.add('fas', 'fa-plus');
    addStockToWatchlistDiv.append(addIcon)


    stockNameDiv.append(stockTickerSpan, stockNameSpan);


    searchResultDiv.append(stockNameDiv, addStockToWatchlistDiv);

    searchMenu.append(searchResultDiv);
}

document.querySelector('div.close-search-bar').addEventListener('click', closeSearchBar);

function closeSearchBar(event) {
    const searchMenu = document.querySelector('div.search-bar div.form-group');
    const closeSearchBarIcon = document.querySelector('div.close-search-bar');
    console.log(searchMenu);
    [...searchMenu.children].slice(1).forEach(x => x.remove());
    closeSearchBarIcon.setAttribute('hidden', true);
}
