"use strict";

// signalR connection
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// chat variables
var currentChat = "messagesection";
let chats = {};
chats["BPC group chat"] = "messagesection";

var newChatnum = 0;
var numOfUsers = 0;


//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (message) {
    var table = document.getElementById("messagetable");

    var new_row = table.insertRow(-1);
    new_row.className = `message r`;

    var newMSG = new_row.insertCell(0);
    newMSG.textContent = message;

    var messageSection = document.getElementById("messagesection");
    messageSection.scrollTop = messageSection.scrollHeight;
});

connection.on("ReceiveMessageFrom", function (message, sender) {
    // find the table for the sender , if none then create one
    // if found then apply messages to that table


    var userTable = chats[sender];
    if (userTable) {
        var table = document.getElementById(userTable);

        var new_row = table.insertRow(-1);
        new_row.className = `message r`;

        var newMSG = new_row.insertCell(0);
        newMSG.textContent = message;

        var messageSection = document.getElementById(userTable);
        messageSection.scrollTop = messageSection.scrollHeight; 
    }

});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;

    var username = document.getElementById("username").value
    console.log(username);

    connection.invoke("ReceiveID", username)

    var table = document.getElementById("messageList");
    var newRow = table.insertRow(-1);
    var newUser = newRow.insertCell(0);


}).catch(function (err) {
    return console.log(err.toString());
});


document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    message = message.trim();

    if (message != "") {
        var table = document.getElementById(currentChat);
        var user = "";

        for (let users in chats) {
            if (chats[users] === currentChat) {
                user = users;
                break;
            }
        }

        var new_row = table.insertRow(-1);
        new_row.className = `message sent`;

        var newMSG = new_row.insertCell(0);
        newMSG.textContent = message;

        var messageSection = document.getElementById(currentChat);
        messageSection.scrollTop = messageSection.scrollHeight;

        if (user == "BPC group chat") {
            connection.invoke("SendMessage", message).catch(function (err) {
                return console.error(err.toString());
            });
        }
        else {
            connection.invoke("SendMessageTo", user, message).catch(function (err) {
                return console.error(err.toString());
            });
        }
        event.preventDefault();
    }

    document.getElementById("messageInput").value = "";
});


var activechats = document.getElementsByClassName("UserChat");
for (let i = 0; i < activechats.length; i++) {
    if (activechats[i] != null) {
        activechats[i].addEventListener("click", function () {
            var title = this.querySelector("h3").textContent;
            var newChat = chats[title];
            if (!newChat) { console.log("error"); return; }

            if (currentChat) {
                document.getElementById(currentChat).style.display = "none";
            }
            document.getElementById(newChat).style.display = "block";

            currentChat = newChat;
        })
    }
}

document.getElementById("newChat").addEventListener("click", function (event) {

    if (newChatnum == 0) {
        newChatnum++;
        document.getElementById(currentChat).style.display = "none";

        // configuring the button in the message list
        console.log("new chat");
        var table = document.getElementById("messageList");
        var newBtn = document.createElement("input");

        newBtn.type = "button";
        newBtn.className = "UserChat";

        var newrow = table.insertRow(0);
        var newcell = newrow.insertCell(0);
        newcell.appendChild(newBtn);

        //configuring the message section to add messages to
        var tableSection = document.getElementById("tables");
        var msgSection = document.createElement("table");
        var msgid = "user" + numOfUsers;
        msgSection.id = msgid;
        numOfUsers++;

        var tableBody = document.createElement("tbody");
        tableBody.className = "tableformat";

        msgSection.appendChild(tableBody);
        tableSection.appendChild(msgSection);

        //configuring the message to tell the user to enter a username
        var table = document.getElementById(msgid);

        var new_row = table.insertRow(-1);
        new_row.className = `message`;

        var newMSG = new_row.insertCell(0);
        var newEnter = new_row.insertCell(-1);
        var userinputname = document.createElement("input");
        userinputname.type = "text";
        var enter = document.createElement("input");
        enter.type = "button";
        enter.id = "usernameInput";
        newMSG.appendChild(userinputname);
        newEnter.appendChild(enter);

        enter.addEventListener("click", function (event) {
            var username = userinputname.value;

            var headername = document.createElement("h3");
            headername.textContent = username;
            headername.className = "UserChatTitle";
            newBtn.appendChild(headername);

            console.log(newBtn);
            console.log(headername);

            chats[username] = msgSection;
            currentChat = msgSection;
        });

        var messageSection = document.getElementById(msgid);
        messageSection.scrollTop = messageSection.scrollHeight;
    }

    event.preventDefault();
});

//add a connection to recieve errors if the username is not found