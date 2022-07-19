$(document).ready(function () {
    $("#privateZone").on("click", function () { goLogin(); });

    $('#buscador_tipo').change(function () { buscadorCentros(); });

    $('#buscador_especilidades').change(function () { buscadorCentros(); });

    $('#buscador_localizacion').change(function () { buscadorCentros(); });

    $("#btnRegistro").on("click", function () { irRegistro(); });
    
});



function goLogin() {
    location.href = '/Home/Login';
}

function irRegistro() {
    location.href = '/Home/Registro';
}

function irIndex() {
    location.href = '/Home/Index';
}

function formularioContacto() {
    var Nombre = document.getElementById('contactoNombre').value;
    var Email = document.getElementById('contactoEmail').value;
    var Telefono = document.getElementById('contactoTelefono').value;
    var Comentario = document.getElementById('contactoComentario').value;
    var QuienContacta = document.querySelector('input[type="radio"]:checked').value;
    var formContact = new FormData();
    formContact.append("Nombre", Nombre);
    formContact.append("Email", Email);
    formContact.append("Telefono", Telefono);
    formContact.append("Comentario", Comentario);
    formContact.append("QuienContacta", QuienContacta);

    $.ajax({
        url: '/Home/Contacto',
        type: "POST",
        data: formContact,
        processData: false,
        contentType: false,
        async: false,
        success: function (data) {
            if (data.flag === 0) {
                $("#contactoNombre").val("");
                $("#contactoEmail").val("");
                $("#contactoTelefono").val("");
                $("#contactoComentario").val("");
                Swal.fire({
                    title: 'Mensaje enviado, nos pondremos en contacto contigo lo antes posible',
                    icon: 'success',
                    iconColor: '#f2233a',
                    backdrop: 'true',
                    customClass: {
                        confirmButton: 'btnLO',
                    }
                })
            } else {
                Swal.fire({
                    title: 'Ha ocurrido un error',
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
                title: 'erroe',
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


function buscadorCentros() {
    var bTipo = $("#buscador_tipo").val();
    var bEsp = $("#buscador_especilidades").val();
    var bLoc = $("#buscador_localizacion").val();
    window.location.href = '/Home/BuscarCentros?bTipo=' + bTipo + '&bEsp=' + bEsp + '&bLoc=' + bLoc;
}