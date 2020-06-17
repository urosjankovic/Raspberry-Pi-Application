package com.example.sensehatdataapp;

import androidx.appcompat.app.AppCompatActivity;

import android.graphics.Color;
import android.os.Bundle;
import android.util.Log;
import android.view.DragEvent;
import android.view.View;
import android.widget.Button;
import android.widget.SeekBar;
import android.widget.TableLayout;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;

import java.util.HashMap;
import java.util.Map;
import java.util.Vector;

import javax.xml.transform.ErrorListener;

public class LEDActivity extends AppCompatActivity {

    int redActiveColor=0x00, greenActiveColor=0x00, blueActiveColor=0x00, ledActivecolor, alphaval, ledOffClr=0xFFB1ABAB;
    String ip="192.168.0.2";
    Integer[][][] ledModel=new Integer[8][8][3];

    private RequestQueue queue;

    String url= getURL(ip);
    String FILE_NAME="ledmatrix.php";
    Map<String, String> LedsClear= new HashMap<String, String>();
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_l_e_d);

        final Button ClrShadeBtn= (Button) findViewById(R.id.ClrShadeBtn);


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

    public void ClearUIandModel(View v){

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

    String indexToJson(int x, int y){
        String xJ=Integer.toString(x);
        String yJ=Integer.toString(y);
        String rJ=Integer.toString(ledModel[x][y][0]);
        String gJ=Integer.toString(ledModel[x][y][1]);
        String bJ=Integer.toString(ledModel[x][y][2]);

        return "["+xJ+","+yJ+","+rJ+","+gJ+","+bJ+"]";
    }

    Boolean LedNotNull(int x, int y){
        return !((ledModel[x][y][0]==null)||(ledModel[x][y][1])==null||(ledModel[x][y][2]==null));
    }

    public Map<String, String> getChangedLeds(){

        String ledIndex, ledJson;
        Map <String, String> Values =new HashMap<>();

        for (int i=0; i<8; i++){
            for (int j=0; j<8; j++){
                if(LedNotNull(i,j)) {
                    ledIndex = indexTotag(i, j);
                    ledJson = indexToJson(i, j);
                    Values.put(ledIndex, ledJson);
                }
            }
        }
        return Values;
    }

    public void SendColor(View v){
        url="";
        StringRequest requestv= new StringRequest(Request.Method.POST, url, new Response.Listener<String>() {
            @Override
            public void onResponse(String response) {
                Log.d("response",response);
            }
        }, null){
            @Override
        protected Map<String, String> getParams(){
        return getChangedLeds();
        }};

        queue.add(requestv);

    }

    public void SendClearRequest(){
        url="";
        StringRequest requestv= new StringRequest(Request.Method.POST, url, new Response.Listener<String>() {
            @Override
            public void onResponse(String response) {
                Log.d("response",response);
            }
        }, null){
            @Override
            protected Map<String, String> getParams(){
                return LedsClear;
            }};

        queue.add(requestv);
    }

}
