
function renderGraph () {
        Highcharts.setOptions({
            global: {
                useUTC: false
            }
        });

        var chart = new Highcharts.Chart({
            chart: {
                type: 'spline',
                animation: Highcharts.svg, // don't animate in old IE
                marginRight: 10,
                renderTo: 'dynamic-graph',
            },
            title: {
                text: 'Response time graph',   
            },
            xAxis: {
                type: 'datetime',
                tickPixelInterval: 150
            },
            yAxis: {
                title: {
                    text: 'Value(second)'
                },
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }]
            },
            tooltip: {
                formatter: function () {
                    return '<b>' + this.point.url + '</b><br/>' +
                        Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x) + '<br/>' +
                        Highcharts.numberFormat(this.y, 2);
                }
            },
            legend: {
                enabled: false
            },
            exporting: {
                enabled: false
            },
            series: [{
                name: 'response time',
                data: (function () {
                    
                    var data = [],
                        time = (new Date()).getTime(),
                        i;

                    for (i = -19; i <= 0; i += 1) {
                        data.push({
                            x: time + i * 1000,
                            y: 0,
                            url: 'testing url'
                        });
                    }
                    return data;
                }())
            }]
        });
        return chart;
    };



function displayGraphycaly() {
    var compareGraphs = new Highcharts.Chart({
        chart: {
            type: 'line',
            renderTo: 'compare-graphs'
        },
        title: {
            text: 'Compare response time'
        },
        subtitle: {
            text: ''
        },
        xAxis: {
            categories: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20']
        },
        yAxis: {
            title: {
                text: 'Response time(second)'
            }
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: true
                },
                enableMouseTracking: false
            }
        },
    });
    return compareGraphs;
};