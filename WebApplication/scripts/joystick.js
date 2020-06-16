var counter;
var chart;
var chartContext;

var timer;

const getDataURL = '../server/joystick.php?id=get'
const resetCounterURL = '../server/joystick.php?id=rst'

$(document).ready(function(){
    currentPoint = [{x: 0, y: 0}];

    $(".joysticktable").width($(window).width() / 2);

    chartInit();
    resetAll();
})

/**
 * @brief Sets coordinates to (0,0) and resets counter
 */
function resetAll(){
    ajaxGetJSON(resetCounterURL);
}

/**
* @brief Start request timer
*/
function startTimer(){
    if(timer == null)
    	timer = setInterval(ajaxGetJSON, 100, getDataURL);
}

/**
* @brief Stop request timer
*/
function stopTimer(){
	if(timer != null) {
		clearInterval(timer);
		timer = null;
	}
}

/**
* @brief Send HTTP GET request to IoT server
*/
function ajaxGetJSON(url){
	$.getJSON(url, function(data){	
		handleData(data);
	})
}

function handleData(dataX){    
    if(dataX){
        counter = dataX.Counter;
        chart.data.datasets[0].data[0].x = dataX.X;
        chart.data.datasets[0].data[0].y = dataX.Y;
        $("#counterValue").html(counter);
        chart.update();
    }
}

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
				data: [{x: 0, y: 0}]
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
}