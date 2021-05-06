document.querySelectorAll(".remove-stock-button")
    .forEach(x => x.addEventListener('click', showRemoveStockButton));

function showRemoveStockButton(event) {
    const hiddenDiv = event.target.nextElementSibling;

    if (hiddenDiv.hidden) {
        hiddenDiv.hidden = false;
    } else {
        hiddenDiv.hidden = true;
    }

    const removeButton = hiddenDiv.firstElementChild;
    removeButton.addEventListener('click', removeStockButton);
}

async function removeStockButton() {
    var stockToRemove =
        this.parentElement.parentElement.parentElement.parentElement.parentElement.querySelector('.stock-ticker').textContent;
    const token = document.querySelector('[name=__RequestVerificationToken]').value;

    const response = await fetch('api/stock/removeFromWathlist', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "X-CSRF-TOKEN": token
        },
        body: JSON.stringify({ ticker: stockToRemove })
    });

    if (!response.ok) {
        let searchResult = await response.json();
    }
    this.parentElement.parentElement.parentElement.parentElement.parentElement.remove();
}
