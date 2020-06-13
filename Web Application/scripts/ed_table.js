var tempFromHumUnit = "C";
var tempFromPresUnit = "C";
var presUnit = "hPa";
var humUnit = "P";
var sampleTime = 1000;
var maxStoredSamples = 1000;
var url;

$(document).ready(function() {

    // Writing initial values to input fields
    $("#sampleTime").val(sampleTime);
    $("#storedSamples").val(maxStoredSamples);

    // Listener for radio inputs
    $("input:radio").change(function(e){
        var name = e.currentTarget.name; 
        if(name == "tempFromHumUnit"){
            tempFromHumUnit = e.currentTarget.value;
        }
        else if(name == "tempFromPressUnitRadio"){
            tempFromPresUnit = e.currentTarget.value;
        }
        else if(name == "presUnit"){
            presUnit = e.currentTarget.value;
        }
        else if(name == "humUnit"){
            humUnit = e.currentTarget.value;
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

/**
 * @brief Convert Celsius degrees to Fahrenheit degrees
 */
function CelsiusToFahrenheit(val){
    return 1.8 * val + 32;
}

/**
 * @brief Convert Celsius degrees to Kelvins
 */
function CelsiusToKelvin(val){
    return val + 273.15;
}

/**
 * @brief Convert hPa degrees to mmHg
 */
function hPaTommHg(val){
    return val * 0.7500616827;
}

/**
 * @brief Convert hPa degrees to Bars
 */
function hPaToBar(val){
    return val / 1000;
}

/**
 * @brief Convert percents degrees to decimal fraction
 */
function PercentToDecimal(val){
    return val / 100;
}