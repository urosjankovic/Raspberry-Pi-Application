var AccelerometerUnit = "deg";
var GyroscopeUnit = "deg"
var sampleTime = 1000;
var maxStoredSamples = 1000;
var timer;

const url = "sensors_via_deamon.php?id=ori";

$(document).ready(function() {

    // Writing initial values to input fields
    $("#sampleTime").val(sampleTime);
    $("#storedSamples").val(maxStoredSamples);

    // Listener for radio input
    $("input:radio").change(function(e){
        var name = e.currentTarget.name; 
        if(name == "Gunit"){
            GyroscopeUnit = e.currentTarget.value;
        }
        else if(name == "Aunit"){
            AccelerometerUnit = e.currentTarget.value;
        }
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

})

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
	$.getJSON(url , function(data){	
        console.log(data)
        updateTable(data);
	})
}

/**
 * @brief Update values in table
 * @param oriObj JS object with data received from server
 */
function updateTable(oriObj){

    if(AccelerometerUnit == "rad"){ 
        oriObj[0].data.roll = DegreesToRadians(oriObj[0].data.roll); 
        oriObj[0].data.pitch = DegreesToRadians(oriObj[0].data.pitch); 
        oriObj[0].data.yaw = DegreesToRadians(oriObj[0].data.yaw); 
    }
        
    if(GyroscopeUnit == "rad"){ 
        oriObj[2].data.roll = DegreesToRadians(oriObj[2].data.roll); 
        oriObj[2].data.pitch = DegreesToRadians(oriObj[2].data.pitch); 
        oriObj[2].data.yaw = DegreesToRadians(oriObj[2].data.yaw);  
    }

    $("#AXValue").html(oriObj[0].data.roll);
    $("#AYValue").html(oriObj[0].data.pitch);
    $("#AZValue").html(oriObj[0].data.yaw);
    $("#MXValue").html(oriObj[1].data.x);
    $("#MYValue").html(oriObj[1].data.y);
    $("#MZValue").html(oriObj[1].data.z);
    $("#GXValue").html(oriObj[2].data.roll);
    $("#GYValue").html(oriObj[2].data.pitch);
    $("#GZValue").html(oriObj[2].data.yaw);
}

function DegreesToRadians(value){
    return value * Math.PI / 180;
}