$(document).ready(function () {
    var url = window.location.href;
    if (url.includes('ZonaPrivada')) {        
        $('#anyadirPaciente').on('click', function () {
            window.location.href = '/Paciente/GPaciente?id=0';
        });
        getPacientes();
    }
});

var estructuraTabla = [
    { data: 'ID' },
    { data: 'Nombre' },
    { data: 'ProxCitaS' },
    { data: 'Email' },
    { data: 'Telefono' },
    {
        data: 'ID', orderable: false, render:
            function (data, type, row, meta) {
                return '<span id="editar_' + data + '" title="Editar paciente" onclick="editarPaciente(' + data + ')"><i class="fa-solid fa-pencil editar iconoTabla" ></i ></span>' +
                    '<span id="eliminar_' + data + '" title="Eliminar paciente" onclick="eliminarPaciente(' + data + ')"><i class="fa-solid fa-trash-can eliminar iconoTabla"></i></span> ';
            }

    },
];

function getPacientes() {
    $.ajax({
        url: '/Paciente/GetPacientes',
        type: "GET",
        async: false,
        success: function (data) {
            iniciarTabla('#tablaPacientes', estructuraTabla, data.pacientes);
        },
    });
}

function editarPaciente(id) {
    window.location.href = '/Paciente/GPaciente?id='+id;
}

function eliminarPaciente(id) {
    Swal.fire({
        title: '¿Seguro que quieres dar de baja el paciente?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí',
        cancelButtonText: 'No',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Paciente/EliminarPaciente?id=' + id,
                type: 'DELETE',
                success: function (data) {
                    Swal.fire({
                        title: data.msg,
                        icon: 'success',
                        iconColor: '#f2233a',
                        backdrop: 'true',
                        customClass: {
                            confirmButton: 'btnLO',
                        }
                    }).then((result) => {
                        location.reload();
                    });
                }
            });
        }
    })
}

function guardarPaciente() {
    if (validador()) {
        var formP = new FormData();
        formP.append("ID", $("#ID").val());
        formP.append("Nombre", $("#Nombre").val());
        formP.append("Apellidos", $("#Apellidos").val());
        formP.append("Email", $("#Email").val());
        formP.append("DNI", $("#Dni").val());
        formP.append("Direccion", $("#Direccion").val());
        formP.append("IDProvincia", $("#Provincia").val());
        formP.append("Telefono", $("#Telefono").val());
        formP.append("FNacimiento", $("#FechaNacimiento").val());
        formP.append("EsHombre", $('input[type=radio][name=gender]:checked').val() === 'masculino') ? true : false;

        $.ajax({
            url: '/Paciente/GuardarPaciente',
            data: formP,
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
                        location.href = '/ZonaPrivada/ZonaPrivada?menu=8';
                    }
                });
            },
            error: function (xhr, status, p3, p4) {
                alert("error");
            }
        });
    }
}

function validador() {
    let correcto = true;
    if (!validarSoloLetras($("#Nombre").val())) {
        backgroundInputValidated("#Nombre");
        correcto = false;
    }

    if (!validarSoloLetras($("#Apellidos").val())) {
        backgroundInputValidated("#Apellidos");
        correcto = false;
    }
    var aux = $("#Telefono").val();
    if (!validarSoloNumeros(aux) || aux.length != 9) {
        backgroundInputValidated("#Telefono");
        correcto = false;
    }
    if ($("#Provincia").val() == -1) {
        backgroundInputValidated("#Provincia");
        correcto = false;
    }
    if (!validarEmail($("#Email").val())) {
        backgroundInputValidated("#Email");
        correcto = false;
    }
    if (!validadorDNI($("#Dni").val())) {
        backgroundInputValidated("#Dni");
        correcto = false;
    }
    if ($("#Direccion").val() == "") {
        backgroundInputValidated("#Direccion");
        correcto = false;
    }
    return correcto;
}

function backgroundInputValidated(id) {
    $(id).css("background-color", "#dd8b85");
}
