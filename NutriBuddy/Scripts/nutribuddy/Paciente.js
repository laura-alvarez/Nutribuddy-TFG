var step = 1;
var autoNumericYear = {
    allowDecimalPadding: false,
    caretPositionOnFocus: "start",
    decimalPlaces: 0,
    decimalPlacesRawValue: 0,
    decimalPlacesShownOnBlur: 0,
    decimalPlacesShownOnFocus: 0,
    digitGroupSeparator: "",
    maximumValue: maxYear,
    minimumValue: "1940",
    emptyInputBehavior: 'null',
    watchExternalChanges: true,
    wheelOn: "hover",
    wheelStep: "1"
};

$(document).ready(function () {
    console.log(maxYear);
    $('.btnNext').on('click', function () {
        $("#formInicial_" + step).addClass("hidden");
        step = step + 1;
        $("#formInicial_" + step).removeClass("hidden");
        if (step === 4) {
            $("#formTitle").addClass("hidden");
        } else {
            $("#formTitle").removeClass("hidden");
        }
    });

    $('.btnBack').on('click', function () {
        $("#formInicial_" + step).addClass("hidden");
        step = step - 1;
        $("#formInicial_" + step).removeClass("hidden");
    });

    $('#btnEnd').on('click', function () {
        enviarFInicial();
    });

    var anyo = new AutoNumeric('#anyoNacimiento', autoNumericYear);
});

function enviarFInicial() {
    var formDatos = new FormData();
    formDatos.append("IdUsuario", document.getElementById('idPaciente').value);
    formDatos.append("IDObjetivo", document.querySelector('input[type="radio"][name=objetivoP]:checked').value);
    formDatos.append("EsSedentario", document.querySelector('input[type="radio"][name=estiloV]:checked').value == 1?true:false);
    formDatos.append("TieneEstres", document.querySelector('input[type="radio"][name=AEstres]:checked').value == 1 ? true : false);
    formDatos.append("BajaAutoestima", document.querySelector('input[type="radio"][name=autoestima]:checked').value == 1 ? true : false);
    formDatos.append("Altura", document.getElementById('altura').value);
    formDatos.append("Peso", document.getElementById('peso').value);
    formDatos.append("Pecho", document.getElementById('mPecho').value);
    formDatos.append("Cintura", document.getElementById('mCintura').value);
    formDatos.append("Cadera", document.getElementById('mCadera').value);
    formDatos.append("Grasa", document.getElementById('pGrasa').value);
    formDatos.append("Musculo", document.getElementById('pMagro').value);
    formDatos.append("Agua", document.getElementById('pAgua').value);

    $.ajax({
        url: '/Paciente/SaveFormularioInicial',
        type: "POST",
        data: formDatos,
        processData: false,
        contentType: false,
        async: false,
        success: function (data) {
            window.location.href = '/ZonaPrivada/Home';
        },
        error: function (xhr, status, p3, p4) {
            window.location.href = '/ZonaPrivada/Home';
        }
    });
}
