const Busqueda = {

    busquedaFecha: () => {
        $("#txtFechaInicio").val("")
        $("#txtFechaFin").val("")
        $("#txtNumeroVenta").val("")

        $(".busqueda-fecha").show()
        $(".busqueda-venta").hide()
    }, busquedaVenta: () => {
        $("#txtFechaInicio").val("")
        $("#txtFechaFin").val("")
        $("#txtNumeroVenta").val("")

        $(".busqueda-fecha").hide()
        $(".busqueda-venta").show()
    }
}

$(document).ready(function () {
    Busqueda["busquedaFecha"]()


    $.datepicker.setDefaults($.datepicker.regional["es"])

    $("#txtFechaInicio").datepicker({ dateFormat: "dd/mm/yy" })
    $("#txtFechaFin").datepicker({ dateFormat: "dd/mm/yy" })

})

$("#cboBuscarPor").change(function () {
    if ($("#cboBuscarPor").val() == "fecha") {
        Busqueda["busquedaFecha"]()
    } else {
        Busqueda["busquedaVenta"]()
    }

})

$("#btnBuscar").click(function () {

    if ($("#cboBuscarPor").val() == "fecha") {
        if ($("#txtFechaInicio").val().trim() == "" || $("#txtFechaFin").val().trim() == "") {
            toastr.warning("", "Debe ingresar una fecha")
            return;
        }
    } else {
        if ($("#txtNumeroVenta").val().trim() == "") {
            toastr.warning("", "Debe ingresar el numero de venta")
            return;
        }
    }

    let numeroVenta = $("#txtNumeroVenta").val()
    let fechaInicio = $("#txtFechaInicio").val()
    let fechaFin = $("#txtFechaFin").val()


    $(".card-body").find("div.row").LoadingOverlay("show");




    fetch(`/Cotizacion/Historial?numeroCotizacion=${numeroVenta}&fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`)
        .then(response => {
            $(".card-body").find("div.row").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            $("#tbventa tbody").html("");
            if (responseJson.length > 0) {
                responseJson.forEach((venta) => {
                    $("#tbventa tbody").append(
                        $("<tr>").append(
                            $("<td>").text(venta.fechaCotizacion),
                            $("<td>").text(venta.numeroCotizacion),
                            $("<td>").text(parseFloat(venta.totalDeLaCotizacion).toFixed(2)),
                            $("<td>").append(
                                $("<button>").addClass("btn btn-info btn-sm").append(
                                    $("<i>").addClass("fas fa-eye")
                                ).data("venta", venta)
                            )

                        )
                    )
                })
            }
        })


});

$("#tbventa tbody").on("click", ".btn-info", function () {

    let s = $(this).data("venta")

    $("#txtFechaRegistro").val(s.fechaCotizacion)
    $("#txtNumVenta").val(s.numeroCotizacion)
    $("#txtUsuarioRegistro").val(s.usuario)


    $("#txtTotal").val(parseFloat(s.totalDeLaCotizacion).toFixed(2))

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
    $("#linkImprimir").attr("href", `/Cotizacion/MostrarPDFCotizacion?numeroCotizacion=${s.numeroCotizacion}`)


    $("#modalData").modal("show");
})