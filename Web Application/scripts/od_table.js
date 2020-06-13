var unit = "deg";
var sampleTime = 1000;
var maxStoredSamples = 1000;


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

})