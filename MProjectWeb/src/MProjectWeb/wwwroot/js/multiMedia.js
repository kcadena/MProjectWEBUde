//mostrar texto mouse hover img
$(document).ready(function () {
    $("[rel='tooltip']").tooltip();

    $('.thumbnail').hover(
        function () {
            $(this).find('.caption').slideDown(250); //.fadeIn(250)
        },
        function () {
            $(this).find('.caption').slideUp(250); //.fadeOut(205)
        }
    );
});
//Close windows
$(".btn-close").click(function () {
    var player = document.getElementById("vid");
    //player.load();
    player.pause();
});
//Abrir nueva ventana
function openNewWindows(url) {
    //alert(url);
    window.open(url, "nuevo", "directories=no, location=no, menubar=no, scrollbars=yes, statusbar=no, tittlebar=no, width=400, height=400");
}
//Abrir pestaña
function openNewTab(url, nom) {
    //alert(url);
    var a = document.createElement("a");
    a.title = nom;
    a.target = "_blank";
    a.href = url;
    a.click();
}
//metodo para obtener los archivos de una caracteristica
function showFiles(keym, usu, car, type, text) {
    try {
        $("#" + act_ant).parent().removeClass('op-active');
        $("#Files").parent().addClass('op-active');
        act_ant = "Files";
    } catch (ex) { }
    $.ajax({
        url: '/Projects/Files',
        type: 'POST',
        data: { idCar: car, idUsu: usu, keym: keym, type: type, text: text, publicFile: false },
        async: true,
        cache: false,
        beforeSend: function () {

            $("#content-opt").html("");
            $("#content-opt").html('<img src="/img/ajax-loader.gif" style="position:relative;margin-left:50%;margin-top:20%;height:8%;width:8%;">');
        },
        success: function succ(data) {
            if (data != 0) {
                $("#content-opt").html("");
                $("#content-opt").html(data);
            }
            else {
                alert("Multimedia=>ShowFiles: No se encontraron actividaes relacionadas!!!");
            }
        }
    }).fail(
       function (da) {
           alert("No se pudo cargar la pagina.");
       }
    );
}
//metodo para obtener los archivos publicos de una caracteristica 
function publicShowFiles(keym, usu, car, type, text) {
    //alert(keym + " usu: " + usu + "   car: " + car + "     typ: " + type);
    $.ajax({
        url: '/Projects/PublicFiles',
        type: 'POST',
        data: { idCar: car, idUsu: usu, keym: keym, type: type, text: text, publicFile: true },
        async: true,
        cache: false,
        beforeSend: function () {

            //$("#area").html("");
            //$("#area").html('<img src="/img/ajax-loader.gif" style="position:relative;margin-left:50%;margin-top:20%;height:8%;width:8%;">');
        },
        success: function succ(data) {
            //alert(data);
            if (data != 0) {
                $("#area").html("");
                $("#area").html(data);
            }
            else {
                alert("Multimedia=>PusblicShowFiles:  No se encontraron actividaes relacionadas!!!");
            }
        }
    }).fail(
       function (da) {
           alert("No se pudo cargar la pagina.");
       }
    );
}
//Muestra todos los archivos publicos de lucene y la base de datos
function publicFiles(type, text) {
    $.ajax({
        url: '/Projects/PublicFiles',
        type: 'Get',
        data: { type: type, text: text },
        async: true,
        cache: false,
        beforeSend: function () {

            $("#area").html("");
            $("#area").html('<img src="/img/ajax-loader.gif" style="position:relative;margin-left:50%;margin-top:20%;height:8%;width:8%;">');
        },
        success: function succ(data) {
            //alert(data);
            if (data != 0) {
                $("#area").html("");
                $("#area").html(data);
            }
            else {
                alert("Multimedia=>PublicFiles: No se encontraron actividaes relacionadas!!!");
            }
        }
    }).fail(
   function (da) {
       alert("No se pudo cargar la pagina.");
   }
);
}
//get data from car usu and keym for get files 
function getDataFiles() {
    try {
        var keym = document.getElementById("aux").getAttribute("keym");
        var idUsu = document.getElementById("aux").getAttribute("idUsu");
        var idCar = document.getElementById("aux").getAttribute("idCar");
        //alert("asd");
        publicShowFiles(keym, idUsu, idCar, "img", "");

    } catch (err) {
        window.location = '/Projects/PublicFiles';
    }
}
//GEnera todos los links de los proyectos que poseen publicacion web
//ajaxAllLinks1 => resalta la publicacion actual
function ajaxAllLinks1(keym, idCar, idUsu) {
   // alert("YAY");

    var  cad= keym+"-"+idCar+"-"+ idUsu ;
    $.ajax({
        url: '/Index/Links',
        type: 'Get',
        data: {cad:cad},
        dataType:'text',
        async: true,
        cache: false,
        beforeSend: function () {

            $("#loadInfo").html("");
            $("#loadInfo").html('<img src="/img/ajax-loader.gif" style="position:relative;margin-left:50%;margin-top:20%;height:8%;width:8%;">');
        },
        success: function succ(data) {
            //alert(data);
            if (data != 0) {
                $("#loadInfo").html("");
                $("#loadInfo").html(data);
            }
            else {
                $("#loadInfo").html("No se ha podido realizar esta operacion. Intentalo mas tarde o contacta al administrador.");
            }
        }
    }).fail(function (da) {
        alert("No se pudo cargar la pagina.");
    });
}
//ajaxAllLinks2 => solo muestra todos los links sin resaltar la publicacion
function ajaxAllLinks2() {
    $.ajax({
        url: '/Index/Links',
        type: 'Get',
        //data: { cad: false },
        dataType: 'text',
        async: true,
        cache: false,
        beforeSend: function () {

            $("#loadInfo").html("");
            $("#loadInfo").html('<img src="/img/ajax-loader.gif" style="position:relative;margin-left:50%;margin-top:20%;height:8%;width:8%;">');
        },
        success: function succ(data) {
            //alert(data);
            if (data != 0) {
                $("#loadInfo").html("");
                $("#loadInfo").html(data);
            }
            else {
                $("#loadInfo").html("No se ha podido realizar esta operacion. Intentalo mas tarde o contacta al administrador.");
            }
        }
    }).fail(function (da) {
        alert("No se pudo cargar la pagina.");
    });
}
//Muestra los Graficos estadisticos
function showCharts(keym, usu, car) {
    
    $.ajax({
        url: '/Projects/Charts',
        type: 'POST',
        data: { keym: keym, idCar: car, idUsu: usu, ori:true},
        async: true,
        cache: false,
        beforeSend: function () {

            $("#content-opt").html("");
            $("#content-opt").html('<img src="/img/ajax-loader.gif" style="position:relative;margin-left:50%;margin-top:20%;height:8%;width:8%;">');
        },
        success: function succ(data) {
            //alert(data);
            if (data != 0) {
                $("#content-opt").html("");
                $("#content-opt").html(data);
            }
            else {
                alert("Multimedia=>showCharts:  No se encontraron actividaes relacionadas!!!");
            }
        }
    }).fail(
       function (da) {
           alert("No se pudo cargar la pagina.");
       }
    );
}


//Muestra los Reportes que tienen las caracteristicas
function showReports(keym, usu, car) {

    $.ajax({
        url: '/Projects/Reports',
        type: 'POST',
        data: { keym: keym, idCar: car, idUsu: usu, ori: true },
        async: true,
        cache: false,
        beforeSend: function () {

            $("#content-opt").html("");
            $("#content-opt").html('<img src="/img/ajax-loader.gif" style="position:relative;margin-left:50%;margin-top:20%;height:8%;width:8%;">');
        },
        success: function succ(data) {
            //alert(data);
            if (data != 0) {
                $("#content-opt").html("");
                $("#content-opt").html(data);
            }
            else {
                alert("Multimedia=>showCharts:  No se encontraron Reportes relacionados!!!");
            }
        }
    }).fail(
       function (da) {
           alert("No se pudo cargar la pagina.");
       }
    );
}


//Muestra los Recursos Financieros que tienen las caracteristicas
function showFinancial(keym, usu, car) {

    $.ajax({
        url: '/Projects/Financial',
        type: 'POST',
        data: { keym: keym, idCar: car, idUsu: usu, ori: true },
        async: true,
        cache: false,
        beforeSend: function () {

            $("#content-opt").html("");
            $("#content-opt").html('<img src="/img/ajax-loader.gif" style="position:relative;margin-left:50%;margin-top:20%;height:8%;width:8%;">');
        },
        success: function succ(data) {
            //alert(data);
            if (data != 0) {
                $("#content-opt").html("");
                $("#content-opt").html(data);
            }
            else {
                alert("Multimedia=>showCharts:  No se encontraron Reportes relacionados!!!");
            }
        }
    }).fail(
       function (da) {
           alert("No se pudo cargar la pagina.");
       }
    );
}


//Muestra los Inventario que tienen las caracteristicas
function showInventory(keym, usu, car) {

    $.ajax({
        url: '/Projects/Inventory',
        type: 'POST',
        data: { keym: keym, idCar: car, idUsu: usu, ori: true },
        async: true,
        cache: false,
        beforeSend: function () {

            $("#content-opt").html("");
            $("#content-opt").html('<img src="/img/ajax-loader.gif" style="position:relative;margin-left:50%;margin-top:20%;height:8%;width:8%;">');
        },
        success: function succ(data) {
            //alert(data);
            if (data != 0) {
                $("#content-opt").html("");
                $("#content-opt").html(data);
            }
            else {
                alert("Multimedia=>showCharts:  No se encontraron Reportes relacionados!!!");
            }
        }
    }).fail(
       function (da) {
           alert("No se pudo cargar la pagina.");
       }
    );
}


