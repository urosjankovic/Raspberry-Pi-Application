package com.example.sensehatdataapp;

import androidx.appcompat.app.AppCompatActivity;

import android.graphics.Color;
import android.os.Bundle;
import android.util.ArraySet;
import android.util.Log;
import android.view.DragEvent;
import android.view.View;
import android.widget.Button;
import android.widget.SeekBar;
import android.widget.TableLayout;
import android.widget.Toast;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.lang.reflect.Array;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Vector;

import javax.xml.transform.ErrorListener;

public class LEDActivity extends AppCompatActivity {

    int redActiveColor=0x00, greenActiveColor=0x00, blueActiveColor=0x00, ledActivecolor, alphaval, ledOffClr=0xFFB1ABAB;
    String ip, url;
    Integer[][][] ledModel=new Integer[8][8][3];

    private RequestQueue queue;


    String FILE_NAME="ledmatrix.php";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_l_e_d);

        final Button ClrShadeBtn= (Button) findViewById(R.id.ClrShadeBtn);

        ip=Server_param.globalip;
        url= getURL(ip);

        SeekBar seekBarR= (SeekBar) findViewById(R.id.seekBarR);
        SeekBar seekBarG= (SeekBar) findViewById(R.id.seekBarG);
        SeekBar seekBarB= (SeekBar) findViewById(R.id.seekBarB);

        seekBarR.setMax(255);
        seekBarG.setMax(255);
        seekBarB.setMax(255);

        seekBarR.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            int progressval=0;
            @Override
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                progressval=progress;
            }

            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {

            }

            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {
                redActiveColor=progressval;
                alphaval=(redActiveColor+greenActiveColor+blueActiveColor)/3;
                ledActivecolor=argbtoInt(redActiveColor,greenActiveColor,blueActiveColor,alphaval);
                ClrShadeBtn.setBackgroundColor(ledActivecolor);
            }
        });

        seekBarG.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            int progressval=0;
            @Override
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                progressval=progress;
            }

            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {

            }

            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {
                greenActiveColor=progressval;
                alphaval=(redActiveColor+greenActiveColor+blueActiveColor)/3;
                ledActivecolor=argbtoInt(redActiveColor,greenActiveColor,blueActiveColor,alphaval);
                ClrShadeBtn.setBackgroundColor(ledActivecolor);
            }
        });

        seekBarB.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            int progressval=0;
            @Override
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                progressval=progress;
            }

            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {

            }

            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {
                blueActiveColor=progressval;
                alphaval=(redActiveColor+greenActiveColor+blueActiveColor)/3;
                ledActivecolor=argbtoInt(redActiveColor,greenActiveColor,blueActiveColor,alphaval);
                ClrShadeBtn.setBackgroundColor(ledActivecolor);
            }
        });

        queue=Volley.newRequestQueue(this);






    }


    public void ClearModel(){
    //send all colors to zero
        for (int i=0; i<8; i++){
            for(int j=0; j<8; j++){
                ledModel[i][j][0]=null;
                ledModel[i][j][1]=null;
                ledModel[i][j][2]=null;
            }
        }

    }

    public void ClearLeds(View v){

        TableLayout Table= (TableLayout) findViewById(R.id.Table);
        View L;
        //send all colors to zero
        for (int i=0; i<8; i++){
            for(int j=0; j<8; j++){
                L=Table.findViewWithTag(indexTotag(i,j));
                L.setBackgroundColor(ledOffClr);
            }
        }

        ClearModel();

        SendClearRequest();

    }

    public int argbtoInt(int r, int g, int b, int a){
        return (r&0xff)<<16 | (g&0xff)<<8 |(b&0xff) |(a&0xff)<<24;
    }

    private String getURL(String ip) {
        return ("http://" + ip + "/" + FILE_NAME);
    }

    public void LEDOnClick(View v){
        v.setBackgroundColor(ledActivecolor);

        //updates model
        String tag=(String)v.getTag();
        Vector index= tagToindex(tag);

        int x=(int)index.get(0);
        int y=(int)index.get(1);

        ledModel[x][y][0]=redActiveColor;
        ledModel[x][y][1]=greenActiveColor;
        ledModel[x][y][2]=blueActiveColor;
    }

    Vector tagToindex(String tag){
        Vector v=new Vector(2);
        v.add(0,Character.getNumericValue(tag.charAt(3)));
        v.add(1,Character.getNumericValue(tag.charAt(4)));
        return v;
    }

    String indexTotag(int x, int y){
        return "LED"+Integer.toString(x)+Integer.toString(y);
    }



    JSONArray indexToJson(int x, int y){

        JSONArray J=new JSONArray();
        String xJ=Integer.toString(x);
        String yJ=Integer.toString(y);
        String rJ=Integer.toString(ledModel[x][y][0]);
        String gJ=Integer.toString(ledModel[x][y][1]);
        String bJ=Integer.toString(ledModel[x][y][2]);

        //"["+rJ+","+gJ+","+bJ+"]";
        J.put(rJ);
        J.put(gJ);
        J.put(bJ);
        return  J;
    }

    Boolean LedNotNull(int x, int y){
        return !((ledModel[x][y][0]==null)||(ledModel[x][y][1])==null||(ledModel[x][y][2]==null));
    }



    public JSONObject getChangedLeds(){

        String ledIndex;
        JSONArray ledJsonarray= new JSONArray();
        //Map <String, String> Values =new HashMap<>();
        JSONObject Values= new JSONObject();

        for (int i=0; i<8; i++){
            for (int j=0; j<8; j++){
                if(LedNotNull(i,j)) {
                    ledIndex = indexTotag(i, j);
                    ledJsonarray = indexToJson(i, j);



                    try {
                        Values.put(ledIndex,ledJsonarray);
                    } catch (JSONException e) {
                        e.printStackTrace();
                    }
                }
            }
        }
        return Values;
    }

    public JSONObject ClearPhyLED(){
        JSONArray jarray=new JSONArray();
        JSONObject LedsClear= new JSONObject();
        String ledIndex;

        for (int i=0; i<8; i++){
            for (int j=0; j<8; j++) {
                if (LedNotNull(i, j)) {
                    jarray = indexToJson(i, j);
                    ledIndex = indexTotag(i, j);
                    try {
                        LedsClear.put(ledIndex, jarray);
                    } catch (JSONException e) {
                        e.printStackTrace();
                    }
                }
            }
        }
        return LedsClear;
    }

    public void SendColor(View v){

        JsonObjectRequest requestv= new JsonObjectRequest(Request.Method.POST, url, getChangedLeds(), new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject response) {

                    Toast.makeText(LEDActivity.this, "Sent.", Toast.LENGTH_SHORT).show();

            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Toast.makeText(LEDActivity.this, "Not Sent. Verify IP Address.", Toast.LENGTH_LONG).show();
            }
        }
        );

        queue.add(requestv);


    }

    public void SendClearRequest() {


        JsonObjectRequest requestv = new JsonObjectRequest(Request.Method.POST, url, ClearPhyLED(), new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject response) {

            }
        }, null
        );

        queue.add(requestv);


    }

}
