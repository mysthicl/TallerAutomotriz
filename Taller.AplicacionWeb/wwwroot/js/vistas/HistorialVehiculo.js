const modeloBase = {
    idHistorialCarro: 0,
    placa: "",
    marca: "",
    descripcionDeLaReparacion: "",
    fechaDeInicio: "",
    status: ""
}

let tabladata;

$(document).ready(function () {


    tabladata = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Vehiculo/HistorialVehiculoLista',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "idHistorialCarro", "visible": false, "searchable": false },
            { "data": "placa"},
            { "data": "marca", "searchable": false },
            { "data": "descripcionDeLaReparacion", "searchable": false },
            { "data": "fechaDeInicio", "searchable": false },
            { "data": "status", "searchable": false },
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


$("#tbdata tbody").on("click", ".btn-eliminar", function () {
    let fila;
    if ($(this).closest("tr").hasClass("child")) {
        filaSelect = $(this).closest("tr").prev();
    } else {
        filaSelect = $(this).closest("tr");
    }

    const data = tabladata.row(filaSelect).data();
    swal({
        title: "Esta seguro?",
        text: `Eliminar el historial vehiculo "${data.usuario}"`,
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Si, eliminar",
        camcelButtonText: "Cancelar",
        closeOnConfirm: false,
        closeOnCancel: true

    }, function (respuesta) {
        if (respuesta) {
            $(".showSweetAlert").LoadingOverlay("show");

            fetch(`/Vehiculo/EliminarHistorialVehiculo?IdReparacion=${data.idCarro}`, {
                method: "DELETE"
            })
                .then(response => {
                    $(".showSweetAlert").LoadingOverlay("hide");
                    return response.ok ? response.json() : Promise.reject(response)
                })
                .then(responseJson => {
                    if (responseJson.estado == true) {

                        tabladata.row(filaSelect).remove().draw();
                        swal("Listo", "El vehiculo fue eliminado", "success")

                    } else {
                        swal("Lo sentimos", "no se pudo eliminar el usuario por estar relacionado en otra tabla >:v", "error")

                    }
                })

        }
    })
})



