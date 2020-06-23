package com.example.sensehatdataapp;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

public class Config extends AppCompatActivity {



    EditText IPvalText, sampleText, sampletimeText;




    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_config);

        IPvalText=(EditText) findViewById(R.id.IPvalText);
        sampleText=(EditText) findViewById(R.id.ServerpText);
        sampletimeText=(EditText) findViewById(R.id.sampletimeText);



    }

    public void onclickApply(View v){

        //set default values

        String new_ip=IPvalText.getText().toString();
        String new_sampletime=sampletimeText.getText().toString();
        String new_samples=sampleText.getText().toString();



        if(!new_ip.equals("")||!new_sampletime.equals("")||!new_samples.equals("")) {
            if (!new_ip.equals("")) {
                if (new_ip != Server_param.defaultipAddress) {
                    Server_param.globalip = new_ip;
                }
                IPvalText.setText("");
            }

            if (!new_sampletime.equals("")) {
                int new_sampletime_int = Integer.parseInt(new_sampletime);
                if (new_sampletime_int != Server_param.defaultsampleTime) {
                    Server_param.globalsampletime = new_sampletime_int;
                }
                sampletimeText.setText("");
            }

            if (!new_samples.equals("")) {
                int new_samples_int = Integer.parseInt(new_samples);
                if (new_samples_int != Server_param.defaultsamples) {
                    Server_param.globalsamples = new_samples_int;
                }
                sampleText.setText("");
            }
            Toast.makeText(Config.this, "Parameters Saved", Toast.LENGTH_LONG).show();
        }

        if(new_ip.equals("")&&new_samples.equals("")&&new_sampletime.equals("")){
            Toast.makeText(Config.this,"No changes made.", Toast.LENGTH_LONG).show();
        }

    }



}
