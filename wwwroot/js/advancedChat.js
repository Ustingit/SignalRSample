var advancedChatConnection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/advancedChat")
    .withAutomaticReconnect([0, 1000, 5000, null])
    .build();

advancedChatConnection.on("ReceiveUserConnected", function (userId, userName, isReConnect) {
    if (!isReConnect) {
        addMessage(`${userName} is online`);
    }
});

advancedChatConnection.start().then(function () {
    console.log("connection is established");
}, function () {
    console.log("smth goes wrong");
});

function addMessage(message) {
    if (message == null || message === '') {
        return;
    }

    var messagesList = document.getElementById("messagesList");
    var li = document.createElement("li");
    li.innerHTML = message;
    messagesList.appendChild(li);
};