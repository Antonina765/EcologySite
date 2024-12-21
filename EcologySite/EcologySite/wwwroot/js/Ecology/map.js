function initMap() {
    var map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: -34.397, lng: 150.644 },
        zoom: 8
    });

    function addMarker(location) {
        var pos = {
            lat: location.latitude,
            lng: location.longitude
        };
        var marker = new google.maps.Marker({
            position: pos,
            map: map,
            title: location.userName
        });
    }

    $.ajax({
        type: "GET",
        url: "/Location/getLocations",
        success: function (data) {
            data.forEach(function (location) {
                addMarker(location);
            });
        },
        error: function (error) {
            console.error("Error fetching locations:", error);
        }
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
            
            $.ajax({
                type: "POST",
                url: "/Location/addLocation",
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
