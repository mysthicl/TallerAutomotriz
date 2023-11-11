$(document).ready(function () {

    $("div.container-fluid").LoadingOverlay("show");




    fetch(`/Home/ObtenerResumen`)
        .then(response => {
            $("div.container-fluid").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            if (responseJson.estado) {
                let d = responseJson.objeto

                $("#totalVenta").text(d.totalVentas)
                $("#totalIngresos").text(parseFloat(d.totalIngresos).toFixed(2))
                $("#totalServicios").text(d.totalServicios)
                $("#totalProductos").text(d.totalProductos)

                let barchart_label
                let barchart_data
                console.log(d.ventasUltimaSemana)

                if (d.ventasUltimaSemana.length > 0) {
                    barchart_label = d.ventasUltimaSemana.map((item) => { return item.fecha })
                    barchart_data = d.ventasUltimaSemana.map((item) => { return item.total })

                } else {
                    barchart_label = ["sin resultados"]
                    barchart_data=[0]
                }

                let piechar_label
                let piechar_data

                if (d.productosTopUltimaSemana.length > 0) {
                    piechar_label = d.productosTopUltimaSemana.map((item) => { return item.producto })
                    piechar_data = d.productosTopUltimaSemana.map((item) => { return item.cantidad })

                } else {
                    piechar_label = ["sin resultados"]
                    piechar_data = [0]
                }

                let controlVenta = document.getElementById("chartVentas");
                let myBarChart = new Chart(controlVenta, {
                    type: 'bar',
                    data: {
                        labels: barchart_label,
                        datasets: [{
                            label: "Cantidad",
                            backgroundColor: "#4e73df",
                            hoverBackgroundColor: "#2e59d9",
                            borderColor: "#4e73df",
                            data: barchart_data,
                        }],
                    },
                    options: {
                        maintainAspectRatio: false,
                        legend: {
                            display: false
                        },
                        scales: {
                            xAxes: [{
                                gridLines: {
                                    display: false,
                                    drawBorder: false
                                },
                                maxBarThickness: 50,
                            }],
                            yAxes: [{
                                ticks: {
                                    min: 0,
                                    maxTicksLimit: 5
                                }
                            }],
                        },
                    }
                });

                let controlProducto = document.getElementById("charProductos");
                let myPieChart = new Chart(controlProducto, {
                    type: 'doughnut',
                    data: {
                        labels: piechar_label,
                        datasets: [{
                            data: piechar_data,
                            backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc', "#FF785B"],
                            hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf', "#FF5733"],
                            hoverBorderColor: "rgba(234, 236, 244, 1)",
                        }],
                    },
                    options: {
                        maintainAspectRatio: false,
                        tooltips: {
                            backgroundColor: "rgb(255,255,255)",
                            bodyFontColor: "#858796",
                            borderColor: '#dddfeb',
                            borderWidth: 1,
                            xPadding: 15,
                            yPadding: 15,
                            displayColors: false,
                            caretPadding: 10,
                        },
                        legend: {
                            display: true
                        },
                        cutoutPercentage: 80,
                    },
                });



                
            }
            
        })


   
})