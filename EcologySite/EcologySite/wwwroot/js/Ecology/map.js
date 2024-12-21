var map = L.map('map').setView([51.505, -0.09], 13);
L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '© OpenStreetMap contributors'
}).addTo(map);

fetch('/api/locations')
    .then(response => response.json())
    .then(data => {
        data.forEach(location => {
            L.marker([location.latitude, location.longitude]).addTo(map)
                .bindPopup(`Latitude: ${location.latitude}, Longitude: ${location.longitude}`);
        });
    });

// add a function to save a new location
function saveLocation(lat, lng) {
    fetch('/api/locations', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ latitude: lat, longitude: lng, timestamp: new Date() })
    });
}

// adding a new location on map click
map.on('click', function(e) {
    var lat = e.latlng.lat;
    var lng = e.latlng.lng;
    L.marker([lat, lng]).addTo(map);
    saveLocation(lat, lng);
});

