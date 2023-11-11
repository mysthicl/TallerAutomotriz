﻿window.addEventListener('DOMContentLoaded', function () {
    fetch("/Cotizacion/NumeroCotizacion")
        .then(response => {
            return response.ok ? response.text() : Promise.reject(response);
        })
        .then(numeroCotizacion => {
            document.getElementById("lbfactura").textContent = `#${numeroCotizacion}`;
        })
        .catch(error => {
            console.error("Error al obtener el número de venta:", error);
        });
});

$(document).ready(function () {

    $("#cboBuscarProducto").select2({
        ajax: {
            url: "/Cotizacion/ObtenerProductos",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            delay: 250,
            data: function (params) {
                return {
                    busqueda: params.term
                };
            },
            processResults: function (data) {


                return {
                    results: data.map((item) => (
                        {
                            id: item.idProducto,
                            text: item.descripcion,
                            codigoProducto: item.codigoProducto,
                            nombre: item.nombre,
                            precio: parseFloat(item.precio),
                            cantidadEnStock: parseInt(item.cantidadEnStock),
                            urlImagen: item.urlImagen,
                            valor: item.valor
                        }
                    ))
                };
            },
        },
        language: "es",
        placeholder: 'Buscar Producto...',
        minimumInputLength: 1,
        templateResult: formatoResultado

    });

    function formatoResultado(data) {
        if (data.loading)
            return data.text;



        var contenedor = $(`
        <table width="100%">
            
            <tr>
                <td style="width:60px">
                    <img style="height:60px;width:60px;margin-right:10px" src="${data.urlImagen}"/>
                </td>
                <td>
                    <p style="font-weight:bolder;margin:2px;">${data.nombre}</p>
                    <p style="margin:2px">${data.text}</p>
                 </td>
                
            </tr>
        </table>
        `);

        return contenedor;
    }


})

$(document).on("select2:open", function () {
    document.querySelector(".select2-search__field").focus();
})


let productosDetalleVenta = [];

$("#cboBuscarProducto").on("select2:select", function (e) {
    const data = e.params.data;

    let productoExiste = productosDetalleVenta.filter(p => p.idProducto == data.id);


   
    if (productoExiste.length > 0) {
        $("#cboBuscarProducto").val("").trigger("change");
        toastr.warning("", "El producto ya fue agregado");
        return false;
    }
    console.log("valor: " + data.valor)

    if (data.valor == "Servicio") {
        toastr.warning("", "Servicio Agregado")
        console.log("valor: " + productoExiste.valor)
        let producto = {
            idProducto: data.id,
            nombre: data.nombre,
            descripcion: data.text,
            precio: data.precio.toString(),
            subTotal: (parseFloat(1) * data.precio).toString(),
            cantidad: 1
        }

        productosDetalleVenta.push(producto)

        mostrarProductosPrecios();
        $("#cboBuscarProducto").val("").trigger("change")
        return false;
    }


    swal({
        title: data.nombre,
        text: data.text,
        imageUrl: data.urlImagen,
        type: "input",
        showCancelButton: true,
        closeOnConfirm: false,
        inputPlaceholder: "Ingrese la cantidad"

    }, function (valor) {

        //console.log(valor)


        if (valor === false) {
            $("#cboBuscarProducto").val("").trigger("change")
            return false
        }



        if (valor == "") {
            toastr.warning("", "Necesita ingresar la cantidad")
            return false;
        }

        if (parseFloat(valor) <= 0) {
            toastr.warning("", "Ingrese un valor mayor a 0")
            return false;
        }


        if (isNaN(parseInt(valor))) {
            toastr.warning("", "Ingrese un valor numerico");
            return false;
        }
        console.log(data.cantidadEnStock)

        if (parseInt(valor) > data.cantidadEnStock) {
            toastr.warning("", "Stock insuficiente");
            return false;
        }


        let producto = {
            idProducto: data.id,
            descripcion: data.text,
            codigoProducto: data.codigoProducto,
            nombre: data.nombre,
            precio: data.precio.toString(),
            subTotal: (parseFloat(valor) * data.precio).toString(),
            cantidad: (parseFloat(valor)),

        }
        console.log(producto)

        productosDetalleVenta.push(producto)

        mostrarProductosPrecios();
        $("#cboBuscarProducto").val("").trigger("change")
        swal.close()

    })
})


function mostrarProductosPrecios() {

    let total = 0;
    let iva = 0;
    let sutotal = 0;
    let cambio = 0;
    $("#tbProducto tbody").html("")
    productosDetalleVenta.forEach((item) => {
        total = total + parseFloat(item.subTotal);

        $("#tbProducto tbody").append(
            $("<tr>").append(
                $("<td>").append(
                    $("<button>").addClass("btn btn-danger btn-eliminar btn-sm").append(
                        ($("<i>").addClass("fas fa-trash-alt"))

                    ).data("idProducto", item.idProducto)
                ),
                $("<td>").text(item.nombre),
                $("<td>").text(item.descripcion),
                $("<td>").text(item.cantidad),
                $("<td>").text(item.precio),
                $("<td>").text(item.subTotal)
            )
        )


    })
    const pago = document.getElementById("txtTotalPagado");
    console.log("Total:" + total)
    iva = parseFloat(total) * 0.13;
    sutotal = iva + total;
    $("#txtTotalPagar").text(sutotal.toFixed(2))
    $("#txtSaldoPendiente").text(iva.toFixed(2))


    


}
$(document).on("click", "button.btn-eliminar", function () {
    console.log("Sirve")

    const _idproducto = $(this).data("idProducto");

    productosDetalleVenta = productosDetalleVenta.filter(p => p.idProducto != _idproducto);
    console.log(_idproducto)
    mostrarProductosPrecios();



})






$("#btnTerminarVenta").click(function () {
    if (productosDetalleVenta < 1) {
        toastr.warning("", "Debe ingresar productos");
        return;
    }

    var fechaActual = new Date();
    var dia = fechaActual.getDate();
    var mes = fechaActual.getMonth() + 1;
    var anio = fechaActual.getFullYear();

    var fechaFormateada = dia + '/' + mes + '/' + anio;
    console.log(fechaFormateada);

    const TblDetalleCompraa = productosDetalleVenta;
  

    const cotizacion = {
        totalDeLaCotizacion: $("#txtTotalPagar").text(),
        tblDetalleCotizacions: TblDetalleCompraa,
        fechaCotizacion: fechaFormateada
    }

    $("#btnTerminarVenta").LoadingOverlay("show");



   
    
    fetch("/Cotizacion/RegistraCotizacion", {
        method: "POST",
        headers: { "Content-Type": "application/json; charset=utf-8" },
        body: JSON.stringify(cotizacion)
    })
        .then(response => {
            $("#btnTerminarVenta").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {

            if (responseJson.estado) {
                productosDetalleVenta = [];
                mostrarProductosPrecios();
                swal("Creado", `Numero Cotizacion:${responseJson.objeto.numeroCotizacion}`, "success")

                fetch("/Cotizacion/NumeroCotizacion")
                    .then(response => {
                        console.log(response)
                        return response.ok ? response.text() : Promise.reject(response);
                    })
                    .then(numeroCompra => {
                        document.getElementById("lbfactura").textContent = `#${numeroCompra}`;

                    })
                    .catch(error => {
                        console.error("Error al obtener el número de venta:", error);
                    });


            } else {
                swal("Lo sentimos", `No se pudo registrar la venta`, "error")
            }

        }).catch(error => {
            console.error("Error al obtener el número de venta:", error);
        });

})