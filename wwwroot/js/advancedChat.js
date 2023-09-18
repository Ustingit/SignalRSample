var advancedChatConnection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/advancedChat")
    .withAutomaticReconnect([0, 1000, 5000, null])
    .build();

advancedChatConnection.on("ReceiveUserConnected", function (userId, userName, isReConnect) {
    if (!isReConnect) {
        addMessage(`${userName} is online`);
    }
});

advancedChatConnection.on("ReceiveUserDisconnected", function (userId, userName, userHasConnection) {
    if (!userHasConnection) {
        addMessage(`${userName} is offline`);
    }
});

advancedChatConnection.on("ReceiveAddRoomMessage", function (maxRoom, roomId, roomName, userId, userName) {
    addMessage(`${userName} has created the room ${roomName}`);
    fillRoomDropDown();
});

advancedChatConnection.on("ReceiveDeleteRoomMessage", function (deletedRoom, selectedRoom, roomName, userId, userName) {
    addMessage(`${userName} has deleted the room ${roomName}`);
    fillRoomDropDown();
});

document.getElementById("btnCreateRoom").addEventListener("click", function (event)
{
    addnewRoom(4);
    event.preventDefault();
});

advancedChatConnection.start().then(function () {
    console.log("connection is established");
}, function () {
    console.log("smth goes wrong");
});

function addnewRoom(maxRoom) {
    var createRoomName = document.getElementById('createRoomName');

    var roomName = createRoomName.value;

    if (roomName == null && roomName == '') {
        return;
    }

    /*POST*/
    $.ajax({
        url: '/ChatRooms/PostChatRoom',
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ id: 0, name: roomName }),
        async: true,
        processData: false,
        cache: false,
        success: function (json) {
            /*ADD ROOM COMPLETED SUCCESSFULLY*/
            advancedChatConnection.invoke("SendAddRoomMessage", maxRoom, json.id, roomName);
            createRoomName.value = '';
        },
        error: function (xhr) {
            alert('error during room creation');
        }
    })
}

function deleteRoom() {
    var room = document.getElementById('ddlDelRoom');
    var roomName = room.options[room.selectedIndex].text;

    if (confirm(`Are you sure you want to delete room '${roomName}'?`) === false) {
        return;
    }

    $.ajax({
        url: `/ChatRooms/${room.value}`,
        type: "DELETE",
        contentType: 'application/json;',
        async: true,
        processData: false,
        cache: false,
        success: function (json) {
            advancedChatConnection.invoke("SendDeleteRoomMessage", json.deleted, json.selected, roomName);
            fillRoomDropDown();
        },
        error: function (xhr) {
            alert('error during room deletion');
        }
    })
}

document.addEventListener('DOMContentLoaded', (event) => {
    fillRoomDropDown();
    fillUserDropDown();
});

function fillUserDropDown() {

    $.getJSON('/ChatRooms/GetChatUser')
        .done(function (json) {
            var ddlSelUser = document.getElementById("ddlSelUser");
            ddlSelUser.innerText = null;

            json.forEach(function (item) {
                var newOption = document.createElement("option");

                newOption.text = item.userName; //item.whateverProperty
                newOption.value = item.id;
                ddlSelUser.add(newOption);
            });

        })
        .fail(function (jqxhr, textStatus, error) {
            var err = textStatus + ", " + error;
            console.log("Request Failed: " + jqxhr.detail);
        });
}

function fillRoomDropDown() {

    $.getJSON('/ChatRooms/GetChatRoom')
        .done(function (json) {
            var ddlDelRoom = document.getElementById("ddlDelRoom");
            var ddlSelRoom = document.getElementById("ddlSelRoom");

            ddlDelRoom.innerText = null;
            ddlSelRoom.innerText = null;

            json.forEach(function (item) {
                var newOption = document.createElement("option");

                newOption.text = item.name;
                newOption.value = item.id;
                ddlDelRoom.add(newOption);

                var newOption1 = document.createElement("option");

                newOption1.text = item.name;
                newOption1.value = item.id;
                ddlSelRoom.add(newOption1);

            });

        })
        .fail(function (jqxhr, textStatus, error) {
            var err = textStatus + ", " + error;
            console.log("Request Failed: " + jqxhr.detail);
        });

}

function addMessage(message) {
    if (message == null || message === '') {
        return;
    }

    var messagesList = document.getElementById("messagesList");
    var li = document.createElement("li");
    li.innerHTML = message;
    messagesList.appendChild(li);
};