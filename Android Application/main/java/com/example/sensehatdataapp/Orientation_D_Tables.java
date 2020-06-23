package com.example.sensehatdataapp;



import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.view.View;
import androidx.recyclerview.widget.LinearLayoutManager;

public class Orientation_D_Tables extends AppCompatActivity {



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_od__tables);


    }




    public void RefreshData(View v){
        Bundle extras=getIntent().getExtras();
        int val1=extras.getInt("val1A");
        int val2=extras.getInt("val2A");
        int val3=extras.getInt("val3A");

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
