$(document).ready(function () {
    $('#showPassword').on('click', function (e) {
        if (e.target.tagName.toUpperCase() === "LABEL") {
            return;
        }
        var passwordInput = document.getElementById("Pass");
        passwordInput.type = passwordInput.type === "password" ? "text" : "password";
    });



    $('#forgotPassword').on('click', function () {
        recoverPassword();
    });
});

function doLogin() {
    var formDataLogin = new FormData();
    formDataLogin.append("email", document.getElementById('Email').value);
    formDataLogin.append("password", document.getElementById('Pass').value);
        
    $.ajax({
        url: '/Home/DoLogin',
        type: "POST",
        data: formDataLogin,
        processData: false,
        contentType: false,
        async: false,
        success: function (data) {  
            if (data.flag === 0 && !data.FI) {
                window.location.href = '/ZonaPrivada/Home';
            } else if (data.flag === 0 && data.FI) {
                window.location.href = '/Paciente/FormularioInicial';
            } else {
                Swal.fire({
                    title: data.msg,
                    icon: 'error',
                    iconColor: '#f2233a',
                    backdrop: 'true',
                    customClass: {
                        confirmButton: 'btnLO',
                    }
                })
            }
        },
        error: function (xhr, status, p3, p4) {
            Swal.fire({
                title: msgGeneralError,
                icon: 'error',
                iconColor: '#f2233a',
                backdrop: 'true',
                customClass: {
                    confirmButton: 'btnLO',
                }
            })
        }
    });
}

function recoverPassword() {
    location.href = '/Home/RecoverPass';
}