$(document).ready(function () {


    $("#cboBuscarProducto").select2({
        ajax: {
            url: "/Venta/ObtenerProductos",
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
                            cantidadEnStock: parseInt(item.cantidadEnStock)
                        }
                    ))
                };
            },
        },
        languaje: "es",
        placeholder: 'Buscar Producto...',
        minimumInputLength: 1,
        templateResult: formatoResultado

    });

    function formatoResultado(data) {
        if (data.loading)
            return data.text

        var contenedor = $(`
        <table width="100%">
            <tr>
                <td>
                <p style="font-weight:bolder;margin:2px">${data.codigoProducto}</p>
                <p style="margin:2px">${data.text}</p>
                </td>
                <td>
                <p style="margin:2px">${data.cantidadEnStock}</p>
                </td>

        `);

        return contenedor;

    }


})