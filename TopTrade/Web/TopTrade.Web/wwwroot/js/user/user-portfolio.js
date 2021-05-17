document.addEventListener('DOMContentLoaded', function () {
    function initHub() {

        var allTickers = [...document.querySelectorAll('.stock-ticker')].map(x => x.textContent);
        let tickers = [...new Set(allTickers)].map(x => { return { ticker: x } });

        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/user/portfolio/index")
            .build();


        connection.on("GetStockData", function (data) {
            let updatedData = data.stocks;

            const rows = [...document.querySelectorAll('.table-div tbody tr')];

            document.querySelector('.profit-funds span').textContent = `$${data.userProfit.toFixed(2)}`;
            document.querySelector('.equity-funds > span').textContent = `${data.userEquity.toFixed(2)}`;

            for (var i = 0; i < updatedData.length; i++) {
                let currentRow = rows.find(x => x.querySelector('.stock-ticker').textContent == updatedData[i].ticker);
                currentRow.querySelectorAll('.price')
                    .forEach(x => x.textContent = updatedData[i].price.toFixed(2));

                const changeInCashElement = currentRow.querySelector('.change');
                const changeInPercentElement = currentRow.querySelector('.change-percent');

                if (updatedData[i].change < 0) {
                    changeInCashElement.classList.add('text-danger');
                    changeInPercentElement.classList.add('text-danger');
                    changeInCashElement.classList.remove('text-success');
                    changeInPercentElement.classList.remove('text-success');
                } else {
                    changeInCashElement.classList.add('text-success');
                    changeInPercentElement.classList.add('text-success');
                    changeInCashElement.classList.remove('text-danger');
                    changeInPercentElement.classList.remove('text-danger');
                }
                currentRow.querySelector('.change').textContent = updatedData[i].change.toFixed(2);
                currentRow.querySelector('.change-percent').textContent = `(${updatedData[i].changePercent.toFixed(2)}%)`;
            }
        });

        connection.start()
            .then(() => {
                setInterval(function () {
                    if (tickers) {
                        connection.invoke("GetUpdateForStockPrice", tickers);
                    }
                }, 60000);
            })
            .catch(err => console.error(err.toString()));
    }
    initHub();
});