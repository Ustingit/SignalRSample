//declaring connection string
var connectionUserCount = new signalR.HubConnectionBuilder().withUrl("/hubs/userCount").build();

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
    connectionUserCount.send("NewWindowLoaded");
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