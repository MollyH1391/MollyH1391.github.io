let ubikeData = []
let markers = []
let map

// Initialize and add the map
function initMap() {
    // The location of Uluru
    const uluru = { lat: 25.0461158, lng: 121.5255704 };
    // The map, centered at Uluru
    map = new google.maps.Map(document.getElementById("menu_map"), {
        zoom: 12,
        center: uluru,
    });
    // The marker, positioned at Uluru
    const marker = new google.maps.Marker({
        position: uluru,
        map: map,
    });
}

function fetchUbikeData() {
    //const req = new Request()

    fetch('https://tcgbusfs.blob.core.windows.net/dotapp/youbike/v2/youbike_immediate.json')
        .then((res) => {
            return res.json()
        })
        .then((res) => {
            console.log(res)

            ubikeData = res

            // Loop through the results array and place a marker for each
            // set of coordinates.
            for (let i = 0; i < ubikeData.length; i++) {
                const coords = { lat: ubikeData[i].lat, lng: ubikeData[i].lng }
                const latLng = new google.maps.LatLng(coords.lat, coords.lng);

                const marker = new google.maps.Marker({
                    position: latLng,
                    map: map,
                });
                markers.push(marker)
            }

            // Add a marker clusterer to manage the markers.
            const markerCluster = new markerClusterer.MarkerClusterer({ map, markers });
        })
}

window.onload = () => {
    fetchUbikeData()
}