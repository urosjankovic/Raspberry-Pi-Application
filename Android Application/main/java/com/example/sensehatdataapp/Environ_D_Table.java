package com.example.sensehatdataapp;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.view.View;

public class Environ_D_Table extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_environd__table);
    }

    public void RefreshData(View v){
        Bundle extras=getIntent().getExtras();
        int val1=extras.getInt("val1");
        int val2=extras.getInt("val2");
        int val3=extras.getInt("val3");

        if(val1==1){
            //display data -read the data from json array? with units and variable name

        }

        if(val2==1){
            //display data
        }

        if(val3==1){
            //display data
        }
    }
}
