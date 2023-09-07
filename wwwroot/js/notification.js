//declaring connection string
var connectionNotification = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/notification")
    .build();

document.getElementById("sendButton").disabled = true;
document.getElementById("sendButton").addEventListener("click", function(event) {
    var message = document.getElementById("notificationInput").value;
    connectionNotification.send("SendNotification", message).then(function() {
        document.getElementById("notificationInput").value = "";
    });

    event.preventDefault();
});

connectionNotification.on("LoadNotifications", function(counter, messages) {
    document.getElementById("notificationInput").innerHTML = "";

    document.getElementById("notificationCounter").innerHTML = "<span>(" + counter + ")</span>";

    for (var i = messages.length - 1; i >= 0; i--) {
        var li = document.createElement("li");
        li.textContent = "Notification - " + messages[i];
        document.getElementById("messageList").appendChild(li);
    }
});

//start connection
connectionNotification.start().then(function() {
    document.getElementById("sendButton").disabled = false;

    connectionNotification.send("LoadNotifications");
}, function() {
    console.log("smth goes wrong");
});