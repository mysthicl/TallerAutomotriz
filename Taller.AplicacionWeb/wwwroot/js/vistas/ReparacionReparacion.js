var idCarro
var idCotizacion

$(document).ready(function () {

   
        $("#cboBuscarCotizacion").select2({
            minimumResultsForSearch: 0, 
            ajax: {
                url: "/Reparacion/ObtenerCotizacion",
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
                                id: item.idCotizacion,
                                text: item.numeroCotizacion,
                                totalDeLaCotizacion: item.totalDeLaCotizacion,
                                fechaCotizacion: item.fechaCotizacion,
                                tblDetalleCotizacions: item.tblDetalleCotizacions
                            }
                        ))
                    };
                },
            },
            language: "es",
            placeholder: 'Buscar Cotizacion...',
            minimumInputLength: 1,
            templateResult: formatoResultado

        }).on("change", function () {
            idCotizacion = $(this).val();
            var cotizacion = $(this).select2("data").find(item => item.id === idCotizacion);

            if (cotizacion) {

                $("#tbdata tbody").empty();
                var subtotal = 0;

                cotizacion.tblDetalleCotizacions.forEach(function (item) {
                    var $fila = $("<tr>");

                    subtotal += parseFloat(item.subtotal);

                    $fila.append(
                        $("<td>").text(item.descripcionProducto),
                        $("<td>").text(item.cantidad),
                        $("<td>").text(item.precio),
                        $("<td>").text(item.subtotal)
                    );

                    $("#tbdata tbody").append($fila);
                });

                
            }
        });


    $("#cboBuscarCarro").select2({
        minimumResultsForSearch: 0,
        ajax: {
            url: "/Reparacion/ObtenerVehiculo",
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
                            id: item.idCarro,
                            text: item.placa,
                            marca: item.marca,
                            modelo: item.modelo,
                            ano: item.ano
                        }
                    ))
                };
            },
        },
        language: "es",
        placeholder: 'Buscar Vehiculo...',
        minimumInputLength: 1,
        templateResult: formatoResultadoVehi

    }).on("change", function (event) {
        idCarro = $(this).val(); 
    });



    function formatoResultadoVehi(data) {
        if (data.loading)
            return data.text;



        var contenedor = $(`
        <table width="100%">
            
            <tr>
                
                <td>
                    <p style="font-weight:bolder;margin:2px;">${data.text}</p>
                    <p style="margin:2px">${data.marca} ${data.modelo}</p>
                 </td>
                
            </tr>
        </table>
        `);

        return contenedor;
    }




        function formatoResultado(data) {
            if (data.loading)
                return data.text;



            var contenedor = $(`
        <table width="100%">
            
            <tr>
                
                <td>
                    <p style="font-weight:bolder;margin:2px;">${data.text}</p>
                    <p style="margin:2px">${data.fechaCotizacion}</p>
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


$("#btnGuardar").click(function () {
   





    var fechaActual = new Date();
    var dia = fechaActual.getDate();
    var mes = fechaActual.getMonth() + 1;
    var anio = fechaActual.getFullYear();

    var fechaFormateada = dia + '/' + mes + '/' + anio;
    console.log(fechaFormateada);

 

    const reparacion = {
        IdCarro: idCarro,
        FechaDeInicio: fechaFormateada,
        DescripcionDeLaReparacion: $("#txtDescripcion").val()
    };


    $("#btnGuardar").LoadingOverlay("show");

        fetch("/Reparacion/RegistrarReparacion", {
            method: "POST",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(reparacion)
        })
            .then(response => {
                $("#btnGuardar").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response)
            })
            .then(responseJson => {

                if (responseJson.estado) {
                    $("#cboBuscarCotizacion").empty()
                    $("#cboBuscarCarro").empty()
                    $("#tbdata tbody").empty();
                    $("#txtDescripcion").val("")

                    swal("Registrado", "La reparacion se registro exitosamente", "success")

                   


                } else {
                    swal("Lo sentimos", `No se pudo registrar la reparacion`, "error")
                }

            })





})

