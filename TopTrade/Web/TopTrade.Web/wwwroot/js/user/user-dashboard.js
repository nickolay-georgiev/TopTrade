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
    })

    inputPrice.placeholder = `${stockPrice.toFixed(2)}`
}


//document.querySelector('.make-deposit-button').addEventListener('click', makeDeposit);

//function makeDeposit(event) {
//    event.preventDefault();

//    const depositAmount = document.querySelector('.deposit-amount').value.trim();
//    const cardNumber = document.querySelector('.card-number').value;
//    const cardExpiryDate = document.querySelector('.card-expiry-date').value;
//    const cardCvvNumber = document.querySelector('.card-cvv-number').value;
//    const cardFirstName = document.querySelector('.card-first-name').value.trim();
//    const cardMiddleName = document.querySelector('.card-middle-name').value.trim();
//    const cardLastName = document.querySelector('.card-last-name').value.trim();
//    const token = document.querySelector('[name=__RequestVerificationToken]').value

//    var card = {
//        depositAmount, cardNumber, cardExpiryDate, cardCvvNumber, cardFirstName, cardMiddleName, cardLastName
//    }

//    fetch('api/deposit', {
//        method: 'POST',
//        headers: {
//            'Accept': 'application/json',
//            'Content-Type': 'application/json',
//            "X-CSRF-TOKEN": token
//        },
//        body: JSON.stringify(card)
//    }).then(res => {
//        console.log(res);
//    })

//}

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

//document.querySelector('.card-expiry-date').addEventListener('input', function (e) {
//    var expiryDate = this.value;
//    if (expiryDate.length == 2) {
//        expiryDate += ' / ';
//    }
//    this.value = expiryDate;
//});