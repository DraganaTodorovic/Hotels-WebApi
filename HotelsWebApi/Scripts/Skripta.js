$(document).ready(function () {

    //>>>>>>>>>>>>>>>>> Global variable initialization on start <<<<<<<<<<<<<<<<<<<<<<<<<
    var host = window.location.host;
    var token = null;
    var headers = {};
    var dropDowEndpoint = "/api/lanci";
    var tabelaEndpoint = "/api/hoteli";
    var pretragaEndpoint = "/api/kapacitet";
    var tradicijaEndpoint = "/api/tradicija";

    var editingId;

    $("#logoutDiv").css("display", "none");
    $("#loginDiv").css("display", "none");
    $("#info_begin").css("display", "none");
    $("#search").addClass("hidden");
    $("#show_info").addClass("hidden");    
    $("#create").addClass("hidden");
    $("#registracijaDiv").addClass("hidden");
    $("#uspesna_registracija").addClass("hidden");
    $("#btns").removeClass("hidden");
    
    var tabelaUrl = "http://" + host + tabelaEndpoint;
    var dropDownUrl = "http://" + host + dropDowEndpoint;
    var pretragaUrl = "http://" + host + pretragaEndpoint;
    var tradicijaUrl = "http://" + host + tradicijaEndpoint;

    $.getJSON(tabelaUrl, setTabela);

    $("body").on("click", "#regBtn", loadRegistracijaForm);    
    $("body").on("click", "#odustajanjeResistracijaBtn", loadStartPageFormOdustajanjeReg);
    $("body").on("click", "#prijava_na_sistemBtn", loadPrijavaForm);
    $("body").on("click", "#prijavaBtn", loadPrijavaForm);
    $("body").on("click", "#odustajanjePrijavaBtn", loadStartPageFormOdustajanjePrijava);
    $("body").on("click", "#logoutBtn", loadStartPageFormOdjava);
    $("body").on("click", "#giveUpBtn", resetFormeZaDodavanje);
    $("body").on("click", "#btnDelete", deleteItemFromTabela);
    $("body").on("click", "#findBtn", pretraga);
    $("body").on("click", "#tradicijaBtn", tradicija);

    //>>>>>>>>>>> Load main entity(into table) <<<<<<<<<<<<<<<<<
    function setTabela(data, status) {
        console.log("Status: " + status);
        if (token) {
            var container = $("#data").css("width", "1100px");
        }
        else {
            var container = $("#data").css("width", "1100px");
        }
        
        container.empty();

        if (status === "success") {
            console.log(data);

            var div = $("<div></div>");
            var h1 = $("<h2 class=\"text-center\">Hoteli</h2><br />");
            div.append(h1);

            var table = $("<table border='1' class=\"table table-hover\"></table>");
            if (token) {
                var header = $("<tr style=\"background-color : yellow\"><th class=\"text-center\" style=\"width:100px\">Naziv</th><th class=\"text-center\" style=\"width:100px\">Godina otvaranja</th><th class=\"text-center\" style=\"width:70px\">Broj soba</th><th class=\"text-center\" style=\"width:80px\">Broj zaposlenih</th><th class=\"text-center\" style=\"width:100px\">Lanac</th><th style='width:100px' class=\"text-center\">Akcija</th></tr>");
            }
            else {
                var header = $("<tr style=\"background-color : yellow\"><th class=\"text-center\" style=\"width:100px\">Naziv</th><th class=\"text-center\" style=\"width:100px\">Godina otvaranja</th><th class=\"text-center\" style=\"width:70px\">Broj soba</th><th class=\"text-center\" style=\"width:80px\">Broj zaposlenih</th><th class=\"text-center\" style=\"width:100px\">Lanac</th></tr>");
            } 
            table.append(header);

            for (var i = 0; i < data.length; i++) {
                var row = "<tr>";
                var displayData = "<td>" + data[i].Naziv + "</td><td>" + data[i].GodinaOtvaranja + "</td><td>" + data[i].BrojSoba + "</td><td>" + data[i].BrojZaposlenih + "</td><td>" + data[i].LanacNaziv + "</td>";
                if (token) {                    
                    var stringId = data[i].Id.toString();
                    var displayDelete = "<td><a href=\"#\" id=btnDelete name=" + stringId + ">[Brisanje]</a></td>";
                    row += displayData + displayDelete + "</tr>";
                }
                else {
                    row += displayData + "</tr>";
                }
                table.append(row);
            }

            div.append(table);
            container.append(div);
        }
        else {
            var div = $("<div></div>");
            var h1 = $("<h1>Greška prilikom preuzimanja hotela!</h1>");
            div.append(h1);
            container.append(div);
        }
    }

    //>>>>>>>>>>>>>>>> Load 2nd entity into dropdown menu-create form <<<<<<<<<<<<<<<<<<
    function getDropDown(data, status) {
        var lanci = $("#createInput4select");
        lanci.empty();

        if (status === "success") {
            for (var i = 0; i < data.length; i++) {
                var option = "<option value=" + data[i].Id + ">" + data[i].Naziv + "</option>";
                lanci.append(option);
            }
            //$("#createInput4select").val($("#createInput4select > option:first").val());
        }
        else {
            var div = $("<div></div>");
            var h3 = $("<h3>Greška prilikom preuzimanja lanaca hotela!</h3>");
            div.append(h3);
            lanci.append(div);
        }
    }

    // Posle klika na dugme Registracija
    $("#do_registration").submit(function (e) {
        e.preventDefault();

        var email = $("#regEmail").val();
        var loz1 = $("#regLoz").val();
        var loz2 = $("#regLoz2").val();

        // objekat koji se salje
        var sendData = {
            "Email": email,
            "Password": loz1,
            "ConfirmPassword": loz2
        };

        $.ajax({
            type: "POST",
            url: 'http://' + host + "/api/Account/Register",
            data: sendData

        }).done(function (data) {
            $("#regEmail").val('');
            $("#regLoz").val('');
            $("#regLoz2").val('');
            $("#uspesna_registracija").removeClass("hidden");
                        
            alert("Uspesna registracija na sistem!");

        }).fail(function (data) {
            alert("Greska prilikom registracije!");
        });
    });

    // Posle klika na dugme Prijava na formi za prijavu korisnika
    $("#do_prijava").submit(function (e) {
        e.preventDefault();

        var email = $("#loginEmail").val();
        var loz = $("#loginPass").val();

        var sendData = {
            "grant_type": "password",
            "username": email,
            "password": loz
        };

        $.ajax({
            "type": "POST",
            "url": 'http://' + host + "/Token",
            "data": sendData

        }).done(function (data) {
            token = data.access_token;

            $("#loginEmail").val('');
            $("#loginPass").val('');
            $("#loginDiv").css("display", "none");
            $("#loggedInParagraph").html("Prijavljen korisnik: <b>" + email + "</b>");            $("#logoutDiv").css("display", "block");

            $("#data").css("display", "block");
            $("#search").removeClass("hidden");
            $("#create").removeClass("hidden");            
            $("#show_info").removeClass("hidden");

            $.getJSON(dropDownUrl, getDropDown);
            $.getJSON(tabelaUrl, setTabela); 

        }).fail(function (data) {
            alert("Greška prilikom prijave!");
        });
    });

    //>>>>>>>>>>>>>>  After clicking on button Dodaj za dodavanje <<<<<<<<<<<<<<<<<<<<<<<<<
    $("#create").submit(function (e) {
        e.preventDefault();

        // korisnik mora biti ulogovan
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        var naziv = $("#createInput1").val();
        var soba = $("#createInput2").val();
        var godina = $("#createInput3").val();
        var lanac = $("#createInput4select").val();
        var zaposleni = $("#createInput5").val();
        $("#validationMsgInput1").empty();
        $("#validationMsgInput2").empty();
        $("#validationMsgInput3").empty();
        $("#validationMsgInput5").empty();

        var sendData = {
            "Naziv": naziv,
            "GodinaOtvaranja": godina,
            "BrojZaposlenih": zaposleni,
            "BrojSoba": soba,
            "LanacId": lanac
        }
        httpAction = "POST";
        url = tabelaUrl;

        $.ajax({
            "url": url,
            "type": httpAction,
            "data": sendData,
            "headers": headers
        })
            .done(function (data, status) {
                $.getJSON(tabelaUrl, setTabela); 
                resetFormeZaDodavanje();
            })
            .fail(function (data, status) {
                alert("Greska prilikom dodavanja!");
            });

    });

    //>>>>>>>>>>>>>>>>>>>>>> After clicking on button Pretrazi <<<<<<<<<<<<<<<<<<<<<<<<<    
    function pretraga() {

        var start = $("#findInput1").val();
        var kraj = $("#findInput2").val();
        httpAction = "POST";

        if (token) {
            headers.Authorization = "Bearer " + token;
        }

        var pretragaUrlFull = pretragaUrl + "/?najmanje=" + start + "&najvise=" + kraj;
        $.ajax({
            "url": pretragaUrlFull,
            "type": httpAction,
            "headers": headers
        })
            .done(setTabela)
            .fail(function (data, status) {
                alert("Greska prilikom pretrage!");
            });

        $("#findInput2").val('');
        $("#findInput1").val('');
    }

    //>>>>>>>>>>>>>>>> Ucitaj hotele sa tradicijom - posle klika na dugme Ucitavanje <<<<<<<<<<<<<<<<<<
    function tradicija() {

        if (token) {
            headers.Authorization = "Bearer " + token;
        }
        httpAction = "GET";
        var url = tradicijaUrl;
        $.ajax({
            "url": url,
            "type": httpAction,
            "headers": headers
        })
            .done(function (data) {
                $("#tradicijaBtn").css("display", "none");
                $("#pFirst").html("<b>1. " + data[0].Naziv + "</b> (osnovan: <b>" + data[0].GodinaOsnivanja + "</b>. godine)");
                $("#pSecond").html("<b>2. " + data[1].Naziv + "</b> (osnovan: <b>" + data[1].GodinaOsnivanja + "</b>. godine)");
            })
            .fail(function (data, status) {
                alert(data.status + ": " + data.statusText);
            });
    }

    //>>>>>>>>>>>>>>>>>>>> Removing entry from table after clicking on button [Brisanje] <<<<<<<<<<<<<<<<<<<<<<<
    function deleteItemFromTabela() {
        var deleteId = this.name;
        httpAction = "DELETE";

        if (token) {
            headers.Authorization = "Bearer " + token;
        }
        var deleteUrl = tabelaUrl + "?id=" + deleteId;
        $.ajax({
            "url": deleteUrl,
            "type": httpAction,
            "headers": headers
        })
            .done(function () {
                $.getJSON(tabelaUrl, setTabela);
            })
            .fail(function () {
                alert("Greska prilikom brisanja nekretnine!")
            })
    }

    function resetFormeZaDodavanje() {
        $("#createInput1").val('');
        $("#createInput2").val('');
        $("#createInput3").val('');
        $("#createInput4select").val($("#createInput4select > option:first").val());
        $("#createInput5").val('');
    }

    //>>>>>>>> Load Prijava form <<<<<<<<<<
    function loadPrijavaForm() {
        $("#btns").addClass("hidden");
        $("#registracijaDiv").addClass("hidden");
        $("#uspesna_registracija").addClass("hidden");
        $("#loginDiv").css("display", "block");  
        $("#loginEmail").val('');
        $("#loginPass").val('');
    }

    // After clicking on button Registracija, load form for regitration
    function loadRegistracijaForm() {
        $("#registracijaDiv").removeClass("hidden");
        $("#btns").addClass("hidden");    
        $("#uspesna_registracija").addClass("hidden");      
        $("#regEmail").val('');
        $("#regLoz").val('');
        $("#regLoz2").val('');
    }

    //>>>>>>>> Load Start Page form after clicking on button Odustajanje sa Registracione forme <<<<<<<<<<
    function loadStartPageFormOdustajanjeReg() {
        $("#regEmail").val('');
        $("#regLoz").val('');
        $("#regLoz2").val('');
        $("#registracijaDiv").addClass("hidden");
        $("#btns").removeClass("hidden");
    }

    //>>>>>>>> Load Start Page form after clicking on button Odustajanje sa Prijave <<<<<<<<<<
    function loadStartPageFormOdustajanjePrijava() {
        $("#btns").removeClass("hidden");
        $("#loginDiv").css("display", "none");
        $("#btns").removeClass("hidden");
    }

    //>>>>>>>> Load Start Page form after clicking on button Odjava <<<<<<<<<<
    function loadStartPageFormOdjava() {
        token = null;
        headers = {};
        $("#loggedInParagraph").empty();
        $("#logoutDiv").css("display", "none");
        $("#btns").removeClass("hidden");
        $("#search").addClass("hidden");      
        $("#show_info").addClass("hidden");
        $("#create").addClass("hidden");
        $("#tradicijaBtn").css("display", "block");
        $("#pFirst").empty();
        $("#pSecond").empty();
        $.getJSON(tabelaUrl, setTabela); //posle odjave
    }

});
