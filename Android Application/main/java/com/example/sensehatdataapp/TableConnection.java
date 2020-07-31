package com.example.sensehatdataapp;

import android.content.Context;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.Volley;
import com.example.sensehatdataapp.TableVolleyListener;

import org.json.JSONArray;
/**
 * @brief handles connection to server for data in tables
 */
public class TableConnection {

    /* Server resources */
    private String baseUrl;
    private String protocol = "http://";
    private String environmentSensor = "/sensors_via_deamon.php?id=env";
    private String OriSensor = "/sensors_via_deamon.php?id=ori";

    private TableVolleyListener listener;
    private RequestQueue queue;

    /**
     * @brief ServerIoT parametric constructor.
     * @param url Base URL - server IP or domain.
     * @param context HTTP request execution context. This ensures that the RequestQueue will last
     *                for the lifetime of an app, instead of being recreated every time the
     *                activity is recreated (for example, when the user rotates the device).
     * @param volleyResponseListener Custom response listener interface.
     */
    public TableConnection(String url, Context context, TableVolleyListener volleyResponseListener) {
        baseUrl = url;
        queue = Volley.newRequestQueue(context.getApplicationContext());
        listener = volleyResponseListener;
    }

    /**
     * @brief Server IoT listener getter.
     * @return request listener.
     */
    public TableVolleyListener getListener() {
        return listener;
    }

    /**
     * @brief Get environment sensors measurement: temperature, pressure & humidity.
     */
    public void EnvironmentSensors()  {
        String url = protocol + baseUrl + environmentSensor;

        // Initialize a new JsonArrayRequest instance
        JsonArrayRequest request = new JsonArrayRequest(
                Request.Method.GET, url, null,
                new Response.Listener<JSONArray>() {
                    @Override
                    public void onResponse(JSONArray response) {
                        // Call ViewModel response listener
                        listener.onResponse(response);
                    }
                },
                new Response.ErrorListener(){
                    @Override
                    public void onErrorResponse(VolleyError error){
                        String msg = error.getMessage();
                        // Call ViewModel error listener
                        if(msg != null)
                            listener.onError(msg);
                        else
                            listener.onError("UNKNOWN ERROR");
                    }
                }
        );

        // Add the request to the RequestQueue.
        queue.add(request);
    }

    /**
     * @brief Get orientation sensors measurement: roll, pitch, yaw & x, y, z.
     */
    public void OriSensors()  {
        String url = protocol + baseUrl + OriSensor;

        // Initialize a new JsonArrayRequest instance
        JsonArrayRequest request = new JsonArrayRequest(
                Request.Method.GET, url, null,
                new Response.Listener<JSONArray>() {
                    @Override
                    public void onResponse(JSONArray response) {
                        // Call ViewModel response listener
                        listener.onResponse(response);
                    }
                },
                new Response.ErrorListener(){
                    @Override
                    public void onErrorResponse(VolleyError error){
                        String msg = error.getMessage();
                        // Call ViewModel error listener
                        if(msg != null)
                            listener.onError(msg);
                        else
                            listener.onError("UNKNOWN ERROR");
                    }
                }
        );

        // Add the request to the RequestQueue.
        queue.add(request);
    }
}
