/* $(document).ready(function () {
    const baseUrl = `http://localhost:5173/`;
    const userName = $(".user-name").val();

    const hub = new signalR.HubConnectionBuilder()
        .withUrl(baseUrl + "/hub/map")
        .build();

    initMap();

    hub.on("updateMap", addMarker);

    function initMap() {
        const map = L.map('map').setView([51.505, -0.09], 13);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        }).addTo(map);

        // Get initial data
        $.get(baseUrl + "/api/locations").then(function (data) {
            data.forEach((point) => addMarker(point, map));
        });

        // Example: Add user location
        map.on('click', function (e) {
            const latitude = e.latlng.lat;
            const longitude = e.latlng.lng;
            hub.invoke("UpdateMap", userName, latitude, longitude);
        });
    }

    function addMarker(point, map) {
        L.marker([point.latitude, point.longitude]).addTo(map)
            .bindPopup(`<b>${point.userName}</b><br>Latitude: ${point.latitude}<br>Longitude: ${point.longitude}`).openPopup();
    }

    hub.start().then(function () {
        console.log("Connected to the map hub");
    });
});*/

$(document).ready(function () {
    const baseUrl = `http://localhost:5173/`;
    const userName = $(".user-name").val();

    const hub = new signalR.HubConnectionBuilder()
        .withUrl(baseUrl + "/hub/map")
        .build();

    hub.start().then(function () {
        console.log("Connected to the map hub");
    }).catch(function (err) {
        return console.error(err.toString());
    });

    hub.on("updateMap", addMarker);

    function initMap() {
        const map = L.map('map').setView([51.505, -0.09], 13);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        }).addTo(map);

        // Get initial data
        $.get(baseUrl + "/api/locations").then(function (data) {
            data.forEach((point) => addMarker(point, map));
        });

        // Example: Add user location
        map.on('click', function (e) {
            const latitude = e.latlng.lat;
            const longitude = e.latlng.lng;
            hub.invoke("UpdateMap", userName, latitude, longitude).catch(function (err) {
                return console.error(err.toString());
            });
        });

        function addMarker(point) {
            L.marker([point.latitude, point.longitude]).addTo(map)
                .bindPopup(`<b>${point.userName}</b><br>Latitude: ${point.latitude}<br>Longitude: ${point.longitude}`).openPopup();
        }
    }

    initMap();
});
