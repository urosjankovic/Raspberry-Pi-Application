package com.example.sensehatdataapp;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.os.Handler;
import android.os.SystemClock;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.jjoe64.graphview.GraphView;
import com.jjoe64.graphview.LegendRenderer;
import com.jjoe64.graphview.series.DataPoint;
import com.jjoe64.graphview.series.LineGraphSeries;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.lang.reflect.Array;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;
import java.util.Timer;
import java.util.TimerTask;
import java.util.Vector;

import static java.lang.Double.isNaN;

public class Orientation_D_Graph extends AppCompatActivity {

    private final double dataGraphMaxX = 10.0d;
    private final double dataGraphMinX =  0.0d;
    private final double dataGraphMaxY =  360.0d;
    private final double dataGraphMinY = 0.0d;

    private final double dataGraphMaxYM =  50.0d;
    private final double dataGraphMinYM = -50.0d;

    private GraphView dataGraph;
    private GraphView dataGraph2;
    private GraphView dataGraph3;
    private LineGraphSeries<DataPoint> dataSeriesA_roll;
    private LineGraphSeries<DataPoint> dataSeriesB_roll;
    private LineGraphSeries<DataPoint> dataSeriesC_roll;
    private LineGraphSeries<DataPoint> dataSeriesA_pitch;
    private LineGraphSeries<DataPoint> dataSeriesB_pitch;
    private LineGraphSeries<DataPoint> dataSeriesC_pitch;
    private LineGraphSeries<DataPoint> dataSeriesA_yaw;
    private LineGraphSeries<DataPoint> dataSeriesB_yaw;
    private LineGraphSeries<DataPoint> dataSeriesC_yaw;


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


    public static String JSON_FILE="sensors_via_deamon.php?id=ori";





    TextView tv1, tv2, tv3;
    String ipAddress,url;
    int sampleTime, samplesvalue, dataGraphMaxDataPointsNumber;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_orientationd__graph);


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
        dataSeriesA_roll = new LineGraphSeries<>(new DataPoint[]{});
        dataSeriesB_roll = new LineGraphSeries<>(new DataPoint[]{});
        dataSeriesC_roll = new LineGraphSeries<>(new DataPoint[]{});
        dataSeriesA_pitch = new LineGraphSeries<>(new DataPoint[]{});
        dataSeriesB_pitch = new LineGraphSeries<>(new DataPoint[]{});
        dataSeriesC_pitch = new LineGraphSeries<>(new DataPoint[]{});
        dataSeriesA_yaw = new LineGraphSeries<>(new DataPoint[]{});
        dataSeriesB_yaw = new LineGraphSeries<>(new DataPoint[]{});
        dataSeriesC_yaw = new LineGraphSeries<>(new DataPoint[]{});

        dataSeriesA_roll.setTitle("x");
        dataSeriesB_roll.setTitle("x");
        dataSeriesC_roll.setTitle("x");
        dataSeriesA_pitch.setTitle("y");
        dataSeriesB_pitch.setTitle("y");
        dataSeriesC_pitch.setTitle("y");
        dataSeriesA_yaw.setTitle("z");
        dataSeriesB_yaw.setTitle("z");
        dataSeriesC_yaw.setTitle("z");

        dataGraph2.addSeries(dataSeriesB_roll);
        dataGraph2.addSeries(dataSeriesB_pitch);
        dataGraph2.addSeries(dataSeriesB_yaw);
        dataGraph.addSeries(dataSeriesA_roll);
        dataGraph.addSeries(dataSeriesA_pitch);
        dataGraph.addSeries(dataSeriesA_yaw);
        dataGraph3.addSeries(dataSeriesC_roll);
        dataGraph3.addSeries(dataSeriesC_pitch);
        dataGraph3.addSeries(dataSeriesC_yaw);

        dataGraph.getViewport().setXAxisBoundsManual(true);
        dataGraph.getViewport().setMinX(dataGraphMinX);
        dataGraph.getViewport().setMaxX(dataGraphMaxX);
        dataGraph.getViewport().setYAxisBoundsManual(true);
        dataGraph.getViewport().setMinY(dataGraphMinY);
        dataGraph.getViewport().setMaxY(dataGraphMaxY);
        dataGraph.getLegendRenderer().setVisible(true);
        dataGraph.getLegendRenderer().setAlign(LegendRenderer.LegendAlign.TOP);
        dataGraph.getLegendRenderer().setFixedPosition(800,-3);
        dataGraph.getGridLabelRenderer().setVerticalAxisTitle("Degrees");
        dataGraph.setTitle("Accelerometer");

        dataGraph2.getViewport().setXAxisBoundsManual(true);
        dataGraph2.getViewport().setMinX(dataGraphMinX);
        dataGraph2.getViewport().setMaxX(dataGraphMaxX);
        dataGraph2.getViewport().setYAxisBoundsManual(true);
        dataGraph2.getViewport().setMinY(dataGraphMinY);
        dataGraph2.getViewport().setMaxY(dataGraphMaxY);
        dataGraph2.getLegendRenderer().setVisible(true);
        //dataGraph2.getLegendRenderer().setAlign(LegendRenderer.LegendAlign.TOP);
        dataGraph2.getLegendRenderer().setFixedPosition(800,-6);
        dataGraph2.getGridLabelRenderer().setVerticalAxisTitle("Degrees");
        dataGraph2.setTitle("Gyroscope");


        dataGraph3.getViewport().setXAxisBoundsManual(true);
        dataGraph3.getViewport().setMinX(dataGraphMinX);
        dataGraph3.getViewport().setMaxX(dataGraphMaxX);
        dataGraph3.getViewport().setYAxisBoundsManual(true);
        dataGraph3.getViewport().setMinY(dataGraphMinYM);
        dataGraph3.getViewport().setMaxY(dataGraphMaxYM);
        dataGraph3.getLegendRenderer().setVisible(true);
        dataGraph3.getLegendRenderer().setAlign(LegendRenderer.LegendAlign.TOP);
        dataGraph3.getLegendRenderer().setFixedPosition(800,0);
        dataGraph3.getGridLabelRenderer().setVerticalAxisTitle("microTesla");
        dataGraph3.setTitle("Magnetometer");


        /* END initialize GraphView */


        // Initialize Volley request queue
        queue = Volley.newRequestQueue(Orientation_D_Graph.this);



        tv1= (TextView) findViewById(R.id.textView9);
        tv2= (TextView) findViewById(R.id.textView10);
        tv3= (TextView) findViewById(R.id.textView11);



        tv1.setText("IP address: "+ipAddress);
        tv2.setText("Sample time: "+Integer.toString(sampleTime)+"ms");
        tv3.setText("No. of samples: "+Integer.toString(samplesvalue));



    }


    private void startRequestTimer() {

        Bundle extras=getIntent().getExtras();
        int val1=extras.getInt("val1A");
        int val2=extras.getInt("val2A");
        int val3=extras.getInt("val3A");

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
        int val1=extras.getInt("val1A");
        int val2=extras.getInt("val2A");
        int val3=extras.getInt("val3A");

        if(val1==1 || val2==1 || val3==1) {
            startRequestTimer();
        }

           /* if (val1 == 1) {
                // tv1.setText("humidity");
              startRequestTimer();
            }


            if (val2 == 1) {
                //show graph
                startRequestTimerB();
            }


            if (val3 == 1) {
                //show graph
                startRequestTimerC();
            }*/

        if (val1 != 1 && val2 != 1 && val3 != 1) {
            Toast.makeText(Orientation_D_Graph.this, "No variables selected. \nSelect variables on variables page to view.", Toast.LENGTH_LONG).show();
        }
    }


    public void stopclick(View v){

        //stopRQTimer();
        stopRequestTimer();
    }



    private String getURL(String ip) {
        return ("http://" + ip + "/" + JSON_FILE);
    }

    //private String getURLB(String ip) { return ("http://" + ip + "/" + GYRO_FILE); }

    //private String getURLC(String ip) {
        //return ("http://" + ip + "/" + MAG_FILE);
    //}

    /**
     * @brief Initialize request timer period task with 'Handler' post method as 'sendGetRequest'.
     */

   /* private void errorHandling(int errorCode) {
        switch(errorCode) {
            case COMMON.ERROR_TIME_STAMP:
                textViewError.setText("ERR #1");
                Log.d("errorHandling", "Request time stamp error.");
                break;
            case COMMON.ERROR_NAN_DATA:
                textViewError.setText("ERR #2");
                Log.d("errorHandling", "Invalid JSON data.");
                break;
            case COMMON.ERROR_RESPONSE:
                textViewError.setText("ERR #3");
                Log.d("errorHandling", "GET request VolleyError.");
                break;
            default:
                textViewError.setText("ERR ??");
                Log.d("errorHandling", "Unknown error.");
                break;
        }
    }*/


    private JSONArray getRawDataFromResponseB(String response) {
        JSONArray jarray;
        JSONObject jObject=new JSONObject();
        double x = Double.NaN;
        double y = Double.NaN;
        double z = Double.NaN;
        JSONArray values =new JSONArray();

        // Create generic JSON object form string
        try {
            jarray = new JSONArray(response);

        } catch (JSONException e) {
            e.printStackTrace();
            return values;
        }

        // Read chart data form JSON object
        try {
            jObject = (JSONObject) jarray.getJSONObject(2).get("data");
            x=(Double) jObject.get("roll");
            y=(Double) jObject.get("pitch");
            z=(Double) jObject.get("yaw");
            values.put(x);
            values.put(y);
            values.put(z);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        return values;

    }


    private JSONArray getRawDataFromResponseA(String response) {

        JSONArray jarray;
        JSONObject jObject=new JSONObject();
        double x = Double.NaN;
        double y = Double.NaN;
        double z = Double.NaN;
        JSONArray values =new JSONArray();

        // Create generic JSON object form string
        try {
            jarray = new JSONArray(response);

        } catch (JSONException e) {
            e.printStackTrace();
            return values;
        }

        // Read chart data form JSON object
        try {
            jObject = (JSONObject) jarray.getJSONObject(0).get("data");
            x=(Double) jObject.get("roll");
            y=(Double) jObject.get("pitch");
            z=(Double) jObject.get("yaw");
            values.put(x);
            values.put(y);
            values.put(z);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        return values;
    }

    private JSONArray getRawDataFromResponseC(String response) {
        JSONArray jarray;
        JSONObject jObject=new JSONObject();
        double x = Double.NaN;
        double y = Double.NaN;
        double z = Double.NaN;
        JSONArray values =new JSONArray();

        // Create generic JSON object form string
        try {
            jarray = new JSONArray(response);

        } catch (JSONException e) {
            e.printStackTrace();
            return values;
        }

        // Read chart data form JSON object
        try {
            jObject = (JSONObject) jarray.getJSONObject(1).get("data");
            x=(Double) jObject.get("x");
            y=(Double) jObject.get("y");
            z=(Double) jObject.get("z");
            values.put(x);
            values.put(y);
            values.put(z);

        } catch (JSONException e) {
            e.printStackTrace();
        }
        return values;
    }

    public void sendGetRequestA(){



        // Request a string response from the provided URL
        StringRequest stringRequest = new StringRequest(Request.Method.GET, url,
                new Response.Listener<String>() {
                    @Override
                    public void onResponse(String response) {

                        //ERROR HERE
                        try {
                            showGraphA(response);
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
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
                        try {
                            showGraphB(response);
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
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
                        try {
                            showGraphC(response);
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
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
        // avoids holes in the plot
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


    public void showGraphA(String response) throws JSONException {

        if(rqTimerA != null) {
            // get time stamp with SystemClock
            long requestTimerCurrentTime = SystemClock.uptimeMillis(); // current time
            rqTimerTimeStamp += timingIncrease(requestTimerCurrentTime);

            // get raw data from JSON response
            double rawDataA = getRawDataFromResponseA(response).getDouble(0);

            // update chart
            if (isNaN(rawDataA)) {
                // errorHandling(COMMON.ERROR_NAN_DATA);

            } else {

                // update plot series
                double timeStamp = rqTimerTimeStamp / 1000.0; // [sec]
                boolean scrollGraph = (timeStamp > dataGraphMaxX);

                dataSeriesA_yaw.setColor(Color.BLUE);
                dataSeriesA_roll.setColor(Color.RED);
                dataSeriesA_pitch.setColor(Color.GREEN);


                dataSeriesA_roll.appendData(new DataPoint(timeStamp, rawDataA), scrollGraph, dataGraphMaxDataPointsNumber);
                dataSeriesA_pitch.appendData(new DataPoint(timeStamp, getRawDataFromResponseB(response).getDouble(1)), scrollGraph, dataGraphMaxDataPointsNumber);
                dataSeriesA_yaw.appendData(new DataPoint(timeStamp, getRawDataFromResponseB(response).getDouble(2)), scrollGraph, dataGraphMaxDataPointsNumber);


                // refresh chart
                dataGraph.onDataChanged(true, true);
            }

            // remember previous time stamp
            requestTimerPreviousTime = requestTimerCurrentTime;
        }
    }

    public void showGraphB(String response) throws JSONException {

        if(rqTimerB != null) {
            // get time stamp with SystemClock
            long requestTimerCurrentTime = SystemClock.uptimeMillis(); // current time
            rqTimerTimeStamp += timingIncrease(requestTimerCurrentTime);

            // get raw data from JSON response
            double rawDataA = getRawDataFromResponseB(response).getDouble(0);

            // update chart
            if (isNaN(rawDataA)) {
                // errorHandling(COMMON.ERROR_NAN_DATA);

            } else {

                // update plot series
                double timeStamp = rqTimerTimeStamp / 1000.0; // [sec]
                boolean scrollGraph = (timeStamp > dataGraphMaxX);


                dataSeriesB_yaw.setColor(Color.BLUE);
                dataSeriesB_roll.setColor(Color.RED);
                dataSeriesB_pitch.setColor(Color.GREEN);




                dataSeriesB_roll.appendData(new DataPoint(timeStamp, rawDataA), scrollGraph, dataGraphMaxDataPointsNumber);
                dataSeriesB_pitch.appendData(new DataPoint(timeStamp, getRawDataFromResponseB(response).getDouble(1)), scrollGraph, dataGraphMaxDataPointsNumber);
                dataSeriesB_yaw.appendData(new DataPoint(timeStamp, getRawDataFromResponseB(response).getDouble(2)), scrollGraph, dataGraphMaxDataPointsNumber);



                // refresh chart
                dataGraph2.onDataChanged(true, true);
            }

            // remember previous time stamp
            requestTimerPreviousTime = requestTimerCurrentTime;
        }
    }

    public void showGraphC(String response) throws JSONException {

        if(rqTimerC != null) {
            // get time stamp with SystemClock
            long requestTimerCurrentTime = SystemClock.uptimeMillis(); // current time
            rqTimerTimeStamp += timingIncrease(requestTimerCurrentTime);

            // get raw data from JSON response
            double rawDataA = getRawDataFromResponseC(response).getDouble(0);

            // update chart
            if (isNaN(rawDataA)) {
                // errorHandling(COMMON.ERROR_NAN_DATA);

            } else {

                // update plot series
                double timeStamp = rqTimerTimeStamp / 1000.0; // [sec]
                boolean scrollGraph = (timeStamp > dataGraphMaxX);


                dataSeriesC_yaw.setColor(Color.BLUE);
                dataSeriesC_roll.setColor(Color.RED);
                dataSeriesC_pitch.setColor(Color.GREEN);


                dataSeriesC_roll.appendData(new DataPoint(timeStamp, rawDataA), scrollGraph, dataGraphMaxDataPointsNumber);
                dataSeriesC_pitch.appendData(new DataPoint(timeStamp, getRawDataFromResponseC(response).getDouble(1)), scrollGraph, dataGraphMaxDataPointsNumber);
                dataSeriesC_yaw.appendData(new DataPoint(timeStamp, getRawDataFromResponseC(response).getDouble(2)), scrollGraph, dataGraphMaxDataPointsNumber);


                // refresh chart
                dataGraph3.onDataChanged(true, true);
            }

            // remember previous time stamp
            requestTimerPreviousTime = requestTimerCurrentTime;
        }
    }
}

