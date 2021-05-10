document.addEventListener('DOMContentLoaded', function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/user/index")
        .build();

    connection.on("GetStockData", function (data) {
        console.log(data);
    });

    connection.start()
        .then(() => {
            var message = 'test message';
            connection.invoke("GetActualStockData");
        })
        .catch(err => console.error(err.toString()));

});