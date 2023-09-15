var chatConnection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/chat")
    .build();

document.getElementById("sendMessage").disabled = true;

document.getElementById("sendMessage").addEventListener("click", function (event) {
    var sender = document.getElementById("senderEmail").value;
    var message = document.getElementById("chatMessage").value;

    // send message for all the users
    chatConnection.send("SendMessageToAll", sender, message);

    event.preventDefault();
});

chatConnection.on("MessageReceived", function(user, message) {
    var li = document.createElement("li");
    li.textContent = `${user} - ${message}`;
    document.getElementById("messagesList").appendChild(li);
});

chatConnection.start().then(function () {
    document.getElementById("sendMessage").disabled = false;
}, function () {
    console.log("smth goes wrong");
});