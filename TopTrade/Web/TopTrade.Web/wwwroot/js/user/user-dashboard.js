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

        const userAvailableFunds = Number(document.querySelector('.available-amount > h5')
            .textContent.substring(1));
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
            const token = document.querySelector('[name=__RequestVerificationToken]').value;
            const endpoint = orderType.toLowerCase() === 'b' ? 'buy' : 'sell';

            const tradeDetails = {
                price: Number(orderDetailsPrice.textContent),
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
                document.querySelector('.profit-funds').textContent = result.profit;
                document.querySelector('.equity-funds').textContent = result.equity;
                document.querySelector('.close').click();
            }
        });
    })

    inputPrice.placeholder = `${stockPrice.toFixed(2)}`;
};

const cardNumber = document.querySelector('.card-number');
if (cardNumber) {
    cardNumber.addEventListener('input', function (e) {
        var cardNumber = this.value.split("-").join("");
        if (cardNumber.length > 0) {
            cardNumber = cardNumber.match(new RegExp('.{1,4}', 'g')).join("-");
        }
        this.value = cardNumber;
    });
};

document.querySelector('.desired-amount').addEventListener('input', (event) => {
    var desiredAmount = Number(event.target.value);
    var withdrawableAmount = Number(document.querySelector('#withdraw-modal form>input').value);
    var errorSpan = document.querySelector('p.withdraw-error-message');
    if (desiredAmount > withdrawableAmount) {
        errorSpan.textContent = `You can withdraw up to ${(withdrawableAmount.toFixed(2))}`
        errorSpan.hidden = false;
    } else {
        errorSpan.hidden = true;
    }
});