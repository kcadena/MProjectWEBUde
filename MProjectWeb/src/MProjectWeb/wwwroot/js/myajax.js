function myajax(key, idcar, usu) {

    var src="http://localhost:5000/Projects/publicprojects?p=" + key + "-" + idcar + "-" + usu;
    history.pushState(src,src,src );

    $.ajax({
        url: '/Projects/getLinks',
        data: { "key": key, "id_caracteristica": idcar, "usu": usu },
        type: 'GET',
        dataType: "json",
        crossDomain: true,
        //async: true,
        success: function (e) {
            //alert("OK");
            try { 

                document.getElementById("aux").setAttribute("keym", key);
                document.getElementById("aux").setAttribute("idUsu", usu);
                document.getElementById("aux").setAttribute("id_caracteristica", idcar); 
            } catch (err) { }
            //alert(e);
            //alert(e);
            try{
                $("#activityList").html('');
                $("#scrip").html('');
                jQuery.each(e, function (i, val) {
                    var sep = val.split("-");
                    var idcom = sep[0].split(",");
                    //alert(sep[1]);
                    var id = sep[1];
                    for (var i = 0; i < id.length; i++) {
                        id = id.replace(' ', '_');
                    }
                    for (var i = 0; i < sep[2].length; i++) {
                        sep[2] = sep[2].replace(',', '-');
                    }

                    if (sep[1] == "Atras") {
                        var ht = '<li class="selItems" id="selBack"><a id="' + id + '">' + sep[1] + '</a></li>';
                    }
                    else {
                        var ht = '<li class="selItems" ><a id="' + id + '">' + sep[1] + '</a></li>';
                    }
                    
                    var sc = "<script type='text/javascript'> $('#" + id + "').click(function() { myajax('" + idcom[0] + "','" + idcom[1] + "','" + idcom[2] + "'); ";
                    sc = sc+ " $('#area').load('" + sep[2] + "'); }); <\/script>";
                
                    $("#activityList").append(ht);
                    $("#scrip").append(sc);

                
                
                });
            } catch (err) {
                alert(err);
            }
            
        }
    }).fail(function () {
        alert("error");
    });
}

window.addEventListener('popstate', function (e) {
    // e.state is equal to the data-attribute of the last image we clicked
    var src = e.state;
    var dat = src.split("=");
    dat = dat[1].split("-");
    window.open(e.state, "_self");
    //$("#area").load(src);
    try {

        document.getElementById("aux").setAttribute("keym",  dat[0]);
        document.getElementById("aux").setAttribute("idUsu", dat[2]);
        document.getElementById("aux").setAttribute("id_caracteristica", dat[1]);
    } catch (err) { }

    myajax(dat[0],dat[1],dat[2]);
});