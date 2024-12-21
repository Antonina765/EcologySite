function initMap() {
    var map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: -34.397, lng: 150.644 },
        zoom: 8
    });

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var pos = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };

            var marker = new google.maps.Marker({
                position: pos,
                map: map
            });

            map.setCenter(pos);

            // Отправка данных местоположения на сервер
            $.ajax({
                type: "POST",
                url: "/location",
                contentType: "application/json",
                data: JSON.stringify({ Latitude: pos.lat, Longitude: pos.lng, UserId: userId, UserName: userName }),
                success: function () {
                    console.log("Location saved successfully.");
                },
                error: function () {
                    console.error("Error saving location.");
                }
            });
        });
    }
}

function getUserId() {
    // Здесь должно быть получение ID пользователя из вашего контекста или другой логики
    return "12345";  // Пример значения
}

function getUserName() {
    // Здесь должно быть получение имени пользователя из вашего контекста или другой логики
    return "JohnDoe";  // Пример значения
}
