let connection = null;

setupConnection = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/notify")
        .build();

    connection.start().then(function () {
        console.log("connected");
    });

    connection.on("NotifyOrders", (message) => {
        customAlert(message,5000);
    });

};

setupConnection();

function customAlert(msg, duration) {
    var styler = document.createElement("div");
    styler.classList.add("justify-content-around");
    styler.classList.add("container");
    styler.classList.add("rounded");
    styler.classList.add("border-info");  
    styler.setAttribute("style", "width:auto;height:auto;top:50%;left:40%;background-color:#2196F3;color:white;text-align: center");
    styler.innerHTML = "<h1>" + msg + "</h1>";
    setTimeout(function () {
        styler.parentNode.removeChild(styler);
    }, duration);
    document.body.appendChild(styler);
}