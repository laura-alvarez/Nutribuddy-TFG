var idCentro;
$(document).ready(function () { 

    $('.buttonCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
        var active = $('#sidebar').hasClass('active');
        saveActiveSidebarState(active);
    });

    $('#cerrarSesion').on('click', function () {
        var idioma = getIdioma();
        Swal.fire({
            title: idioma == 'es' ? '¿Estás seguro de que quieres salir?' :'Are you sure you want to go out?',
            icon: 'warning',
            iconColor: '#ff9f3c',
            backdrop: 'true',
            customClass: {
                confirmButton: 'btnLO',
                cancelButton: 'btnLO',
            },
            showCancelButton: true,
            confirmButtonText: idioma == 'es'?'Cerrar sesión':'Close session',
            cancelButtonText: idioma == 'es' ?'Cancelar':'Cancel',
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
                window.location.href = '/ZonaPrivada/CerrarSesion';
            }
        });

    });

    $("#cambiarCentroModal").prependTo("body");

    $('.cambiarCentroLink').on('click', function () {
        $('#cambiarCentroModal').modal();
    });

    idCentro = $("#centroDestino").val();

    $("#centroDestino").on('change', function () {
        idCentro = $("#centroDestino").val();
        $.ajax({
            url: '/ZonaPrivada/CambiarCentro?IDCentro=' + idCentro,
            type: "GET",
            async: false,
            success: function () {
                $("#centroActual").text($("#centroDestino option:selected").text());                
            },
            complete: function () {
                $("#cambiarCentroModal").modal('hide');
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                window.location.reload();
            }
        });
      
    });
});

function saveActiveSidebarState(active) {
    $.ajax({
        url: '/ZonaPrivada/ActualizarEstadoMenu?Collapse=' + active,
        type: "GET",        
        async: true     
    });
}

