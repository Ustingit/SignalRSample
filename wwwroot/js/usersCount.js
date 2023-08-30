﻿//declaring connection string
var connectionUserCount = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/userCount", signalR.HttpTransportType.WebSockets) // here we can declare transport type
    //.configureLogging(signalR.LogLevel.Trace) // manage logging level
    .build();

//connect (kind of establishing two-way connection) (connect to methods that hub invokes aka receive notifications from hub)
connectionUserCount.on("updateTotalViews", (value) => {
    var newCountSpan = document.getElementById("totalViewsCounter");
    newCountSpan.innerText = value.toString();
});

connectionUserCount.on("updateTotalUsers", (value) => {
    var newCountSpan = document.getElementById("totalUsersCounter");
    newCountSpan.innerText = value.toString();
});

//invoke hub methods (send notification to hub)
function newWindowLoadedOnClient() {
    connectionUserCount.invoke("NewWindowLoaded", "JustAStubDataToShowPassingOfTheParameters").then((value) => console.log(value));
}

function fulfilled() {
    console.log("connection succcessful");
    newWindowLoadedOnClient();
}

function rejected() {
    console.log("connection refused");
}

//start connection
connectionUserCount.start().then(fulfilled, rejected);