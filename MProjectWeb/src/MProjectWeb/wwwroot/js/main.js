$(document).ready(function () {
    var pos = 0;
    var cars = ["P1", "P2", "P3", "P4", "P5", "P6", "P7", "P8", "P9", "P10",
        "P11", "P12", "P13", "P14", "P15", "P16", "P17", "P18", "P19", "P20",
        "P21", "P22", "P23", "P24"];

    //$("html").css("overflow", "hidden");

    
    var fullscreenElement = document.fullscreenElement || document.mozFullScreenElement ||
document.webkitFullscreenElement || document.msFullscreenElement;


    document.fullscreenEnabled = true;
    window.onresize = function (e) {
        

        if(screen.height === window.innerHeight){
            // this is full screen
            $("html").css("overflow", "hidden");
        }
        else {
            $("html").css("overflow", "auto");
            //alert("bad");
        }
    }


    document.onkeypress = function (e) {

        //do the required work
        //function keydPress() {

        //}

        if (e.key == 'b' || e.key == 'B') {
            if (pos > 0)
                pos--;
            location.hash = cars[pos];
        }
        if (e.key == 'n' || e.key == 'N') {
            if (pos < cars.length - 1)
                pos++;
            location.hash = cars[pos];

        }
    }


});
