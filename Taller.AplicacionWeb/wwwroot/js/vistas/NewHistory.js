let tabladata;
$(document).ready(function () {


    tabladata = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Reparacion/HistorialVehiculos',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "idHistorialCarro", "visible": false, "searchable": false },
            { "data": "idReparacion", "visible": false, "searchable": false },
            { "data": "usuario" },
            { "data": "placa" },
            { "data": "marca" },
            { "data": "descripcionDeLaReparacion", "searchable": false },
            {
                "data": "fechaDeInicio",
                "searchable": false,
                "render": function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },
            {
                "data": "fechaDeFin",
                "searchable": false,
                "render": function (data) {
                    var formattedDate = moment(data).format('DD/MM/YYYY');
                    if (formattedDate === "Invalid date") {
                        return "En reparación";
                    }
                    return formattedDate;
                }
            },
            { "data": "status", "searchable": false },
            { "data": "numberTracking", "searchable": false },
            {
                "defaultContent": '<button class="btn btn-info btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>' +
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


$("#tbdata tbody").on("click", ".btn-info", function () {

    let s = tabladata.row($(this).closest("tr")).data();


    $("#txtFechaRegistro").val(moment(s.fechaDeInicio).format('DD/MM/YYYY'));
    if (moment(s.fechaDeFin).isValid()) {
        $("#txtFechaSalida").val(moment(s.fechaDeFin).format('DD/MM/YYYY'));
    } else {
        $("#txtFechaSalida").val('En Reparación');
    }
    $("#txtNumeroSeguimiento").val(s.numberTracking)
    $("#txtPlaca").val(s.placa)
    $("#txtMarca").val(s.marca)
    $("#cboEstado").val(s.status)
    $("#txtID").val(s.idReparacion)
    $("#txtIDHis").val(s.idHistorialCarro)

    console.log("id Historial Carro: " + s.idHistorialCarro)
    $("#tbProductos tbody").html("")
    let subtotal = 0;

    s.tblDetalleCotizacions.forEach((item) => {

        var $fila = $("<tr>");

        subtotal += parseFloat(item.subtotal);

        $fila.append(
            $("<td>").text(item.descripcionProducto),
            $("<td>").text(item.cantidad),
            $("<td>").text(item.precio),
            $("<td>").text(item.subtotal)
        );


        $("#tbProductos tbody").append($fila);


    });
    $("#txtSubTotal").val(subtotal.toFixed(2));
    $("#txtIGV").val((subtotal * 0.13).toFixed(2));
    $("#txtTotal").val((parseFloat(subtotal) + (parseFloat(subtotal) * 0.13)).toFixed(2))
    $("#linkImprimir").attr("href", `/Reparacion/PDFReparacion?numberTracking=${s.numberTracking}`)


    $("#modalData").modal("show");
})
let filaSelect
$("#btnEditar").on("click", function () {
    let selectedRow = tabladata.row(filaSelect).data();

    const reparacion = {
        status: $("#cboEstado").val(),
        idReparacion: $("#txtID").val(),
        numberTracking: $("#txtNumeroSeguimiento").val(),
        idHistorialCarro: $("#txtIDHis").val()
    }

    fetch("/Reparacion/Editar", {
        method: "PUT",
        headers: { "Content-Type": "application/json; charset=utf-8" },
        body: JSON.stringify(reparacion)
    })
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            if (responseJson.estado == true) {
                const rowData = tabladata.row(filaSelect).data();
                rowData.status = responseJson.objeto.status; 
                tabladata.row(filaSelect).data(rowData).draw(false);
                filaSelect = null;
                $("#modalData").modal("hide");
                swal("Listo", "El estado fue editado", "success");
                tabladata.ajax.reload(); 
            } else {
                swal("Lo sentimos", responseJson.mensaje, "error");
            }
        });

});



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
        text: `Eliminar el historial vehiculo "${data.placa}"`,
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

            fetch(`/Reparacion/EliminarHistorial?idHistorialCarro=${data.idReparacion}`, {
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
