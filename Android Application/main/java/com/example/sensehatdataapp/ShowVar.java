package com.example.sensehatdataapp;

import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.os.Handler;
import android.os.SystemClock;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.util.Log;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.jjoe64.graphview.GraphView;
import com.jjoe64.graphview.LegendRenderer;
import com.jjoe64.graphview.series.DataPoint;
import com.jjoe64.graphview.series.LineGraphSeries;


import java.util.Timer;
import java.util.TimerTask;

import static java.lang.Double.isNaN;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;


public class ShowVar extends AppCompatActivity {



    private final double dataGraphMaxX = 10.0d;
    private final double dataGraphMinX =  0.0d;

    private final double dataGraphMaxYP =  1300.0d;
    private final double dataGraphMinYP = 0.0d;


    private final double dataGraphMaxYT =  120.0d;
    private final double dataGraphMinYT = -40.0d;


    private final double dataGraphMaxYH =  100.0d;
    private final double dataGraphMinYH = 0.0d;

    private GraphView dataGraph;
    private GraphView dataGraph2;
    private GraphView dataGraph3;
    private LineGraphSeries<DataPoint> dataSeriesA;
    private LineGraphSeries<DataPoint> dataSeriesB;
    private LineGraphSeries<DataPoint> dataSeriesC;

    private RequestQueue queue;
    private Timer rqTimerA;
    private Timer rqTimerB;
    private Timer rqTimerC;
    private TimerTask rqTimertaskA;
    private TimerTask rqTimertaskB;
    private TimerTask rqTimertaskC;
    private final Handler handler=new Handler();
    private long rqTimerTimeStamp=0;
    private long requestTimerPreviousTime=-1;
    private boolean rqTimerFirstRequest = true;
    private boolean rqTimerFirstRequestAfterStop;


    public static String JSON_DATA="sensors_via_deamon.php?id=env";



    TextView tv1, tv2, tv3;

    String ipAddress,url;
    int sampleTime, samplesvalue, dataGraphMaxDataPointsNumber;



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_showvar);


        ipAddress =Server_param.globalip;
        sampleTime = Server_param.globalsampletime;
        samplesvalue = Server_param.globalsamples;

        url = getURL(ipAddress);

        dataGraphMaxDataPointsNumber = samplesvalue;

        /* BEGIN initialize GraphView */
        // https://github.com/jjoe64/GraphView/wiki
        dataGraph = (GraphView)findViewById(R.id.dataGraph);
        dataGraph2 = (GraphView)findViewById(R.id.dataGraph2);
        dataGraph3 = (GraphView)findViewById(R.id.dataGraph3);
        dataSeriesA = new LineGraphSeries<>(new DataPoint[]{});
        dataSeriesB = new LineGraphSeries<>(new DataPoint[]{});
        dataSeriesC = new LineGraphSeries<>(new DataPoint[]{});

        dataSeriesA.setTitle("Humidity");
        dataSeriesB.setTitle("Temperature");
        dataSeriesC.setTitle("Pressure");

        dataGraph2.addSeries(dataSeriesB);
        dataGraph.addSeries(dataSeriesA);
        dataGraph3.addSeries(dataSeriesC);

        dataGraph.getViewport().setXAxisBoundsManual(true);
        dataGraph.getViewport().setMinX(dataGraphMinX);
        dataGraph.getViewport().setMaxX(dataGraphMaxX);
        dataGraph.getViewport().setYAxisBoundsManual(true);
        dataGraph.getViewport().setMinY(dataGraphMinYH);
        dataGraph.getViewport().setMaxY(dataGraphMaxYH);
        dataGraph.getLegendRenderer().setVisible(true);
        dataGraph.getLegendRenderer().setAlign(LegendRenderer.LegendAlign.TOP);
        dataGraph.getLegendRenderer().setFixedPosition(650,-3);
        dataGraph.getGridLabelRenderer().setVerticalAxisTitle("[%]");

        dataGraph2.getViewport().setXAxisBoundsManual(true);
        dataGraph2.getViewport().setMinX(dataGraphMinX);
        dataGraph2.getViewport().setMaxX(dataGraphMaxX);
        dataGraph2.getViewport().setYAxisBoundsManual(true);
        dataGraph2.getViewport().setMinY(dataGraphMinYT);
        dataGraph2.getViewport().setMaxY(dataGraphMaxYT);
        dataGraph2.getLegendRenderer().setVisible(true);
        //dataGraph2.getLegendRenderer().setAlign(LegendRenderer.LegendAlign.TOP);
        dataGraph2.getLegendRenderer().setFixedPosition(650,82);
        dataGraph2.getGridLabelRenderer().setVerticalAxisTitle("Celcius");


        dataGraph3.getViewport().setXAxisBoundsManual(true);
        dataGraph3.getViewport().setMinX(dataGraphMinX);
        dataGraph3.getViewport().setMaxX(dataGraphMaxX);
        dataGraph3.getViewport().setYAxisBoundsManual(true);
        dataGraph3.getViewport().setMinY(dataGraphMinYP);
        dataGraph3.getViewport().setMaxY(dataGraphMaxYP);
        dataGraph3.getLegendRenderer().setVisible(true);
        dataGraph3.getLegendRenderer().setAlign(LegendRenderer.LegendAlign.TOP);
        dataGraph3.getLegendRenderer().setFixedPosition(650,165);
        dataGraph3.getGridLabelRenderer().setHorizontalAxisTitle("Time (ms)");
        dataGraph3.getGridLabelRenderer().setVerticalAxisTitle("Milibars");



        /* END initialize GraphView */


        // Initialize Volley request queue
        queue = Volley.newRequestQueue(ShowVar.this);



        tv1= (TextView) findViewById(R.id.textView9);
        tv2= (TextView) findViewById(R.id.textView10);
        tv3= (TextView) findViewById(R.id.textView11);



        tv1.setText("IP address: "+ipAddress);
        tv2.setText("Sample time: "+Integer.toString(sampleTime)+"ms");
        tv3.setText("No. of Samples: "+Integer.toString(samplesvalue));



    }


    private void startRequestTimer() {

        Bundle extras=getIntent().getExtras();
        int val1=extras.getInt("val1");
        int val2=extras.getInt("val2");
        int val3=extras.getInt("val3");

        if(val1==1) {
            if (rqTimerA == null) {
                // set a new Timer
                rqTimerA = new Timer();

                // initialize the TimerTask's job
                initializeRequestTimerTaskA();

                rqTimerA.schedule(rqTimertaskA, 0, sampleTime);


                // clear error message
                //textViewError.setText("");
            }
        }else{
            dataGraph.getGridLabelRenderer().setHorizontalLabelsVisible(false);
        }

        if(val2==1) {
            if (rqTimerB == null) {
                // set a new Timer
                rqTimerB = new Timer();

                // initialize the TimerTask's job

                initializeRequestTimerTaskB();

                rqTimerB.schedule(rqTimertaskB, 0, sampleTime);


                // clear error message
                //textViewError.setText("");
            }
        }




        if(val3==1) {
            if (rqTimerC == null) {
                // set a new Timer
                rqTimerC = new Timer();

                // initialize the TimerTask's job

                initializeRequestTimerTaskC();

                rqTimerC.schedule(rqTimertaskC, 0, sampleTime);


                // clear error message
                //textViewError.setText("");
            }
        }
    }

    private void stopRequestTimer() {
        // stop the timer, if it's not already null
        if (rqTimerA != null) {
            rqTimerA.cancel();
            rqTimerA = null;
            rqTimerFirstRequestAfterStop = true;
        }
        if (rqTimerB != null) {
            rqTimerB.cancel();
            rqTimerA = null;
            rqTimerFirstRequestAfterStop = true;
        }
        if (rqTimerC != null) {
            rqTimerC.cancel();
            rqTimerC = null;
            rqTimerFirstRequestAfterStop = true;
        }
    }

    private void initializeRequestTimerTaskA() {
        rqTimertaskA = new TimerTask() {
            public void run() {
                handler.post(new Runnable() {
                    public void run() { sendGetRequestA(); }
                });
            }
        };
    }


    private void initializeRequestTimerTaskB() {
        rqTimertaskB = new TimerTask() {
            public void run() {
                handler.post(new Runnable() {
                    public void run() { sendGetRequestB(); }
                });
            }
        };
    }

    private void initializeRequestTimerTaskC() {
        rqTimertaskC = new TimerTask() {
            public void run() {
                handler.post(new Runnable() {
                    public void run() { sendGetRequestC(); }
                });
            }
        };
    }


    public void startclick(View v){
        //startRQTimer();

        Bundle extras=getIntent().getExtras();
        int val1=extras.getInt("val1");
        int val2=extras.getInt("val2");
        int val3=extras.getInt("val3");

        if(val1==1 || val2==1 || val3==1) {
            startRequestTimer();
        }


            if (val1 != 1 && val2 != 1 && val3 != 1) {
                Toast.makeText(ShowVar.this, "No variables selected. \nSelect variables on variables page to view.", Toast.LENGTH_LONG).show();
            }
        }


    public void stopclick(View v){

        //stopRQTimer();
        stopRequestTimer();
    }



    private String getURL(String ip) {
        return ("http://" + ip + "/" + JSON_DATA);
    }


    /**
     * @brief Initialize request timer period task with 'Handler' post method as 'sendGetRequest'.
     */

    private double getRawDataFromResponseA(String response) {
        JSONArray jarray;

        double x = Double.NaN;

        // Create generic JSON object form string
        try {
            jarray = new JSONArray(response);

        } catch (JSONException e) {
            e.printStackTrace();
            return x;
        }

        // Read chart data form JSON object
        try {
            x = (Double) jarray.getJSONObject(3).get("data");

        } catch (JSONException e) {
            e.printStackTrace();
        }
        return x;
    }


    private double getRawDataFromResponseB(String response) {
        JSONArray jarray;

        double x = Double.NaN;

        // Create generic JSON object form string
        try {
            jarray = new JSONArray(response);

        } catch (JSONException e) {
            e.printStackTrace();
            return x;
        }

        // Read chart data form JSON object
        try {
            x = (Double) jarray.getJSONObject(0).get("data");

        } catch (JSONException e) {
            e.printStackTrace();
        }
        return x;
    }

    private double getRawDataFromResponseC(String response) {
        JSONArray jarray;

        double x = Double.NaN;

        // Create generic JSON object form string
        try {
            jarray = new JSONArray(response);

        } catch (JSONException e) {
            e.printStackTrace();
            return x;
        }

        // Read chart data form JSON object
        try {
            x = (Double) jarray.getJSONObject(2).get("data");

        } catch (JSONException e) {
            e.printStackTrace();
        }
        return x;
    }

    public void sendGetRequestA(){


            // Request a string response from the provided URL
            StringRequest stringRequest = new StringRequest(Request.Method.GET, url,
                    new Response.Listener<String>() {
                        @Override
                        public void onResponse(String response) {
                            showGraphA(response);
                        }
                    }, null);
               /* new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error) { errorHandling(COMMON.ERROR_RESPONSE); }
                }*/

            // Add the request to the RequestQueue.
            queue.add(stringRequest);
        }

    public void sendGetRequestB(){

            // Request a string response from the provided URL
            StringRequest stringRequest = new StringRequest(Request.Method.GET, url,
                    new Response.Listener<String>() {
                        @Override
                        public void onResponse(String response) {
                            showGraphB(response);
                        }
                    }, null);
               /* new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error) { errorHandling(COMMON.ERROR_RESPONSE); }
                }*/

            // Add the request to the RequestQueue.
            queue.add(stringRequest);
        }

    public void sendGetRequestC(){

            // Request a string response from the provided URL
            StringRequest stringRequest = new StringRequest(Request.Method.GET, url,
                    new Response.Listener<String>() {
                        @Override
                        public void onResponse(String response) {
                            showGraphC(response);
                        }
                    }, null);
               /* new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error) { errorHandling(COMMON.ERROR_RESPONSE); }
                }*/

            // Add the request to the RequestQueue.
            queue.add(stringRequest);
        }



    public long timingIncrease(long currentTime){
        if(rqTimerFirstRequest)
        {
            requestTimerPreviousTime = currentTime;
            rqTimerFirstRequest = false;
            return 0;
        }

        // After each stop return value not greater than sample time
        // to avoid "holes" in the plot
        if(rqTimerFirstRequestAfterStop)
        {
            if((currentTime - requestTimerPreviousTime) > sampleTime)
                requestTimerPreviousTime = currentTime - sampleTime;

            rqTimerFirstRequestAfterStop = false;
        }

        // If time difference is equal zero after start
        // return sample time
        if((currentTime - requestTimerPreviousTime) == 0)
            return sampleTime;

        // Return time difference between current and previous request
        return (currentTime - requestTimerPreviousTime);
    }


    public void showGraphA(String response){

        if(rqTimerA != null) {
            // get time stamp with SystemClock
            long requestTimerCurrentTime = SystemClock.uptimeMillis(); // current time
            rqTimerTimeStamp += timingIncrease(requestTimerCurrentTime);

            // get raw data from JSON response
            double rawData = getRawDataFromResponseA(response);

            // update chart
            if (isNaN(rawData)) {
               // errorHandling(COMMON.ERROR_NAN_DATA);

            } else {

                // update plot series
                double timeStamp = rqTimerTimeStamp / 1000.0; // [sec]
                boolean scrollGraph = (timeStamp > dataGraphMaxX);

                dataSeriesA.setColor(Color.GREEN);


                    dataSeriesA.appendData(new DataPoint(timeStamp, rawData), scrollGraph, dataGraphMaxDataPointsNumber);


                // refresh chart
                dataGraph.onDataChanged(true, true);
            }

            // remember previous time stamp
            requestTimerPreviousTime = requestTimerCurrentTime;
        }
    }

    public void showGraphB(String response){

        if(rqTimerB != null) {
            // get time stamp with SystemClock
            long requestTimerCurrentTime = SystemClock.uptimeMillis(); // current time
            rqTimerTimeStamp += timingIncrease(requestTimerCurrentTime);

            // get raw data from JSON response
            double rawData = getRawDataFromResponseB(response);

            // update chart
            if (isNaN(rawData)) {
                // errorHandling(COMMON.ERROR_NAN_DATA);

            } else {

                // update plot series
                double timeStamp = rqTimerTimeStamp / 1000.0; // [sec]
                boolean scrollGraph = (timeStamp > dataGraphMaxX);


                dataSeriesB.setColor(Color.RED);




                dataSeriesB.appendData(new DataPoint(timeStamp, rawData), scrollGraph, dataGraphMaxDataPointsNumber);



                // refresh chart
                dataGraph2.onDataChanged(true, true);
            }

            // remember previous time stamp
            requestTimerPreviousTime = requestTimerCurrentTime;
        }else{
            dataGraph2.getGridLabelRenderer().setHorizontalLabelsVisible(false);
        }
    }

    public void showGraphC(String response){

        if(rqTimerC != null) {
            // get time stamp with SystemClock
            long requestTimerCurrentTime = SystemClock.uptimeMillis(); // current time
            rqTimerTimeStamp += timingIncrease(requestTimerCurrentTime);

            // get raw data from JSON response
            double rawData = getRawDataFromResponseC(response);

            // update chart
            if (isNaN(rawData)) {
                // errorHandling(COMMON.ERROR_NAN_DATA);

            } else {

                // update plot series
                double timeStamp = rqTimerTimeStamp / 1000.0; // [sec]
                boolean scrollGraph = (timeStamp > dataGraphMaxX);


                dataSeriesC.setColor(Color.BLUE);



                    dataSeriesC.appendData(new DataPoint(timeStamp, rawData), scrollGraph, dataGraphMaxDataPointsNumber);



                // refresh chart
                dataGraph3.onDataChanged(true, true);

            }

            // remember previous time stamp
            requestTimerPreviousTime = requestTimerCurrentTime;
        }
    }
}
