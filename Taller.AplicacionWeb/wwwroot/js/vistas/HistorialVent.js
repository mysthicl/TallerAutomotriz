const Busqueda = {


    busquedaFecha:()=>{
        $("#txtFechaInicio").val("")
        $("#txtFechaFin").val("")
        $("#txtNumeroVenta").val("")

        $(".busqueda-fecha").show()
        $(".busqueda-venta").hide()
    }, busquedaVenta:()=>{
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

    $("#txtFechaInicio").datepicker({dateFormat:"dd/mm/yy"})
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

    


    fetch(`/Venta/Historial?numeroVenta=${numeroVenta}&fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`)
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
                                $("<td>").text(venta.fecha),
                                $("<td>").text(venta.numeroVenta),
                                $("<td>").text(venta.documentoCliente),
                                $("<td>").text(venta.nombreCliente),
                                $("<td>").text(parseFloat(venta.totalDeLaVenta).toFixed(2)),
                                $("<td>").append(
                                    $("<button>").addClass("btn btn-info btn-sm").append(
                                        $("<i>").addClass("fas fa-eye")
                                    ).data("venta",venta)
                                )
                            
                            )
                        )
                    })
                }
            })


});

$("#tbventa tbody").on("click", ".btn-info", function () {

    let data = $(this).data("venta")

    $("#txtFechaRegistro").val(data.fecha)
    $("#txtNumVenta").val(data.numeroVenta)
    $("#txtUsuarioRegistro").val(data.usuario)
    $("#txtDocumentoCliente").val(data.documentoCliente)
    $("#txtNombreCliente").val(data.nombreCliente)
    $("#txtSubTotal").val(parseFloat(data.totalDeLaVenta) - parseFloat(data.totalDeLaVenta) * 0.13)
    $("#txtIGV").val(parseFloat(data.totalDeLaVenta) * 0.13)
    $("#txtTotal").val(parseFloat(data.totalDeLaVenta).toFixed(2))

    $("#tbProductos tbody").html("")



    data.tblDetalleVenta.forEach((item) => {
        $("#tbProductos tbody").append(
            $("<td>").text(item.fecha),
            $("<td>").text(item.numeroVenta),
            $("<td>").text(item.documentoCliente),
            $("<td>").text(item.nombreCliente)

        )
    })

    $("#linkImprimir").attr("href", `/Venta/MostrarPDFVenta?numeroVenta=${s.numeroVenta}`)
    $("#modalData").modal("show");
})