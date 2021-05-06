//function init() {

//    var lastClick = 0;
//    var delay = 20;

//    document.querySelectorAll('.close-trade-button')
//        .forEach(x => x.addEventListener('click', closeTrade));

//    async function closeTrade(event) {
//        event.preventDefault();

//        if (lastClick >= (Date.now() - delay)) { return }
//        lastClick = Date.now();

//        const currentTableRow = event.target.parentElement.parentElement;
//        const ticker = currentTableRow.querySelector('.stock-ticker').textContent;
//        const token = document.querySelector('[name=__RequestVerificationToken]').value;

//        const response = await fetch('api/stock/closeTrade', {
//            method: 'POST',
//            headers: {
//                'Accept': 'application/json',
//                'Content-Type': 'application/json',
//                "X-CSRF-TOKEN": token
//            },
//            body: JSON.stringify({ ticker })
//        });

//        if (response.ok) {
//            currentTableRow.remove();
//            const result = await respone.json();
//        };
//    };
//};

//init();