//declaring connection string
var connectionDeathlyHallows = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/deathlyhallows")
    .build();

var wandSpan = document.getElementById("wandCounter");
var stoneSpan = document.getElementById("stoneCounter");
var cloakSpan = document.getElementById("cloakCounter");

//connect (kind of establishing two-way connection) (connect to methods that hub invokes aka receive notifications from hub)
connectionDeathlyHallows.on("updateHallowCounter", (cloak, stone, wand) => {
    wandSpan.innerText = wand.toString();
    stoneSpan.innerText = stone.toString();
    cloakSpan.innerText = cloak.toString();
});

function rejected() {
    console.log("smth went wrong");
}

function fulfilled() {
    connectionDeathlyHallows.invoke("GetRaceStatus").then((raceCounter) => {
        wandSpan.innerText = raceCounter.wand.toString();
        stoneSpan.innerText = raceCounter.stone.toString();
        cloakSpan.innerText = raceCounter.cloak.toString();
    });
    console.log("smth went wrong");
}

//start connection
connectionDeathlyHallows.start().then(fulfilled, rejected);