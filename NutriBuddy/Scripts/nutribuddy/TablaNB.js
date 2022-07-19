$(document).ready(function () {
    //iniciarTabla();
});

function iniciarTabla(id, columns, data) {
    var language = getIdioma();
    var table = new DataTable(id, {
        responsive: true,
        "language": {
            "url": "/Scripts/Datatable/" + (language === 'es' ? "Spanish.json" : "English.json")
        },
        columns: columns,
        data: data
    });
}