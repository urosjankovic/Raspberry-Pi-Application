package com.example.sensehatdataapp;



import org.json.JSONException;
import org.json.JSONObject;
/**
 * @brief handles creation of measurement models from response
 */
public class MeasurementModel {

    public String mName;
    public Double mValue;
    public Double yaw;
    public Double x;
    public Double y;
    public Double z;
    public Double roll;
    public Double pitch;
    public JSONObject angles;
    public String mUnit;



    public MeasurementModel(JSONObject data) throws JSONException {
        try {
            mName = data.getString("name");
            mValue= (Double) data.getDouble("data");
            mUnit = data.getString("unit");
        }
        catch (JSONException e) {
            e.printStackTrace();
            throw new JSONException("Json Object to Measurement Data parse error");
        }


    }

    public MeasurementModel(JSONObject data,int q) throws JSONException {
        try {
            mName = data.getString("name");
            angles= (JSONObject) data.get("data");
            if(!mName.equals("magnetic")) {
                yaw = (Double) angles.get("yaw");
                roll = (Double) angles.get("roll");
                pitch = (Double) angles.get("pitch"); 
            }

            if(mName.equals("magnetic")){
                x = (Double) angles.get("x");
               y = (Double) angles.get("y");
               z = (Double) angles.get("z");

            }
            mUnit = data.getString("unit");
        }
        catch (JSONException e) {
            e.printStackTrace();
            throw new JSONException("Json Object to Measurement Data parse error");
        }
    }



    public MeasurementViewModel toVM()
    {
        return new MeasurementViewModel(this);
    }
}
