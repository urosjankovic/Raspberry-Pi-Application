var unit = "deg";
var sampleTime = 1000;
var maxStoredSamples = 1000;

var timeVec;
const Accelerometer = 0;
const Magnetic = 1;
const Gyroscope = 2;
var chartIds = ["#Achart", "#Mchart", "Gchart"];
var AxVec = [], AyVec = [], AzVec = [];
var MxVec = [], MyVec = [], MzVec = [];
var GxVec = [], GyVec = [], GzVec = [];
var dataVec = [[AxVec, AyVec, AzVec], [MxVec, MyVec, MzVec], [GxVec, GyVec, GzVec]];
var lastTimeStamp;

var chartContexts = [];
var charts = [];

var timer;

const url = '';

function addData(data){
    if(AxVec.length > maxStoredSamples)
    {
        removeOldData();
        lastTimeStamp += sampleTime / 1000;
        timeVec.push(lastTimeStamp.toFixed(4));
    }
    
    for(var i = 0; i < dataVec.length; i++){
        var coordinates = dataVec[i]; 
        for(var j = 0; j < coordinates.length; j++){
            dataVec[j].push(data[i][j]);
        }
        charts[i].update();
    }   
}

/**
* @brief Remove oldest data point.
*/
function removeOldData(){
    timeVec.splice(0,1);
    for(var i = 0; i < dataVec.length; i++){
        var coordinates = dataVec[i]; 
        for(var j = 0; j < coordinates.length; j++){
            dataVec[j].splice(0,1);
        }
    }
}

/**
* @brief Start request timer
*/
function startTimer(){
	if(timer == null)
		timer = setInterval(ajaxJSON, sampleTimeMsec);
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
function ajaxJSON() {
	$.ajax(url, {
		type: 'GET', dataType: 'json',
		success: function(responseJSON, status, xhr) {
            addData([+responseJSON[0].value, +responseJSON[1].value, +responseJSON[2].value],
                    [+responseJSON[3].value, +responseJSON[4].value, +responseJSON[5].value]
                    [+responseJSON[6].value, +responseJSON[7].value, +responseJSON[8].value]);
		}
	});
}

$(document).ready(function() {

    // Writing initial values to input fields
    $("#sampleTime").val(sampleTime);
    $("#storedSamples").val(maxStoredSamples);

    // Listener for radio input
    $("input:radio").change(function(e){
        unit = e.currentTarget.value;
    });

    $("#sampleTime").change(function(){
        var inputValue = Number($(this).val());
        if (inputValue != NaN){
            sampleTime = inputValue;
        }
    });

    $("#storedSamples").change(function(){
        var inputValue = Number($(this).val());
        if (inputValue != NaN){
            maxStoredSamples = inputValue;
        }
    });

    getURL();

    chartInit(1);
    chartInit(2);
    chartInit(3);

})

function getURL() {
    url = window.location.href;
}

function chartInit(id)
{
	// array with consecutive integers: <0, maxSamplesNumber-1>
	timeVec = [...Array(maxSamplesNumber).keys()]; 
	// scaling all values ​​times the sample time 
	timeVec.forEach(function(p, i) {this[i] = (this[i]*sampleTimeSec).toFixed(4);}, timeVec);

	// last value of 'timeVec'
	lastTimeStamp = +timeVec[timeVec.length-1]; 

	// get chart contexts from 'canvas' elements
    chartContexts.push($(chartIds[id])[0].getContext('2d'));
    
	Chart.defaults.global.elements.point.radius = 1;
	
	charts.push(new Chart(chartContexts[id], {
		// The type of chart: linear plot
		type: 'line',

		// Dataset: 'timeVec' as labels, 'rollVec' as dataset.data
		data: {
			labels: timeVec,
			datasets: [
			{
				fill: false,
				label: 'X',
				backgroundColor: 'rgb(255, 0, 0)',
				borderColor: 'rgb(255, 0, 0)',
				data: dataVec[id][0],
				lineTension: 0
			},
			{
				fill: false,
				label: 'Y',
				backgroundColor: 'rgb(0, 255, 0)',
				borderColor: 'rgb(0, 255, 0)',
				data: dataVec[id][1],
				lineTension: 0
			},
						{
				fill: false,
				label: 'Z',
				backgroundColor: 'rgb(0, 0, 255)',
				borderColor: 'rgb(0, 0, 255)',
				data: dataVec[id][2],
				lineTension: 0
			}
			]
		},

		// Configuration options
		options: {
			responsive: true,
			maintainAspectRatio: false,
			animation: false,
			scales: {
				yAxes: [{
					scaleLabel: {
						display: true,
						labelString: 'Angular position [-]'
					},
					ticks: {
						suggestedMin: 0,
						suggestedMax: 360 
					}
				}],es: [{
					scaleLabel: {
						display: true,
						labelString: 'Time [s]'
					}
				}]
			}
		}
	}));
    
    for(var i = 0; i < 3; i++){
        dataVec[id][i] = chart.data.datasets[i].data;
    }
	timeVec = chart.data.labels;
	
	//$.ajaxSetup({ cache: false });
}