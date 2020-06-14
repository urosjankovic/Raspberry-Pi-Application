package com.example.sensehatdataapp;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;

import androidx.appcompat.app.AppCompatActivity;


public class MainActivity extends AppCompatActivity {



    public CheckBox Humiditycb, tempcb, pressurecb;


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

        Humiditycb=(CheckBox) findViewById(R.id.Humiditycb);
        tempcb=(CheckBox) findViewById(R.id.tempcb);
        pressurecb=(CheckBox) findViewById(R.id.pressurecb);



        Button GoBtn= (Button) findViewById(R.id.GoBtn);
        GoBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                //read value from checkboxes
                int val1=0, val2=0, val3=0;

                if(Humiditycb.isChecked()){
                    val1=1;
                }

                if(tempcb.isChecked()){
                    val2=1;
                }

                if(pressurecb.isChecked()){
                    val3=1;
                }
                Intent secondIntent= new Intent(getApplicationContext(),ShowVar.class);
                Bundle checkedval = new Bundle();
                checkedval.putInt("val1", val1);
                checkedval.putInt("val2", val2);
                checkedval.putInt("val3", val3);
                secondIntent.putExtras(checkedval);
                startActivity(secondIntent);

            }
        });


        Button LEDBtn= (Button) findViewById(R.id.LEDBtn);
        LEDBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent thirdIntent= new Intent(getApplicationContext(),LEDActivity.class);
                startActivity(thirdIntent);

            }
        });




    }
}
