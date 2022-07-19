$(document).ready(function () {
    $("#guardarTrabajador").on('click', function () {
        guardarTrabajador();
    });
    var idioma = getIdioma();
    $('#puestosTC').select2({
        placeholder: idioma == 'es'?'Selecciona mínimo un puesto':'Select one position minimun',
    });
});

function validador() {
    let correcto = true;
    if (!validarSoloLetras($("#NombreTC").val())) {
        backgroundInputValidated("#NombreTC");
        correcto = false;
    }

    if (!validarSoloLetras($("#ApellidosTC").val())) {
        backgroundInputValidated("#ApellidosTC");
        correcto = false;
    }

    
    var aux = $("#SSR").val();
    if (!validarSoloNumeros(aux) || aux.length > 12 || aux.length < 0) {
        backgroundInputValidated("#SSR");
        correcto = false;
    }
    if ($("#ProvinciaTC").val() == -1) {
        backgroundInputValidated("#ProvinciaTC");
        correcto = false;
    }
    if (!validarEmail($("#EmailTC").val())) {
        backgroundInputValidated("#EmailTC");
        correcto = false;
    }
    return correcto;
}

function backgroundInputValidated(id) {
    $(id).css("background-color", "#dd8b85");
}

function guardarTrabajador() {
    if (validador()) {
        var data = {};
        data["ID"] = $("#idTrabajador").val();
        data["Nombre"] = $("#NombreTC").val();
        data["Apellidos"] = $("#ApellidosTC").val();
        

        var Puestos = [];
        $.each($("#puestosTC").select2('data'), (index, value) => {
            Puestos.push(value.id);
        });
        data["Puestos"] = Puestos;
        data["SS"] = $("#SSR").val();
        data["DNI"] = $("#DNITC").val();
        data["Nacionalidad"] = $("#NacionalidadTC").val();
        data["IDProvincia"] = $("#ProvinciaTC").val();
        data["Telefono"] = $("#TelefonoTC").val();
        data["Email"] = $("#EmailTC").val();
        data["Localidad"] = $("#LocalidadTC").val();

        $.ajax({
            url: '/Personal/GuardarTrabajadorC',
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
                    window.location.href = '/ZonaPrivada/ZonaPrivada?menu=7';
                });
            },
            error: function (xhr, status, p3, p4) {
                alert("error");
            }
        });
    }
}
