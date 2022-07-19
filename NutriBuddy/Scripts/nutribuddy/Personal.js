$(document).ready(function () {   

    $('#anyadirProfesional').on('click', function () {
        window.location.href = '/Personal/GUsuario?id=0';
    });

    getTrabajadoresTNB();
});

var estructuraTabla = [
    { data: 'ID' },
    { data: 'Nombre' },
    { data: 'Apellidos' },
    { data: 'Provincia' },
    { data: 'Email' },
    {
        data: 'ID', orderable: false, render:
            function (data, type, row, meta) {
                return '<span id="editar_' + data + '" title="Editar empleado" onclick="editarEmpleado(' + data + ')"><i class="fa-solid fa-pencil editar iconoTabla" ></i ></span>' +
                    '<span id="eliminar_' + data + '" title="Eliminar empleado" onclick="eliminarEmpleado(' + data + ')"><i class="fa-solid fa-trash-can eliminar iconoTabla"></i></span> ';
            }

    },
];

function getTrabajadoresTNB() {
    $.ajax({
        url: '/Personal/GetTrabajadores',
        type: "GET",
        async: false,
        success: function (data) {
            iniciarTabla('#tablaPersonal', estructuraTabla, data.Trabajadores);
        },
    });
}

function editarEmpleado(id) {
    window.location.href = '/Personal/GUsuario?id='+id;
}

function eliminarEmpleado(id) {
    Swal.fire({
        title: '¿Seguro que quieres dar de baja el empleado?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí',
        cancelButtonText: 'No',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Personal/EliminarEmpleado?id=' +id,
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