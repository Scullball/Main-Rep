function renderColumn () {
    // Set up the chart
    var column = new Highcharts.Chart({
        chart: {
            renderTo: 'dynamic-column',
            type: 'column',
            options3d: {
                enabled: true,
                alpha: 15,
                beta: 15,
                depth: 50,
                viewDistance: 25
            }
        },
        title: {
            text: 'Page response time'
        },
        yAxis: {
            allowDecimals: false,
            min: 0,
            title: {
                text: 'Page response time'
            }
        },
        subtitle: {
            text: 'Show minimum and maximun values'
        },
        plotOptions: {
            column: {
                stacking: 'normal',
                depth: 40
            }
        },
        series: [{
            name: 'max',
            data: [],

        }, {
            name: 'min',
            data: []

        }]
    });

    return column;
};