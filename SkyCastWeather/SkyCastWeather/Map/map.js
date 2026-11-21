(function () {
  'use strict';

  var DEFAULT_CENTER = [24.8607, 67.0011]; // Karachi
  var map = L.map('map', { zoomControl: true }).setView(DEFAULT_CENTER, 5);

  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
  }).addTo(map);

  var markers = L.layerGroup().addTo(map);
  var selectionMarker = null;
  var radarLayer = null;
  var legendEl = document.getElementById('legend');
  var coordsEl = document.getElementById('coords');

  function postToHost(message) {
    if (window.chrome && window.chrome.webview) {
      window.chrome.webview.postMessage(message);
    }
  }

  // RainViewer publishes the latest radar frame at a public JSON address.
  async function latestRadarFrame() {
    var response = await fetch('https://api.rainviewer.com/public/weather-maps.json');
    if (!response.ok) {
      throw new Error('RainViewer returned ' + response.status);
    }
    var data = await response.json();
    var past = data.radar && data.radar.past;
    if (!past || past.length === 0) {
      throw new Error('no radar frames available');
    }
    return { host: data.host, path: past[past.length - 1].path };
  }

  async function setLayer(key) {
    if (radarLayer) {
      map.removeLayer(radarLayer);
      radarLayer = null;
    }

    if (key !== 'radar') {
      legendEl.textContent = 'Standard map';
      return;
    }

    legendEl.textContent = 'Loading radar...';
    try {
      var frame = await latestRadarFrame();
      radarLayer = L.tileLayer(frame.host + frame.path + '/256/{z}/{x}/{y}/2/1_1.png', {
        opacity: 0.7,
        attribution: 'Radar &copy; <a href="https://www.rainviewer.com">RainViewer</a>'
      }).addTo(map);
      legendEl.textContent = 'Precipitation radar: light (blue) to heavy (red)';
    } catch (err) {
      legendEl.textContent = 'Radar unavailable: ' + err.message;
    }
  }

  window.skycast = {
    setLayer: setLayer,

    setMarkers: function (list) {
      markers.clearLayers();
      (list || []).forEach(function (m) {
        L.marker([m.lat, m.lng]).bindTooltip(m.name).addTo(markers);
      });
    },

    setSelection: function (sel) {
      if (selectionMarker) {
        map.removeLayer(selectionMarker);
      }
      selectionMarker = L.marker([sel.lat, sel.lng]).addTo(map);
      map.panTo([sel.lat, sel.lng]);
      coordsEl.textContent = 'Selected: ' + sel.lat.toFixed(4) + ', ' + sel.lng.toFixed(4);
    }
  };

  map.on('click', function (e) {
    var lat = e.latlng.lat;
    var lng = e.latlng.lng;
    coordsEl.textContent = 'Selected: ' + lat.toFixed(4) + ', ' + lng.toFixed(4);
    postToHost({ type: 'pick', lat: lat, lng: lng });
  });
})();
