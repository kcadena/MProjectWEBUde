function myajax(key, idcar, usu) {

    var src = "http://http://localhost:20539/Projects/publicprojects?p=" + key + "-" + idcar + "-" + usu;
    //alert(src);
    
    try {
        history.pushState(src, src, src);
    } catch (e) { alert(e);}

    $.ajax({
        url: '/Projects/getLinks',
        data: { "key": key, "idCar": idcar, "usu": usu },
        type: 'GET',
        dataType: "json",
        crossDomain: true,
        //async: true,
        success: function (e) {
            //alert(e);
            try { 

                //document.getElementById("aux").setAttribute("keym", key);
                //document.getElementById("aux").setAttribute("idUsu", usu);
                //document.getElementById("aux").setAttribute("idCar", idcar); 
            } catch (err) { }
            //alert(e);
            //alert(e);
            try{
                $("#activityList").html('');
                $("#scrip").html('');
                
                $("#content-opt").html("");
                $("#content-opt").html('<img src="/img/ajax-loader.gif" style="position:relative;margin-left:50%;margin-top:20%;height:8%;width:8%;">');

                jQuery.each(e, function (i, val) {
                    //alert(val);
                    var sep = val.split("|");
                    var jsId = sep[0];
                    for (var i = 0; i < jsId.length; i++) {
                        jsId = jsId.replace(',', '-');
                    }
                    var idcom = sep[0].split(",");
                    var id = sep[1];
                    for (var i = 0; i < id.length; i++) {
                        id = id.replace(' ', '_');
                    }
                    for (var i = 0; i < sep[2].length; i++) {
                        sep[2] = sep[2].replace(',', '-');
                    }


                    if (sep[1] == "Atras") {
                        var ht = '<li class="selItems" id="selBack"><a id="' + jsId + '">' + sep[1] + '</a></li>';
                    }
                    else {
                        var ht = '<li class="selItems" ><a id="' + jsId + '">' + sep[1] + '</a></li>';
                    }
                    
                    var sc = "<script type='text/javascript'> $('#" + jsId + "').click(function() { myajax('" + idcom[0] + "','" + idcom[1] + "','" + idcom[2] + "'); ";
                    
                    //sc = sc + ' alert("KElvin"); </script>';
                    var ax = "$.ajax({url:'" + sep[2] + "' ,error: function () {" +
                        "$('#area').html('<img style=\"margin:20px;\" src=\"http://localhost:82/mp/system/images/page-not-found.png\" height=\"60%\" width=\"60%\">');"
                        + "},success: function () { " +
                        " $('#area').load('" + sep[2] + "');"
                        + " } }); ";

                    sc = sc + "$.ajax({url:'" + sep[2] + "' ,error: function () {" +
                        "$('#area').html('<img style=\"margin:20px;\" src=\"http://localhost:82/mp/system/images/page-not-found.png\" height=\"60%\" width=\"60%\">');"
                        + "},success: function () { " +
                        " $('#area').load('" + sep[2] + "');"
                        + " } }); ";
                    //sc = sc + " $('#area').load('" + sep[2] + "');";
                    sc = sc +" }); <\/script>";
                    
                    $("#activityList").append(ht);
                    $("#scrip").append(sc);

                
                
                });
            } catch (err) {
                //alert("OJO "+err);
            }
            
        }
    }).fail(function () {
        alert("error");
    });
}

window.addEventListener('popstate', function (e) {
    // e.state is equal to the data-attribute of the last image we clicked
    
    //alert(e.CAPTURING_PHASE);
    //alert(e.currentTarget);
    //alert(e.returnValue);

    var src = e.state;
    var dat = src.split("=");
    //alert(dat[1]);
    dat = dat[1].split("-");
    //window.open(e.state, "_self");
    //$("#area").load(src);
    history.go(src);
    //try {
    //    document.getElementById("aux").setAttribute("keym", dat[0]);
    //    document.getElementById("aux").setAttribute("idUsu", dat[2]);
    //    document.getElementById("aux").setAttribute("idCar", dat[1]);
    //} catch (err) { }
        
    
   
});