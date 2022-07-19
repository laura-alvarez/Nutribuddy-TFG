$(document).ready(function () {
    $('#showPassword').on('click', function (e) {
        if (e.target.tagName.toUpperCase() === "LABEL") {
            return;
        }
        var passwordInput = document.getElementById("Pass");
        passwordInput.type = passwordInput.type === "password" ? "text" : "password";
    });

    $('#SendCode').on('click', function () {
        sendCode();
    });

    $('#comeBack').on('click', function () {
        comeBack();
    });

    $('#changePass').on('click', function () {
        cambiarContrasenya();
    });

});

function comeBack() {
    location.href = '/Home/Login';
}

function sendCode() {
    if (validarEmail($("#Email").val())) {

        $.ajax({
            url: '/Home/EnviarCodigoRecuperacionContrasenya?Email=' + $("#Email").val(),
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
                        $("#secondStep").removeClass("hidden");
                        $("#SendCode").prop("disabled", true);
                        $("#Email").prop("readonly", true);
                    }
                });
            },
            error: function (xhr, status, p3, p4) {
                alert("error");
            }
        });
    } else {
        alert("errores");
    }
}

function cambiarContrasenya() {
    if (validarEmail($("#Email").val())) {
        var formCambioContrasenya = new FormData();

        formCambioContrasenya.append("Email", $("#Email").val());
        formCambioContrasenya.append("CodigoRecuperacion", $("#RecoverCode").val());
        formCambioContrasenya.append("NuevaContrasenya", $("#Pass").val());


        $.ajax({
            url: '/Home/CambiarContrasenya',
            data: formCambioContrasenya,
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
    } else {
        alert("errores");
    }
}