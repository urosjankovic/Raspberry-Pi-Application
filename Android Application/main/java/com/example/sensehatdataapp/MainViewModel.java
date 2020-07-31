package com.example.sensehatdataapp;

import android.content.Context;
import android.util.Log;
import android.view.View;

import com.example.sensehatdataapp.MeasurementModel;
import com.example.sensehatdataapp.TableConnection;
import com.example.sensehatdataapp.TableVolleyListener;

import org.json.JSONArray;
import org.json.JSONException;

import java.util.ArrayList;
import java.util.List;

import androidx.lifecycle.ViewModel;

public class MainViewModel extends ViewModel {

    private MeasurementsAdapter adapter;
    private MeasurementsAdapter_Ori adapter_ori;
    private List<MeasurementViewModel> measurements;
    private TableConnection server;

    /**
     * @brief MainViewModel initialization: creation of measurement list view adapter, measurements
     *        list container and IoT server API configuration for environmental data.
     * @param context Activity context
     */
    public void Init(Context context) {
        measurements = new ArrayList<>();
        adapter = new MeasurementsAdapter(measurements);

        server = new TableConnection(Server_param.defaultipAddress, context.getApplicationContext(),
                new TableVolleyListener() {
                    @Override
                    public void onError(String message) {
                        Log.d("Response error", message);
                    }
                    @Override
                    public void onResponse(JSONArray response) {
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
                            try {
                                /* get measurement model from JSON data */
                                MeasurementModel measurement = new MeasurementModel(response.getJSONObject(i));

                                /* update measurements list */
                                if(i >= ms) {
                                    measurements.add(measurement.toVM());
                                    adapter.notifyItemInserted(i);
                                } else {
                                    measurements.set(i, measurement.toVM());
                                    adapter.notifyItemChanged(i);
                                }
                            } catch (JSONException e) {
                                e.printStackTrace();
                            }
                        }
                    }
                }
        );
    }

    /**
     * @brief MainViewModel initialization: creation of measurement list view adapter, measurements
     *        list container and IoT server API configuration for orientation data.
     * @param context Activity context
     */

    public void Init2(Context context) {
        measurements = new ArrayList<>();
        adapter_ori = new MeasurementsAdapter_Ori(measurements);

        server = new TableConnection(Server_param.defaultipAddress, context.getApplicationContext(),
                new TableVolleyListener() {
                    @Override
                    public void onError(String message) {
                        Log.d("Response error", message);
                    }
                    @Override
                    public void onResponse(JSONArray response) {
                        int rs = response.length();
                        int ms = measurements.size();
                        int sizeDiff = ms - rs;
                        // remove redundant measurements from list
                        for( int i = 0; i < sizeDiff; i++) {
                            measurements.remove(ms - 1 - i);
                            adapter_ori.notifyItemRemoved(ms - 1 - i);
                        }
                        // iterate through JSON Array
                        for (int i = 0; i < rs; i++) {
                            try {
                                /* get measurement model from JSON data */
                                MeasurementModel measurement = new MeasurementModel(response.getJSONObject(i),0);

                                /* update measurements list */
                                if(i >= ms) {
                                    measurements.add(measurement.toVM());
                                    adapter_ori.notifyItemInserted(i);
                                } else {
                                    measurements.set(i, measurement.toVM());
                                    adapter_ori.notifyItemChanged(i);
                                }
                            } catch (JSONException e) {
                                e.printStackTrace();
                            }
                        }
                    }
                }
        );
    }


    /**
     * @brief Getter of 'adapter' field for environmental data.
     * @return Environmental data Measurement list view adapter.
     */
    public MeasurementsAdapter getAdapterA() {
        return adapter;
    }

    public MeasurementsAdapter_Ori getAdapterB() {
        return adapter_ori;
    }

    /**
     * @brief 'Refresh' button onClick event handler for environmental data.
     * @param v 'Refresh' button view in environmental data window
     */
    public void updateMeasurements(View v) {
        server.EnvironmentSensors();
    }

    /**
     * @brief 'Refresh' button onClick event handler for orientation data.
     * @param v 'Refresh' button view in orientation data window
     */
    public void updateMeasurementsOri(View v) {
        server.OriSensors();
    }
}
