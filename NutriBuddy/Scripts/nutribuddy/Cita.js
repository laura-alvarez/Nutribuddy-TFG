$(document).ready(function () {
    $("#citaModal").prependTo("body");

    $("#verAnterior").on('click', function () {
        $('#citaModal').modal();
    });

    $("#volverCita").on('click', function () {
        window.location.href = '/ZonaPrivada/ZonaPrivada?menu=5';
    });

    $("#terminarCita").on('click', function () {
        guardarCita();
    });
});

function guardarCita() {
    var fdCita = new FormData();
    fdCita.append("ID", $("#IDCita").val());
    fdCita.append("Notas", $("#comentarios").val());
    fdCita.append("Pautas", $("#comentarios").val());

    $.ajax({
        url: '/Agenda/CompletarCita',
        type: "POST",
        data: fdCita,
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
                window.location.href = '/ZonaPrivada/ZonaPrivada?menu=5';
            });
        },
        error: function (xhr, status, p3, p4) {
            alert("error");
        }
    });
}