

$(document).ready(function () {
    fetch("/api/Authenticate/Login", {
        method: "POST",
        body: JSON.stringify({ email: "redasaad@gmail.com", password: "reda" }),
        headers: {
            "Content-Type": "application/json",

        }
    })
        .then(res => res.json())
        .then(data => {
            localStorage.setItem("token", data.token);

            return fetch("/api/Account/GetUser", {
                method: "GET",
                headers: {
                    "Authorization": `Bearer ${data.token}`
                }
            });

        })
    const token = localStorage.getItem("token");

    const connection = new signalR.HubConnectionBuilder()
        .withUrl(`/communityHub/?access_token=${token}`, {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets,
            headers: {
                "Authorization": `Bearer ${token}`
            }
        })
        .build();

    async function startConnection() {
        try {
            connection.start().then(() => {
                connection.invoke("GetConnectionId").then(connectionId => {
                    console.log("Connection ID from server:", connectionId);
                    localStorage.setItem("connectionId", connectionId);
                });
            });
        } catch (err) {
            console.error("Connection error:", err);
        }
    }

    connection.onreconnecting(() => {
        console.log("Reconnecting...");
    });
    
    $(".join").click(function () {
        var groupName = $("#groupName").val();
        $.ajax({
            type: "POST",
            url: "/api/UserGroup/JoinGroup",
            data: JSON.stringify({ groupName: groupName, ConnectionId: localStorage.getItem("connectionId") }),
            headers: {
                "Authorization": `Bearer ${token}`

            },
            contentType: "application/json",
            success: function (response) {
                console.log("Joined group successfully:", response);
            },
            error: function (error) {
                console.error("Error joining group:", error);
            }
        });
    });

    $(".leave").click(function () {
        var groupName = $("#groupName").val();
        $.ajax({
            type: "POST",
            url: "/api/UserGroup/LeaveGroup",
            data: JSON.stringify({ groupName: groupName, ConnectionId: localStorage.getItem("connectionId") }),
            headers: {
                "Authorization": `Bearer ${token}`

            },
            contentType: "application/json",
            success: function (response) {
                console.log("Joined group successfully:", response);
            },
            error: function (error) {
                console.error("Error joining group:", error);
            }
        });
    });

    startConnection();
});

