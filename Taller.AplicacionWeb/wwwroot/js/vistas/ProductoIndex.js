const modeloBase = {
    CodigoProducto: "",
    Nombre: "",
    Descripcion: "",
    Precio: 0,
    CantidadEnStock: 0,
    UrlImagen: ""
}

let tabladata;

$(document).ready(function () {

      
    tabladata = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Producto/lista',
            "type": "GET",
            "datatype": "json",
            "error": function (xhr, error, thrown) {
                console.log("Error al cargar los datos: " + thrown);
        },
        "columns": [
            { "data": "idProducto", "visible": false, "searchable": false },
            {
                "data": "urlImagen", render: function (data) {
                    return `<img style="height:60px" src=${data} class="rounded mx-auto d-block"/>`
                }
            },
            { "data": "nombre"},
            { "data": "descripcion"},
            { "data": "precio"},
            { "data": "cantidadEnStock"},
            {
                "defaultContent": '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>' +
                    '<button class="btn btn-danger btn-eliminar btn-sm"><i class="fas fa-trash-alt"></i></button>',
                "orderable": false,
                "searchable": false,
                "width": "80px"
            }
        ],
        order: [[0, "desc"]],

        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },


    });

})