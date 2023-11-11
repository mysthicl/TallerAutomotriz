
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

    $("#modalData").modal("show");
})

$("#btnEditar").on("click", function () {
    let s = tabladata.row($(this).closest("tr")).data();



    const reparacion = {
        status: $("#cboEstado").val(),
        idReparacion: s.numberTracking
    }

   

    fetch("/Reparacion/Editar", {
        method: "PUT",
        headers: { "Content-Type": "application/json; charset=utf-8" },
        body: JSON.stringify(reparacion)
    })
        .then(response => {
            $("#modalData").find("div.modal-content").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            if (responseJson.estado == true) {

                tabladata.row(filaSelect).data(responseJson.objeto).draw(false);

                filaSelect = null;
                console.log(responseJson.objeto.idRol)
                $("#modalData").modal("hide")
                swal("Listo", "El estado fue editado", "success")
            } else {
                swal("Lo sentimos", responseJson.mensaje, "error")
            }
        })

})
