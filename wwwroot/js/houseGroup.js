var lbl_houseJoined = document.getElementById("lbl_houseJoined");

var btn_un_gryffindor = document.getElementById("btn_un_gryffindor");
var btn_un_slytherin = document.getElementById("btn_un_slytherin");
var btn_un_hufflepuff = document.getElementById("btn_un_hufflepuff");
var btn_un_ravenclaw = document.getElementById("btn_un_ravenclaw");
var btn_gryffindor = document.getElementById("btn_gryffindor");
var btn_slytherin = document.getElementById("btn_slytherin");
var btn_hufflepuff = document.getElementById("btn_hufflepuff");
var btn_ravenclaw = document.getElementById("btn_ravenclaw");

var trigger_gryffindor = document.getElementById("trigger_gryffindor");
var trigger_slytherin = document.getElementById("trigger_slytherin");
var trigger_hufflepuff = document.getElementById("trigger_hufflepuff");
var trigger_ravenclaw = document.getElementById("trigger_ravenclaw");

//declaring connection string
var connectionHouseGroup = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/housegroup")
    .build();

btn_gryffindor.addEventListener("click", function (event) {
    connectionHouseGroup.send("JoinHouse", "gryffindor"); // send as we don't expect response
    event.preventDefault();
});
btn_slytherin.addEventListener("click", function (event) {
    connectionHouseGroup.send("JoinHouse", "slytherin"); // send as we don't expect response
    event.preventDefault();
});
btn_hufflepuff.addEventListener("click", function (event) {
    connectionHouseGroup.send("JoinHouse", "hufflepuff"); // send as we don't expect response
    event.preventDefault();
});
btn_ravenclaw.addEventListener("click", function (event) {
    connectionHouseGroup.send("JoinHouse", "ravenclaw"); // send as we don't expect response
    event.preventDefault();
});
btn_un_gryffindor.addEventListener("click", function (event) {
    connectionHouseGroup.send("LeaveHouse", "gryffindor"); // send as we don't expect response
    event.preventDefault();
});
btn_un_slytherin.addEventListener("click", function (event) {
    connectionHouseGroup.send("LeaveHouse", "slytherin"); // send as we don't expect response
    event.preventDefault();
});
btn_un_hufflepuff.addEventListener("click", function (event) {
    connectionHouseGroup.send("LeaveHouse", "hufflepuff"); // send as we don't expect response
    event.preventDefault();
});
btn_un_ravenclaw.addEventListener("click", function (event) {
    connectionHouseGroup.send("LeaveHouse", "ravenclaw"); // send as we don't expect response
    event.preventDefault();
});

connectionHouseGroup.on("otherUserStateChanged", (message) => {
    toastr.success(message);
});

connectionHouseGroup.on("subscriptionStatus", (strGroupsJoined, houseName, hasSubscribed) => {
    lbl_houseJoined.innerText = strGroupsJoined;

    if (hasSubscribed) {
        switch (houseName) {
            case "slytherin":
                btn_slytherin.style.display = "none";
                btn_un_slytherin.style.display = "";
                break;
            case "ravenclaw":
                btn_ravenclaw.style.display = "none";
                btn_un_ravenclaw.style.display = "";
                break;
            case "hufflepuff":
                btn_hufflepuff.style.display = "none";
                btn_un_hufflepuff.style.display = "";
                break;
            case "gryffindor":
                btn_gryffindor.style.display = "none";
                btn_un_gryffindor.style.display = "";
                break;
            default:
                console.log("incorrect house");
                break;
        }
        toastr.success(`You have succesfully subscribed to house ${houseName}`);
    } else {
        switch (houseName) {
        case "slytherin":
            btn_slytherin.style.display = "";
            btn_un_slytherin.style.display = "none";
            break;
        case "ravenclaw":
            btn_ravenclaw.style.display = "";
            btn_un_ravenclaw.style.display = "none";
            break;
        case "hufflepuff":
            btn_hufflepuff.style.display = "";
            btn_un_hufflepuff.style.display = "none";
            break;
        case "gryffindor":
            btn_gryffindor.style.display = "";
            btn_un_gryffindor.style.display = "none";
            break;
        default:
                console.log("incorrect house");
                break;
        }
        toastr.success(`You have succesfully unsubscribed from house ${houseName}`);
    }
});

connectionHouseGroup.on("houseTrigger", (message) => {
    toastr.success(message);
});

trigger_gryffindor.addEventListener("click", function (event) {
    connectionHouseGroup.send("TriggerHouse", "gryffindor"); // send as we don't expect response
    event.preventDefault();
});
trigger_slytherin.addEventListener("click", function (event) {
    connectionHouseGroup.send("TriggerHouse", "slytherin"); // send as we don't expect response
    event.preventDefault();
});
trigger_hufflepuff.addEventListener("click", function (event) {
    connectionHouseGroup.send("TriggerHouse", "hufflepuff"); // send as we don't expect response
    event.preventDefault();
});
trigger_ravenclaw.addEventListener("click", function (event) {
    connectionHouseGroup.send("TriggerHouse", "ravenclaw"); // send as we don't expect response
    event.preventDefault();
});

function rejected() {
    console.log("smth went wrong");
}

function fulfilled() {
    console.log("good");
}

//start connection
connectionHouseGroup.start().then(fulfilled, rejected);