//,    "dnxcore50": { }
//permite hacer el cambio del modal login a register y viceversa
$(".btn_log").click(function () {
    $("#modal_forget").modal("hide");
    $("#modal_reg").modal("hide");
    $("#modal_log").modal();
});

$(".btn_reg").click(function () {
    $("#modal_forget").modal("hide");
    $("#modal_log").modal("hide");
    $("#modal_reg").modal();
});

$(".btn_forget").click(function () {
    $("#modal_log").modal("hide");
    $("#modal_reg").modal("hide");
    $("#modal_forget").modal();
});

//vriables para las opciones en side-nav
var opt = "";     //variable para JSON
var con = "";   //Controlador
var act = "";   //Accion
var act_ant = "";//Accion Anterior
var con_ant = "";//Accion Anterior
//carga las optciones provenientes de archivo JSON 
function dropDownClass(op) {
    //alert(op);
    //op = op.replace(/&quot;/gi, '"');
    //op = op.trim();
    opt = op;
}
//funcion que habilita cargar AJAX en la pagina 
//op1 y op2 corresponden al controlador y accion que vienen del click de las opciones
function callOpt(con, act, sel) {
    
   
    try {
        if (sel == 0) {
            try {
                $("#" + act_ant).parent().removeClass('op-active');
            } catch (ex) {}
            $("#" + act).parent().addClass('op-active');
            act_ant = act;
        }
    }catch(exc){}


    if (sel == 1) {
        try {
            try {
                $("#op_" + con_ant).removeClass('in');
            } catch (ex) {  }
            $("#op_" + con).removeClass("in");
            con_ant = con;
        } catch (ex) { alert(ex); }

        //$("#op_" + act_ant).removeClass('in');  //cierra dropdown cuando selecciona otra opcion
        //$("#" + act_ant).parent().removeClass("active");
        //act_ant = op2;
    }
    //$("#op_" + act).addClass('active');
    $("#axd").removeClass('in');           //cierra dropdown cuando selecciona opcion en moviles
   
   

    //e.preventDefault();
    //e.stopPropagation();
    $.ajax({
        url: '/' + con + '/' + act,
        type: 'POST',
        dataType: "text",
        async: true,
        //cache: false,
        beforeSend: function () {

            $("#content-opt").html("");
            $("#content-opt").html('<img src="/img/ajax-loader.gif" style="position:relative;margin-left:50%;margin-top:20%;height:8%;width:8%;">');
        },
        success: function succ(data) {
            $("#content-opt").html("");
            $("#content-opt").html(data);

        }
    }).fail(
       function (da) {
           alert("No se pudo cargar la pagina.");
       }
    );

}


function projectsClick(dat) {
    dat = dat.replace(/prj_/gi, "");
    dat = { id: dat };
    $.ajax({
        url: '/Projects/PanelProject',
        type: "POST",
        dataType: "text",
        data: dat,
        //contentType: 'application/x-www-form-urlencoded; charset=utf-8',

        // //cache: false,
        //datType: "json",
        //async: true,
        ////cache: false,
        beforeSend: function () {
            $("#panel_prj").html("");
            $("#panel_prj").html('<img src="/img/ajax-loader.gif" style="position:relative;margin-left:50%;margin-top:20%;height:8%;width:8%;">');
        },
        success: function succ(data) {
            $("#panel_prj").html(data);
        }
    }).fail(
       function (da) {
           alert("No se pudo cargar la pagina.");
       }
    );

}


///////////////  opciones concernientes al dropdown que poseen las actividades  //////////////////////////

function optionsActivities(id, act) {
    //id => id de la caracteristica 
    //act => Opciones  [ 1* files 2*estatistics 3* more information] => Accion en el controlador
    $.ajax({
        url: '/Projects/' + act,
        type: 'POST',
        data: { id_car: id },
        async: true,
        //cache: false,
        beforeSend: function () {

            $("#content-opt").html("");
            $("#content-opt").html('<img src="/img/ajax-loader.gif" style="position:relative;margin-left:50%;margin-top:20%;height:8%;width:8%;">');
        },
        success: function succ(data) {
            $("#content-opt").html(data);

        }
    });

}


///////////////  Click de los folders  //////////////////////////

function openFolder(car,usu,op) {
    //alert("car: "+car+"  usu: "+usu);
    $.ajax({
        url: '/Projects/Activity',
        type: 'POST',
        data: { id_car:car, usu:usu ,opt:op },
        async: true,
        //cache: false,
        beforeSend: function () {

          // $("#content-opt").html("");
          // $("#content-opt").html('<img src="/img/ajax-loader.gif" style="position:relative;margin-left:50%;margin-top:20%;height:8%;width:8%;">');
        },
        success: function succ(data) {
            if (data != 0) {
                $("#content-opt").html("");
                $("#content-opt").html(data);
            }
            else {
                alert("a=>openFolder:  No se encontraron actividaes relacionadas!!!");
            }
        }
    }).fail(
       function (da) {
           alert("No se pudo cargar la pagina.");
       }
    );
}


$("#allLinks").click(function () {
    try {

        var keym = document.getElementById("aux").getAttribute("keym");
        var idUsu = document.getElementById("aux").getAttribute("idUsu");
        var idCar = document.getElementById("aux").getAttribute("idCar");

        //alert(keym+" "+idCar+" "+idUsu);  
        $("#modalLink").modal();
        ajaxAllLinks1(keym, idCar, idUsu);
        // alert("OK");

    } catch (err) {
        $("#modalLink").modal();
        ajaxAllLinks2();
    }
});

