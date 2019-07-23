let connection = null;

setupConnection = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/notify")
        .build();

    connection.start().then(function () {
        console.log("connected");
    }).then(() => {
        connection.invoke("GetHello");
    });

    connection.on("NotifyOrders", (message) => {
        customAlert(message,5000);
    });

};

setupConnection();

function customAlert(msg, duration) {
    var styler = document.createElement("div");
    styler.setAttribute("style", "border: solid 5px Red;width:auto;height:auto;top:50%;left:40%;background-color:#444;color:Silver");
    styler.innerHTML = "<h1>" + msg + "</h1>";
    setTimeout(function () {
        styler.parentNode.removeChild(styler);
    }, duration);
    document.body.appendChild(styler);
}