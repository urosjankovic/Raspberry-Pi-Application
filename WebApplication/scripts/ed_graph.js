var sampleTime = 1000;
var sampleTimeSec = sampleTime / 1000;
var maxStoredSamples = 100;

var timeVec;
var tempVec;
var humVec;
var presVec;
var lastTimeStamp;

var tempChartContext;
var presChartContext;
var humChartContext;
var tempChart;
var presChart;
var humChart;

var timer;

const url = "sensors_via_deamon.php?id=env";

$(document).ready(function() {

    timer = null;

    // Write initial values to input fields
    $("#sampleTime").val(sampleTime);
    $("#storedSamples").val(maxStoredSamples);

    // Sample time change listener
    $("#sampleTime").change(function(){
        var inputValue = Number($(this).val());
        if (inputValue != NaN){
            sampleTime = inputValue;
        }
		sampleTimeSec = sampleTime / 1000;
		stopTimer();
		startTimer();
    });

    // Max stored samples change listener
    $("#storedSamples").change(function(){
        var inputValue = Number($(this).val());
        if (inputValue != NaN){
            maxStoredSamples = inputValue;
		}
    });

    $("#graphs").css("margin-top", $("#menu").height() + 8);

    tempChartInit();

})

/**
 * @brief Add new value to next data point
 * @param data object with received data
 */
function addData(data){
    if(tempVec.length > maxStoredSamples){
        removeOldData();
        lastTimeStamp += sampleTimeSec;
        timeVec.push(lastTimeStamp.toFixed(4))
    }

    tempVec.push(data[0].data);
    tempChart.update();
    presVec.push(data[2].data);
    presChart.update();
    humVec.push(data[3].data);
    humChart.update();
}

/**
 * Remove oldest data point
 */
function removeOldData(){
    timeVec.splice(0,1);
    tempVec.splice(0,1);
    presVec.splice(0,1);
    humVec.splice(0,1);
}

/**
* @brief Start request timer
*/
function startTimer(){
	if(timer == null)
		timer = setInterval(ajaxGetJSON, sampleTime);
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
function ajaxGetJSON(){
	$.getJSON(url, function(data){	
		addData(data);
	})
}

function tempChartInit(){
    	// array with consecutive integers: <0, maxSamplesNumber-1>
	timeVec = [...Array(maxSamplesNumber).keys()]; 
	// scaling all values ​​times the sample time 
	timeVec.forEach(function(p, i) {this[i] = (this[i]*sampleTimeSec).toFixed(4);}, timeVec);

	// last value of 'timeVec'
	lastTimeStamp = +timeVec[timeVec.length-1]; 

	// empty array
	tempVec = []; 

	// get chart context from 'canvas' element
	chartContext = $("#tempChart").getContext('2d');

	Chart.defaults.global.elements.point.radius = 1;
	
	chart = new Chart(chartContext, {
		// The type of chart: linear plot
		type: 'line',

		// Dataset: 'timeVec' as labels, 'rollVec' as dataset.data
		data: {
			labels: timeVec,
			datasets: [
			{
				fill: false,
				label: 'Temperature',
				backgroundColor: 'rgba(255, 0, 0, 0.5)',
				borderColor: 'rgba(255, 0, 0, 0.5)',
				data: tempVec,
				lineTension: 0
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
                        color: 'rgba(255, 255, 255, 0.5)'
                    }
                }],
				yAxes: [{
					scaleLabel: {
						display: true,
						labelString: 'Temperature [&degC]'
					},
					ticks: {
						suggestedMin: -30,
						suggestedMax: 105 
                    },
                    gridLines: {
                        color: 'rgba(255, 255, 255, 0.5)'
                    }
				}],es: [{
					scaleLabel: {
						display: true,
						labelString: 'Time [s]'
					}
				}]
			}
		}
	});
	

	
	//$.ajaxSetup({ cache: false });
}