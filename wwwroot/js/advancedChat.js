var advancedChatConnection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/advancedChat")
    .build();

document.getElementById("sendMessage").disabled = true;

document.getElementById("sendMessage").addEventListener("click", function (event) {
    var sender = document.getElementById("senderEmail").value;
    var message = document.getElementById("chatMessage").value;
    var receiver = document.getElementById("receiverEmail").value;

    if (receiver.length > 0) {
        // private message
        console.log("private 1", receiver);
        advancedChatConnection.send("SendMessageToReceiver", sender, receiver, message).catch(function (err) {
            console.log(`Error: ${err}`);
        });
    } else {
        // send message for all the users
        advancedChatConnection.send("SendMessageToAll", sender, message).catch(function (err) {
            console.log(`Error: ${err}`);
        });
    }

    event.preventDefault();
});

advancedChatConnection.on("MessageReceived", function (user, message) {
    var li = document.createElement("li");
    li.textContent = `${user} - ${message}`;
    document.getElementById("messagesList").appendChild(li);
});

advancedChatConnection.start().then(function () {
    document.getElementById("sendMessage").disabled = false;
}, function () {
    console.log("smth goes wrong");
});