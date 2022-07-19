var calendar;
var nComida;
var altura;
var peso;
var pecho;
var cintura;
var cadera;
var grasa;
var agua;
var magro;

const autonumericPesoMedidas = {
    caretPositionOnFocus: "end",
    decimalCharacter: ",",
    decimalCharacterAlternative: ".",
    decimalPlaces: 3,
    digitGroupSeparator: "",
    emptyInputBehavior: "zero",
    maximumValue: "300",
    minimumValue: "0",
    watchExternalChanges: true
};

const autonumericAltura = {
    caretPositionOnFocus: "end",
    decimalPlaces: 0,
    digitGroupSeparator: "",
    emptyInputBehavior: "zero",
    maximumValue: "240",
    minimumValue: "0",
    watchExternalChanges: true
};

$(document).ready(function () {
    $("#diarioModal").prependTo("body");

    inicializarMedidas();

    inicializarModalDiario();

    getCitasCalendario();

    $("#guardarDiario").on('click', function () { guardarDiario(); });

    $("#anyadirComida").on('click', function () { anyadirComida(); });  

    $("input[type=radio][name=deporte]").on('click', function (e) {
        if (e.currentTarget.value == "1") {
            $("#deportesDiv").removeClass('hidden');
        } else {
            $("#deportesDiv").addClass('hidden');
        }
    });

    $("#altura").focusout(function () { calcularIMC(); });
    $("#peso").focusout(function () { calcularIMC(); });
});

function calcularIMC() {
    let pesoAct = peso.get();
    let alturaAct = altura.get();
    $.ajax({
        url: '/Diario/CalcularIMC?altura=' + alturaAct + '&peso=' + pesoAct,
        type: "GET",
        async: false,
        success: function (data) {
            $("#IMCData").text(data.imc);
        },
    });
}

function inicializarMedidas() {
    altura = new AutoNumeric('#altura', autonumericAltura);
     peso= new AutoNumeric('#peso', autonumericPesoMedidas);
     pecho= new AutoNumeric('#mPecho', autonumericPesoMedidas);
     cintura= new AutoNumeric('#mCintura', autonumericPesoMedidas);
     cadera= new AutoNumeric('#mCadera', autonumericPesoMedidas);
     grasa= new AutoNumeric('#pGrasa', autonumericPesoMedidas);
     agua= new AutoNumeric('#pAgua', autonumericPesoMedidas);
    magro = new AutoNumeric('#pMagro', autonumericPesoMedidas);
}



function getCitasCalendario() {
    $.ajax({
        url: "/Diario/GetEntradasDiario",
        type: "POST",        
        error: function (jqXHR, textStatus, errorThrown) {
            alert("error");
        },
        success: function (jsondata, status, xhr) {
            cargarCitasEnCalendario(jsondata.events); 
        },
        dataType: "json"
    });
}


function cargarCitasEnCalendario(data) {
    var language = getIdioma();
    calendar = new FullCalendar.Calendar(document.getElementById('calendario'), {
        headerToolbar: { 
            left: 'prev next today',
            center: 'title',
            right: ''
        },
        initialView: 'dayGridMonth',
        locale: language,
        height: 600,
        events: data,
        dateClick: function (info) {
            if (new Date() > info.date) {
                nComida = 0;
                $("#modalTitle2").html(formatearFecha(info.date));
                mostrarDiario(info.date.toISOString());
            }
        }  
    }).render();
};

function mostrarDiario(fecha) {
    $.ajax({
        url: "/Diario/TieneDiario?dia=" + fecha,
        type: "GET",
        async: false,
        error: function (XHR, textStatus, errorThrown) {
            alert("error");
        },
        success: function (data) {
            inicializarModalDiario();
            if (data.existe) {
                //Existe una entrada se precarga
                cargarEntrada(data.diario);
            } else {
                //Se crea de 0
            }
            $(".collapse").removeClass("show")
            $('#diarioModal').modal({ backdrop: 'static', keyboard: false });
        }
    });
}

function cargarEntrada(data) {
    for (var indexC = 0; indexC < data.Comidas.length; indexC++) {
        anyadirComida();
        let currentC = nComida - 1;
        $("#tipoComida_" + currentC).val(data.Comidas[indexC].IDTipo);
        $("#alimentos_"+ currentC).val(data.Comidas[indexC].Alimentos);
        $("#aliObservacioens_" + currentC).val(data.Comidas[indexC].Observaciones);
        $("#horaComida_" + currentC).val(data.Comidas[indexC].HoraS);
    }

    if (data.Medidas != null) {
        altura.set(data.Medidas.Altura);
        peso.set(data.Medidas.Peso);
        pecho.set(data.Medidas.Pecho);
        cintura.set(data.Medidas.Cintura);
        cadera.set(data.Medidas.Cadera);
        grasa.set(data.Medidas.Grasa);
        magro.set(data.Medidas.Musculo);
        agua.set(data.Medidas.Agua);
    }

    $("input[name = estado][value='" + data.IDEstado + "']").prop('checked', true);

    if (data.Emociones != null) {
        for (var indexE = 0; indexE < data.Emociones.length; indexE++) {
            $("input[name=emocion][value='" + data.Emociones[indexE] + "']").prop('checked', true);
        }
    }

    $("#agua").val(data.Agua);
    $("#medicamentos").val(data.Medicamentos);
    $("input[name=suenyo][value='" + data.IDSuenyo + "']").prop('checked', true);
    $("input[name=menstruacion][value='" + (data.Menstruaccion ? 1 : 0) + "']").prop('checked', true);

    $("input[name=deporte][value='" + (data.Deporte ? 1 : 0) + "']").prop('checked', true);
    if (data.Deporte) {
        $("#deportesDiv").removeClass("hidden");
        if (data.Deportes != null) {
            for (var indexD = 0; indexD < data.Deportes.length; indexD++) {
                $("input[name=deporteT][value='" + data.Deportes[indexD] + "']").prop('checked', true);
            }
        }
        $("#depObservaciones").val(data.DeporteObservaciones);
    }
}

function inicializarModalDiario() {
    $("#cardComidas").empty();

    altura.set(0);
    peso.set(0);
    pecho.set(0);
    cintura.set(0);
    cadera.set(0);
    grasa.set(0);
    magro.set(0);
    agua.set(0);

    $('input[type=radio]').prop('checked', false);
    $('input[type=checkbox]').prop('checked', false);

    $("#agua").val('');
    $("#medicamentos").val('');
    $("#depObservaciones").val('');
}

function guardarDiario() {
    var data = {};
    data["Dia"] = $("#modalTitle2").text();
    var Comidas = [];
    $.each($(".comida"), (index, value) => {
        let nComida = value.id.split('_')[1];
        let comida = {};
        comida["IDTipo"] = $("#tipoComida_" + nComida).val();
        comida["Hora"] = $("#horaComida_" + nComida).val();
        comida["Alimentos"] = $("#alimentos_" + nComida).val();
        comida["Observaciones"] = $("#aliObservacioens_" + nComida).val();
        Comidas.push(comida);            
    });
    data["Comidas"] = Comidas;
    var Medidas = {};
    console.log(altura.getNumericString());
    console.log(peso.getNumericString());
    Medidas["Altura"] = altura.getNumericString().replace('.', ','); 
    Medidas["Peso"] = peso.getNumericString().replace('.',',');
    Medidas["Pecho"] = pecho.getNumericString().replace('.', ',');
    Medidas["Cintura"] = cintura.getNumericString().replace('.', ',');
    Medidas["Cadera"] = cadera.getNumericString().replace('.', ',');
    Medidas["Grasa"] = grasa.getNumericString().replace('.', ',');
    Medidas["Musculo"] = magro.getNumericString().replace('.', ',');
    Medidas["Agua"] = agua.getNumericString().replace('.', ',');
    data["Medidas"] = Medidas;

    data["IDEstado"] = $("input[type=radio][name=estado]:checked").val();

    var Emociones = [];
    $.each($("input[type=checkbox][name=emocion]:checked"), (index, value) => {
        Emociones.push(value.value);
    })
    data["Emociones"] = Emociones;

    data["Agua"] = $("#agua").val();

    data["IDSuenyo"] = $("input[type=radio][name=suenyo]:checked").val();

    data["Medicamentos"] = $("#medicamentos").val();
    data["Menstruaccion"] = $("input[type=radio][name=menstruacion]:checked").val() == "1";
    data["Deporte"] = $("input[type=radio][name=deporte]:checked").val() == "1";

    var Deportes = [];
    $.each($("input[type=checkbox][name=deporteT]:checked"), (index, value) => {
        Deportes.push(value.value);
    })
    data["Deportes"] = Deportes;
    data["DeporteObservaciones"] = $("#depObservaciones").val();

    $.ajax({
        url: '/Diario/GuardarDiario',
        type: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        processData: false,
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            Swal.fire({
                title: data.msg,
                icon: data.flag === 0 ? 'success' : 'error',
                iconColor: '#f2233a',
                backdrop: 'true',
                customClass: {
                    confirmButton: 'btnLO',
                }
            }).then((result) => {
                location.reload();
            });
        },
        error: function (xhr, status, p3, p4) {
            alert("error");
        }
    });
}

function anyadirComida() {
    var divComidas = $("#cardComidas");
    if (divComidas.children().length == 1) {
        $("#NoComida").addClass('hidden');
    }

    var idioma = getIdioma();
    if (divComidas.children().length < 6) {
        divComidas.append('<div id="comida_' + nComida + '" class="comida">'
            + '<div style="width: 100%;position: relative;float: revert;"><i id="bComida_' + nComida + '" class="fa-solid fa-trash-can borrarComida" title="'+(idioma == 'es'?'Eliminar comina':'Delete food')+'" onclick="eliminarComida(this)"></i></div>'
            + '<div style="display: flex; flex-direction: row;">'
            + '<div style="width: 51%; display: flex; padding-right: 2%;">'
            + '<label>' + (idioma == 'es' ? 'Tipo' : 'Type')+':</label><select id="tipoComida_' + nComida + '">'
            + '<option value="-1">--</option>'
            + '<option value="1">' + (idioma == 'es' ? 'Desayuno' : 'Breakfast') +'</option>'
            + '<option value="2">' + (idioma == 'es' ? 'Comida' : 'Lunch') +'</option>'
            + '<option value="3">' + (idioma == 'es' ? 'Cena' : 'Dinner') +'</option>'
            + '<option value="4">' + (idioma == 'es' ? 'Snack' : 'Snack') +'</option>'
            + '</select></div>'
            + '<div style="display: flex;"><label>' + (idioma == 'es' ? 'Hora' : 'Hour') +'</label><input id="horaComida_'+nComida+'" type="time"/></div></div>'
            + '<div class="comidaBloque">'
            + '<div style="width: 50%"><label>' + (idioma == 'es' ? 'Alimentos' : 'Foods') +':</label><textarea id="alimentos_'+nComida+'" class="comidaTextArea" style="width:95%; margin-right:5%"></textarea></div>'
            + '<div style="width: 50%"><label>' + (idioma == 'es' ? 'Observaciones' : 'Notes') +':</label><textarea id="aliObservacioens_'+nComida+'" class="comidaTextArea" style="width:100%"></textarea></div>'
            + '</div></div>');
        nComida = nComida + 1;
        if (divComidas.children().length == 6) {
            $("#anyadirComida").prop('disabled', true);
        }
    } 
}

function eliminarComida(e) {
    var idN = e.id.split('_')[1];
    $("#comida_" + idN).remove();
    $("#anyadirComida").prop('disabled', false);
    if ($("#cardComidas").children().length == 1) {
        $("#NoComida").removeClass('hidden');
    }
}