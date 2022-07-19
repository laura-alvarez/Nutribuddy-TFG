$(document).ready(function () {
    $("#guardarTrabajador").on('click', function () {
        guardarTrabajador();
    });
});



function validador() {
    let correcto = true;
    if (!validarSoloLetras($("#NombreTNB").val())) {
        backgroundInputValidated("#NombreTNB");
        correcto = false;
    }

    if (!validarSoloLetras($("#ApellidosTNB").val())) {
        backgroundInputValidated("#ApellidosTNB");
        correcto = false;
    }  
    var aux = $("#SSR").val();
    if (!validarSoloNumeros(aux) || aux.length > 12 || aux.length < 0) {
        backgroundInputValidated("#SSR");
        correcto = false;
    }   
    if ($("#ProvinciaTNB").val() == -1) {
        backgroundInputValidated("#ProvinciaTNB");
        correcto = false;
    }
    if (!validarEmail($("#EmailTNB").val())) {
        backgroundInputValidated("#EmailTNB");
        correcto = false;
    }
    return correcto;
}

function backgroundInputValidated(id) {
    $(id).css("background-color", "#dd8b85");
}

function guardarTrabajador() {
    if (validador()) {
        var formDataTNB = new FormData();
        formDataTNB.append("ID", $("#idTrabajador").val());
        formDataTNB.append("Nombre", $("#NombreTNB").val());
        formDataTNB.append("Apellidos", $("#ApellidosTNB").val());
        formDataTNB.append("SSR", $("#SSR").val());
        formDataTNB.append("IDProvincia", $("#ProvinciaTNB").val());
        formDataTNB.append("Email", $("#EmailTNB").val());

        $.ajax({
            url: '/Personal/GuardarTrabajadorNB',
            type: "POST",
            data: formDataTNB,
            processData: false,
            contentType: false,
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
