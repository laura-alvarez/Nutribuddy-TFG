$(document).ready(function () {
    $("#FechaNacimiento").attr("max", new Date().toISOString().split("T")[0])
    $("#btnVolver").on("click", function () { irIndex(); });
    $('#showPassword').on('click', function (e) {
        if (e.target.tagName.toUpperCase() === "LABEL") {
            return;
        }
        var passwordInput = document.getElementById("Contrasenya");
        passwordInput.type = passwordInput.type === "password" ? "text" : "password";
    });
    $("input,select").not(".radioButton").on("focus", function (e) {

        $("#" + e.currentTarget.id).css("background-color", "#ffffff");
    });

    $('input[type=radio][name=centroAsociado]').change(function () {
        if (this.value == 'si') {
            $("#divCentroAsociado").removeClass("hidden");
        }
        else if (this.value == 'no') {
            $("#divCentroAsociado").addClass("hidden");
        }
    });

    $('#CentrosProvincia').on('change', function () {
        if (this.value != -1) {
            $("#SelectorCentro").empty();
            obtenerCentros(this.value);
        } else {
            $("#SelectorCentro").empty();
            $("#divSelectorCentros").addClass("hidden");
        }

    });
});

function irIndex() {
    location.href = '/Home/Index';
}

function registrar() {

    
        if (validar()) {
        var formRegistrar = new FormData();
        formRegistrar.append("Nombre", $("#Nombre").val());
        formRegistrar.append("Apellidos", $("#Apellidos").val());
        formRegistrar.append("Email", $("#Email").val());
        formRegistrar.append("Contrasenya", $("#Contrasenya").val());
        formRegistrar.append("Dni", $("#Dni").val());
        formRegistrar.append("Direccion", $("#Direccion").val());
        formRegistrar.append("Provincia", $("#Provincia").val());
        formRegistrar.append("Telefono", $("#Telefono").val());
        formRegistrar.append("FechaNacimiento", $("#FechaNacimiento").val());
        formRegistrar.append("EsHombre", $('input[type=radio][name=gender]:checked').val() === 'masculino') ? true : false;
        formRegistrar.append("Centro", $("#SelectorCentro").val());

        $.ajax({
            url: '/Home/Registrar',
            data: formRegistrar,
            type: "POST",
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
                    if (data.flag === 0) {
                        location.href = '/Home/Login';
                    }
                });
            },
            error: function (xhr, status, p3, p4) {
                alert("error");
            }
        });
    }

}

function validar() {
    let correcto = true;
    if (!validarSoloLetras($("#Nombre").val())) {
        backgroundInputValidated("#Nombre");
        correcto = false;
    }
    if (!validarSoloLetras($("#Apellidos").val())) {
        backgroundInputValidated("#Apellidos");
        correcto = false;
    }
    if (!validarEmail($("#Email").val())) {
        backgroundInputValidated("#Email");
        correcto = false;
    }
    if ($("#Contrasenya").val().length < 8) {
        backgroundInputValidated("#Contrasenya");
        correcto = false;
    }
    if ($("#Dni").val().length != 9) {
        backgroundInputValidated("#Dni");
        correcto = false;
    }
    if ($("#Direccion").val() === "") {
        backgroundInputValidated("#Direccion");
        correcto = false;
    }
    if ($("#Provincia").val() === "-1") {
        backgroundInputValidated("#Provincia");
        correcto = false;
    }
    let aux = $("#Telefono").val();
    if (!validarSoloNumeros(aux) || aux.length > 9 || aux.length < 0) {
        backgroundInputValidated("#Telefono");
        correcto = false;
    }
    if ($("#FechaNacimiento").val() === "") {
        backgroundInputValidated("#FechaNacimiento");
        correcto = false;
    }
    if ($('input[type=radio][name=centroAsociado]:checked').val() === 'si') {
        if ($('#CentrosProvincia').val() === "-1") {
            backgroundInputValidated("#CentrosProvincia");
            correcto = false;
        }
        if ($("#SelectorCentro").val() === "-1") {
            backgroundInputValidated("#SelectorCentro");
            correcto = false;
        }
    }


    if (!correcto) {
        Swal.fire({
            title: "Algunos campos son incorrectos",
            icon: 'error',
            iconColor: '#f2233a',
            backdrop: 'true',
            customClass: {
                confirmButton: 'btnLO',
            }
        });
    } else {
        var emailEnBBDD = comprobarEmailEnBBDD($("#Email").val());
        if (emailEnBBDD === undefined) {
            return false;

        }
        if (emailEnBBDD) {
            correcto = false;
            Swal.fire({
                title: "El email con el que se intenta registar ya est&aacute; en nuestra base de datos",
                icon: 'error',
                iconColor: '#f2233a',
                backdrop: 'true',
                customClass: {
                    confirmButton: 'btnLO',
                }
            });
        }
    }
    return correcto;
}

function comprobarEmailEnBBDD(email) {
    var emailEnBBDD = undefined;

    $.ajax({
        url: '/Home/EmailEnBBDD?Email=' + email,
        type: "POST",
        processData: false,
        contentType: false,
        async: false,
        success: function (data) {
            if (data.flag === 0) {
                emailEnBBDD = data.emailEnBBDD;
            } else {
                Swal.fire({
                    title: "Error al intentar consultar el email en nuestra base de datos",
                    icon: 'error',
                    iconColor: '#f2233a',
                    backdrop: 'true',
                    customClass: {
                        confirmButton: 'btnLO',
                    }
                });
            }
        },
        error: function (xhr, status, p3, p4) {
            Swal.fire({
                title: "Error al intentar consultar el email en nuestra base de datos",
                icon: 'error',
                iconColor: '#f2233a',
                backdrop: 'true',
                customClass: {
                    confirmButton: 'btnLO',
                }
            });
        }
    });

    return emailEnBBDD;
}

function backgroundInputValidated(id) {
    $(id).css("background-color", "#dd8b85");
}

function obtenerCentros(idProvincia) {
    $.ajax({
        url: '/Home/ObtenerCentros?IdProvincia=' + idProvincia,
        type: "POST",
        processData: false,
        contentType: false,
        async: false,
        success: function (data) {
            if (data.flag === 0) {
                $("#SelectorCentro").append(new Option("--", "-1"));
                var c = JSON.parse(data.centros);
                $.each(c, function (key, value) {
                    $("#SelectorCentro").append(new Option(value, key));
                });
                $("#divSelectorCentros").removeClass("hidden");
            } else if (data.flag === 1) {
                Swal.fire({
                    title: "Error al intentar consultar los centros en nuestra base de datos",
                    icon: 'error',
                    iconColor: '#f2233a',
                    backdrop: 'true',
                    customClass: {
                        confirmButton: 'btnLO',
                    }
                });
            } else if (data.flag === 2) {
                Swal.fire({
                    title: "La provincia seleccionada no tiene centros asociados",
                    icon: 'error',
                    iconColor: '#f2233a',
                    backdrop: 'true',
                    customClass: {
                        confirmButton: 'btnLO',
                    }
                });
                $('#CentrosProvincia').val("-1").change();
                $("#SelectorCentro").empty();
                $("#divSelectorCentros").addClass("hidden");
            }
        },
        error: function (xhr, status, p3, p4) {
            Swal.fire({
                title: "Error al intentar consultar los centros en nuestra base de datos",
                icon: 'error',
                iconColor: '#f2233a',
                backdrop: 'true',
                customClass: {
                    confirmButton: 'btnLO',
                }
            });
        }
    });
}