package com.example.sensehatdataapp;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;

public class ChooseVar1 extends AppCompatActivity {

    public CheckBox Humiditycb, tempcb, pressurecb;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_choose_var1);


        Humiditycb=(CheckBox) findViewById(R.id.humiditycb);
        tempcb=(CheckBox) findViewById(R.id.tempcb);
        pressurecb=(CheckBox) findViewById(R.id.pressurecb);

        Button GoBtn= (Button) findViewById(R.id.GoBtn);
        GoBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                //Bundle extras=getIntent().getExtras();
                //int ang=extras.getInt("ang");

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
                //Intent firstIntent= new Intent(getApplicationContext(),ShowVar.class);
                //Intent secondIntent= new Intent(getApplicationContext(),Environ_D_Table.class);
                Intent thirdIntent= new Intent(getApplicationContext(),TableGraphView.class);

                Bundle checkedval = new Bundle();
                checkedval.putInt("val1", val1);
                checkedval.putInt("val2", val2);
                checkedval.putInt("val3", val3);



                thirdIntent.putExtras(checkedval);



                startActivity(thirdIntent);

            }
        });
    }
}
