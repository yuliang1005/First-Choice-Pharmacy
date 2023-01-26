/**
* This is a simple JavaScript demonstration of how to call MapBox API to load the maps.
* I have set the default configuration to enable the geocoder and the navigation control.
* https://www.mapbox.com/mapbox-gl-js/example/popup-on-click/
* 
*source: @author Jian Liew <jian.liew@monash.edu>
*/
const TOKEN = "YOUR_MAPBOX_ACCESS_TOKEN";
var locations = [];
// The first step is obtain all the latitude and longitude from the HTML
// The below is a simple jQuery selector
$(".coordinates").each(function () {
    var id = $(".storeid", this).text().trim();
    var name = $(".name", this).text().trim();
    var address = $(".address", this).text().trim();
    var phone = $(".phone", this).text().trim();
    var longitude = $(".longitude", this).text().trim();
    var latitude = $(".latitude", this).text().trim();
    // Create a point data structure to hold the values.
    var point = {
        "id": id,
        "name": name,
        "address": address,
        "phone": phone,
        "latitude": latitude,
        "longitude": longitude
    };
    // Push them all into an array.
    locations.push(point);
});
const layerList = document.getElementById('menu');
const inputs = layerList.getElementsByTagName('input');

for (const input of inputs) {
    input.onclick = (layer) => {
        const layerId = layer.target.id;
        map.setStyle('mapbox://styles/mapbox/' + layerId);
    };
}
var data = [];
for (i = 0; i < locations.length; i++) {
    var feature = {
        "type": "Feature",
        "properties": {
            "description": "<p><a href='/Stores/Details/" + locations[i].id + "'>" + locations[i].name + "</a></p>" +
                "<p>Address: " + locations[i].address + "</p>" +
                "<p>Phone Number :" + locations[i].phone + "</p>",
            "icon": "circle-15"
        },
        "geometry": {
            "type": "Point",
            "coordinates": [locations[i].longitude, locations[i].latitude]
        }
    };
    data.push(feature)
}
mapboxgl.accessToken = TOKEN;
var map = new mapboxgl.Map({
    container: 'map',
    style: 'mapbox://styles/mapbox/streets-v10',
    zoom: 11,
    center: [locations[0].longitude, locations[0].latitude]
});
map.on('load', function () {
    // Add a layer showing the places.
    map.addLayer({
        "id": "places",
        "type": "circle",
        "source": {
            "type": "geojson",
            "data": {
                "type": "FeatureCollection",
                "features": data
            }
        },
        'paint': {
            'circle-color': '#4264fb',
            'circle-radius': 8,
            'circle-stroke-width': 2,
            'circle-stroke-color': '#ffffff'
        }
    });
    map.addControl(new MapboxGeocoder({
        accessToken: mapboxgl.accessToken
    }));
    // When a click event occurs on a feature in the places layer, open a popup at the
    // location of the feature, with description HTML from its properties.
    map.on('click', 'places', function (e) {
        var coordinates = e.features[0].geometry.coordinates.slice();
        var description = e.features[0].properties.description;
        // Ensure that if the map is zoomed out such that multiple
        // copies of the feature are visible, the popup appears
        // over the copy being pointed to.
        map.flyTo({
            center: e.features[0].geometry.coordinates
        });
        while (Math.abs(e.lngLat.lng - coordinates[0]) > 180) {
            coordinates[0] += e.lngLat.lng > coordinates[0] ? 360 : -360;
        }
        new mapboxgl.Popup()
            .setLngLat(coordinates)
            .setHTML(description)
            .addTo(map);
    });
    // Change the cursor to a pointer when the mouse is over the places layer.
    map.on('mouseenter', 'places', function () {
        map.getCanvas().style.cursor = 'pointer';
    });
    // Change it back to a pointer when it leaves.
    map.on('mouseleave', 'places', function () {
        map.getCanvas().style.cursor = '';
    });
});
