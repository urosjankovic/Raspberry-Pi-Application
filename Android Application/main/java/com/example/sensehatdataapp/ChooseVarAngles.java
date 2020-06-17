package com.example.sensehatdataapp;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;

public class ChooseVarAngles extends AppCompatActivity {

    public CheckBox Acccb, Mgcb, Gycb;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_choose_var_angles);



        Acccb=(CheckBox) findViewById(R.id.Acccb);
        Mgcb=(CheckBox) findViewById(R.id.Mgcb);
        Gycb=(CheckBox) findViewById(R.id.Gycb);

        Button GoBtn= (Button) findViewById(R.id.GoBtn2);
        GoBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                Bundle extras=getIntent().getExtras();
                int ang=extras.getInt("ang");

                //read value from checkboxes
                int val1A=0, val2A=0, val3A=0;

                if(Acccb.isChecked()){
                    val1A=1;
                }

                if(Gycb.isChecked()){
                    val2A=1;
                }

                if(Mgcb.isChecked()){
                    val3A=1;
                }

                //Intent firstIntent= new Intent(getApplicationContext(),Orientation_D_Graph.class);
                //Intent secondIntent= new Intent(getApplicationContext(),Orientation_D_Tables.class);
                Intent thirdIntent= new Intent(getApplicationContext(),TableGraphView.class);

                Bundle checkedval = new Bundle();
                checkedval.putInt("val1A", val1A);
                checkedval.putInt("val2A", val2A);
                checkedval.putInt("val3A", val3A);

                checkedval.putInt("ang", ang);


                thirdIntent.putExtras(checkedval);

                startActivity(thirdIntent);

            }
        });
    }
}
