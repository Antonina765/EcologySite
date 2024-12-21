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
                data: JSON.stringify({ Latitude: pos.lat, Longitude: pos.lng }),
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
