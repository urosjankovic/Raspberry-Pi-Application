package com.example.sensehatdataapp;


/**
 * @brief allows to access elements of measurement models
 */
public class MeasurementViewModel {

    private MeasurementModel model;
    private static final String valueFormat = "%.4f";
    String value="n";

    public MeasurementViewModel(MeasurementModel measurement) {
        model = measurement;
    }

    public String getName() {
        return model.mName;
    }

    public String getValue() {
        //return String.format(valueFormat, model.mValue);
        return model.mValue.toString();

    }

    public String getUnit() {
        return model.mUnit;
    }

    public String getRoll() {
        if(model.roll==null) {
            value= "roll: -";
        }else{value= "roll: "+model.roll.toString();}

        //return String.format(valueFormat, model.mValue);
        return value;
    }

    public String getYaw() {
        //return String.format(valueFormat, model.mValue);
        if(model.yaw==null) {
            value= "yaw: -";
        }else{value="yaw: "+model.yaw.toString();}
        return value;
    }

    public String getPitch() {
        //return String.format(valueFormat, model.mValue);
        if(model.pitch==null) {
            value= "pitch: -";
        }else{value="pitch: "+model.pitch.toString();}
        return value;
    }

    public String getX() {
        //return String.format(valueFormat, model.mValue);

        if(model.x==null) {
            value= "x: -";
        }else{value= "x: " + model.x.toString();}
        return value;
    }

    public String getY() {
        //return String.format(valueFormat, model.mValue);

        if(model.y==null) {
            value= "y: -";
        }else{value= "y: " + model.y.toString();}

        return value;
    }

    public String getZ() {
        //return String.format(valueFormat, model.mValue);

        if(model.z==null) {
            value= "z: -";
        }else{value= "z: " + model.z.toString();}
        return value;
    }
}

