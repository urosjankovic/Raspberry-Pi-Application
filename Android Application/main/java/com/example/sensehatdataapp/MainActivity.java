package com.example.sensehatdataapp;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;


public class MainActivity extends AppCompatActivity {




    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Button settingsBtn= (Button) findViewById(R.id.settingsBtn);
        settingsBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent firstIntent= new Intent(getApplicationContext(),Config.class);
                startActivity(firstIntent);



            }
        });





        Button LEDBtn= (Button) findViewById(R.id.LEDBtn);
        LEDBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent firstIntent= new Intent(getApplicationContext(),LEDActivity.class);
                startActivity(firstIntent);

            }
        });



        Button EnvBtn= (Button) findViewById(R.id.EnvBtn);
        EnvBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                Intent secIntent= new Intent(getApplicationContext(),ChooseVar1.class);

                startActivity(secIntent);

            }
        });

        Button OriBtn= (Button) findViewById(R.id.OriBtn);



        OriBtn.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                int ang=1;

                Intent thirdIntent= new Intent(getApplicationContext(),ChooseVarAngles.class);
                //Intent adIntent= new Intent(getApplicationContext(),TableGraphView.class);
                thirdIntent.putExtra("ang",ang);

                startActivity(thirdIntent);

            }
        });





    }
}
