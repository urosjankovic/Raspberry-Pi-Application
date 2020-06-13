var AccelerometerUnit = "deg";
var GyroscopeUnit = "deg"
var sampleTime = 1000;
var maxStoredSamples = 1000;
var url;

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

    url = getURL();

})

/**
 * @brief Get current URL
 */
function getURL() {
    return window.location.href;
}