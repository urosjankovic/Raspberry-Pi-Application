var counter;
var chart;
var chartContext;
var currentPoint;

$(document).ready(function(){
    currentPoint = [{x: 0, y: 0}];

    $(".joysticktable").width($(window).width() / 2);

    chartInit();
})

/**
 * @brief Sets coordinates to (0,0)
 */
function resetGraph(){
    currentPoint = [{x: 0, y: 0}];
}

/**
 * @brief Resets value of counter
 */
function resetCounter(){
    counter = 0;
}

/**
* @brief Start request timer
*/
/*function startTimer(){
	if(timer == null)
		timer = setInterval(ajaxGetJSON, sampleTime);
}

/**
* @brief Stop request timer
*/
/*function stopTimer(){
	if(timer != null) {
		clearInterval(timer);
		timer = null;
	}
}*/

function chartInit(){
    chartContext = $("#chart")[0].getContext('2d');

    Chart.defaults.global.elements.point.radius = 5;

	chart = new Chart(chartContext, {
		// The type of chart: linear plot
		type: 'scatter',

		data: {
			datasets: [
			{
				label: 'Joystick coordinates',
				backgroundColor: 'rgba(255, 255, 255, 1)',
				borderColor: 'rgba(255, 255, 255, 0.75)',
				data: currentPoint
			}
			]
		},

		// Configuration options
		options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                xAxes: [{
                    gridLines: {
                        color: 'rgba(255, 255, 255, 0.5)',
                        zeroLineColor: 'rgba(255, 255, 255, 1)'
                    },
                    ticks:{
                        stepSize: 1,
                        callback: function(value){if (value % 1 === 0){return value;}},
                        suggestedMin: -5,
						suggestedMax: 5
                    }
                }],
                yAxes: [{
                    gridLines: {
                        color: 'rgba(255, 255, 255, 0.5)',
                        zeroLineColor: 'rgba(255, 255, 255, 1)'
                    },
                    ticks:{
                        stepSize: 1,
                        callback: function(value){if (value % 1 === 0){return value;}},
                        suggestedMin: -5,
						suggestedMax: 5 
                    }
                }]
            }
        }
        
    });
    
    currentPoint = chart.data.datasets[0].data;
}