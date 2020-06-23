package com.example.sensehatdataapp;

import android.content.Context;
import android.util.Log;
import android.view.View;


import org.json.JSONArray;
import org.json.JSONException;

import java.util.ArrayList;
import java.util.List;

import androidx.lifecycle.ViewModel;

public class MainViewModel extends ViewModel {
    private MeasurementsAdapter adapter;
    private List<MeasurementViewModel> measurements;
    private TableConnection server;

    public void Init(Context context) {
        measurements = new ArrayList<>();
        adapter = new MeasurementsAdapter(measurements);


        server = new TableConnection(Server_param.globalip, context,
                new TableVolleyListener() {
                    @Override
                    public void onError(String message) {
                        Log.d("Response error", message);
                    }
                    @Override
                    public void onResponse(JSONArray response) throws JSONException {
                        int rs = response.length();
                        int ms = measurements.size();
                        int sizeDiff = ms - rs;
                        // remove redundant measurements from list
                       for( int i = 0; i < sizeDiff; i++) {
                            measurements.remove(ms - 1 - i);
                            adapter.notifyItemRemoved(ms - 1 - i);
                        }
                        // iterate through JSON Array
                        for (int i = 0; i < rs; i++) {
                            /* get measurement model from JSON data */
                            MeasurementModel measurement = new MeasurementModel(response.getJSONObject(i));
                            //MeasurementModel measurement = new MeasurementModel("humid",50.0,"%");
                            measurements.add(measurement.toVM());
                            adapter.notifyItemInserted(i);

                            /* update measurements list */
                            if(i >= ms) {

                            } else {
                                measurements.set(i, measurement.toVM());
                                adapter.notifyItemChanged(i);
                            }
                        }

                        adapter = new MeasurementsAdapter(measurements);
                    }
                }
        );
    }

    /**
     * @brief Getter of 'adapter' field.
     * @return Measurement list view adapter.
     */
    public MeasurementsAdapter getAdapter() {
        return adapter;
    }

    /**
     * @brief 'Refresh' button onClick event handler.
     * @param v 'Refresh' button view
     */
    public void updateMeasurements(View v) {
        server.EnvironmentSensors();
    }
}



