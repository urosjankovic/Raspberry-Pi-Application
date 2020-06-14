package com.example.sensehatdataapp;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

public class Config extends AppCompatActivity {

    EditText IPvalText, ServerpText, sampletimeText;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_config);

        EditText IPvalText=(EditText) findViewById(R.id.IPvalText);
        EditText ServerpText=(EditText) findViewById(R.id.ServerpText);
        EditText sampletimeText=(EditText) findViewById(R.id.sampletimeText);






        Button ApplyBtn= (Button) findViewById(R.id.ApplyBtn);


    }

    public void onclickApply(View v){

        String a=IPvalText.getText().toString();
        String b=sampletimeText.getText().toString();
        String c=ServerpText.getText().toString();

        if(a.length()!=0){
            ShowVar.ipAddress=IPvalText.toString();
        }
       if(b.length()!=0){
           ShowVar.sampleTime=Integer.parseInt(sampletimeText.toString());
       }
        if(c.length()!=0){
            ShowVar.serverPort=Integer.parseInt(ServerpText.toString());
        }
        if(a.isEmpty()&&b.isEmpty()&&c.isEmpty()){
            Toast.makeText(Config.this,"No changes made.", Toast.LENGTH_LONG).show();
        }


    }



}
