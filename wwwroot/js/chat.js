var chatConnection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/chat")
    .build();

document.getElementById("sendMessage").disabled = true;

document.getElementById("sendMessage").addEventListener("click", function (event) {
    var sender = document.getElementById("senderEmail").value;
    var message = document.getElementById("chatMessage").value;
    var receiver = document.getElementById("receiverEmail").value;

    if (receiver.length > 0) {
        // private message
        console.log("private 1", receiver);
        chatConnection.send("SendMessageToReceiver", sender, receiver, message).catch(function (err) {
            console.log(`Error: ${err}`);
        });
    } else {
        // send message for all the users
        chatConnection.send("SendMessageToAll", sender, message).catch(function(err) {
            console.log(`Error: ${err}`);
        });
    }

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