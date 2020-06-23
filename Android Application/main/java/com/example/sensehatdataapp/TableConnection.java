package com.example.sensehatdataapp;



import android.content.Context;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.Volley;

import org.json.JSONArray;
import org.json.JSONException;

public class TableConnection {

    /* Server resources */
    private String baseUrl;
    private String protocol = "http://";
    private String environmentSensor = "sensors_via_deamon.php?id=env";

    private TableVolleyListener listener;
    private RequestQueue queue;

    /**
     * @brief TableConnection parametric constructor.
     * @param url Base URL - server IP or domain.
     * @param context HTTP request execution context. This ensures that the RequestQueue will last
     *                for the lifetime of an app, instead of being recreated every time the
     *                activity is recreated (for example, when the user rotates the device).
     * @param volleyResponseListener Custom response listener interface.
     */
    public TableConnection(String url, Context context, TableVolleyListener volleyResponseListener) {
        baseUrl = url;
        queue = Volley.newRequestQueue(context);
        listener = volleyResponseListener;
    }

    /**
     * @brief  listener getter.
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
                        try {
                            listener.onResponse(response);
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
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

