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

        String new_ip=IPvalText.getText().toString();
        String new_sampletime=sampletimeText.getText().toString();
        String new_samples=sampleText.getText().toString();

        int new_sampletime_int=Integer.parseInt(new_sampletime);
        int new_samples_int=Integer.parseInt(new_samples);

        if(new_ip.length()!=0){
            if(new_ip!=Server_param.defaultipAddress){
                Server_param.globalip=IPvalText.toString();
            }
        }
       if(new_sampletime.length()!=0){
           if(new_sampletime_int!=Server_param.defaultsampleTime){
               Server_param.globalsampletime=new_sampletime_int;
           }
       }
        if(new_samples.length()!=0){
            if(new_samples_int!=Server_param.defaultsamples){
                Server_param.globalsamples=new_samples_int;
            }
        }
        if(new_ip.isEmpty()&&new_samples.isEmpty()&&new_sampletime.isEmpty()){
            Toast.makeText(Config.this,"No changes made.", Toast.LENGTH_LONG).show();
        }



    }



}
