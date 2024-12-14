$(document).ready(function () {
    const hub = new signalR.HubConnectionBuilder()
        .withUrl("/hub/chatMainPage")
        .build();

    init();

    hub.on("newMessageAdded", createNewMessage);

    $(".send-new-message").click(sendMessage);

    $(".new-message").on("keyup", function (e) {
        if (e.which == 13) {
            sendMessage();
        }
    });

    $(".toggle-chat").click(function () {
        $(".chat-container").toggleClass("show hidden");
        // Добавляем обработчик кликов по документу для закрытия чата 
        $(document).click (function (e) 
        { 
            if (!$(e.target).closest(".chat-container, .toggle-chat").length) 
            { 
                $(".chat-container").addClass("hidden").removeClass("show"); 
            } 
        });
    });

    function sendMessage() {
        const message = $(".new-message").val();
        if (message.trim() !== "") {
            hub.invoke("addNewMessage", message)
                .catch(err => console.error(err));
            $(".new-message").val("");
        }
    }

    hub.start().then(function () {
        // When connection with the server is alive
        hub.invoke("userEnteredToChat")
            .catch(err => console.error(err));
    }).catch(err => console.error(err));

    function init() {
        const url = "/api/ApiChat/GetLastMessages";
        $.get(url).then(function (messages) {
            messages.forEach((message) => createNewMessage(message));
        }).catch(err => console.error(err));
    }

    function createNewMessage(message) {
        const messageBlock = $(".message.template").clone();
        messageBlock.removeClass("template");
        messageBlock.text(message);
        $(".messages").append(messageBlock);
        $(".messages").scrollTop($(".messages")[0].scrollHeight);
        showNotification("New message", message);
    }
});
