//Visualizar un punto
function showPoint(loc, lon) {
    var x = parseFloat(loc.replace(/,/, '.'));
    var y = parseFloat(lon.replace(/,/, '.'));

    var pla = { lat: x, lng: y };

    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 7,
        center: pla,
        mapTypeControl: true,
        mapTypeControlOptions: {
            style: google.maps.MapTypeControlStyle.HORIZONTAL_BAR,
            position: google.maps.ControlPosition.TOP_CENTER
        },
        zoomControl: true,
        zoomControlOptions: {
            position: google.maps.ControlPosition.LEFT_CENTER
        },
        scaleControl: true,
        streetViewControl: true,
        streetViewControlOptions: {
            position: google.maps.ControlPosition.LEFT_TOP
        }
    });

    var marker = new google.maps.Marker({
        position: pla,
        map: map
    });

    google.maps.event.addListener(map, "idle", function () {
        google.maps.event.trigger(map, 'resize');
        map.setCenter(marker.getPosition());
    });

}

var xpos;
//Registrar Punto o poligono
function initMapReg() {
    //alert("YAY");
    var map = new google.maps.Map(document.getElementById('mapReg'), {
        center: { lat: 5.070477, lng: -75.514064 },
        zoom: 5,
        mapTypeControl: true,
        mapTypeControlOptions: {
            style: google.maps.MapTypeControlStyle.HORIZONTAL_BAR,
            position: google.maps.ControlPosition.TOP_CENTER
        },
        zoomControl: true,
        zoomControlOptions: {
            position: google.maps.ControlPosition.LEFT_CENTER
        },
        scaleControl: true,
        streetViewControl: true,
        streetViewControlOptions: {
            position: google.maps.ControlPosition.LEFT_TOP
        }
    });

    google.maps.event.addListener(map, "idle", function () {
        google.maps.event.trigger(map, 'resize');
        //map.setCenter(marker.getPosition());
    });
    var drawingManager = new google.maps.drawing.DrawingManager({
        drawingMode: google.maps.drawing.OverlayType.MARKER,
        drawingControl: true,
        drawingControlOptions: {
            position: google.maps.ControlPosition.TOP_CENTER,
            drawingModes: [
              google.maps.drawing.OverlayType.MARKER,
              google.maps.drawing.OverlayType.POLYGON,
            ]
        }
    });
    drawingManager.setMap(map);

    google.maps.event.addListener(drawingManager, 'markercomplete', function (marker) {
        xpos = marker.getPosition();
        marker.setOptions({
            draggable: true
        });
        google.maps.event.addListener(marker, 'dragend', function () {
            // Put your code here when marker finish event drangend Example get LatLang
            /*
                var objLatLng = marker.getPosition().toString().replace("(", "").replace(")", "").split(',');
                Lat = objLatLng[0];
                Lat = Lat.toString().replace(/(\.\d{1,5})\d*$/, "$1");// Set 5 Digits after comma
                Lng = objLatLng[1];
                Lng = Lng.toString().replace(/(\.\d{1,5})\d*$/, "$1");// Set 5 Digits after comma

            */
            xpos = marker.getPosition();

        });
        drawingManager.setOptions({ drawingControl: false });
        drawingManager.setDrawingMode(null);
        DeleteShapesGoogleMaps(marker);
    });
}

function regPoint() {
    var inf = $("#infMapReg").html().split('-');

    $.ajax({
        url: '/MultiMedia/UpdateRegPosition',
        type: 'POST',
        data: { idCar: inf[1], usuCar: inf[2], keym: inf[0], idFile: inf[3], loc: xpos.lat(), lng: xpos.lng() },//keym-idCar-usuCar-idFile
        async: true,
        cache: false,
        beforeSend: function () {
        },
        success: function succ(data) {
            if (data == true) {
                alert("Operacion realizada exitosamente!!!.");
            }
            else {
                alert("No se actualizo la informacion!!!");
            }
            $("#modalMapReg").modal("hidde");
        }
    }).fail(
       function (da) {
           alert("No se pudo cargar la pagina.");
       }
    );

}
