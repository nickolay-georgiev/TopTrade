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


