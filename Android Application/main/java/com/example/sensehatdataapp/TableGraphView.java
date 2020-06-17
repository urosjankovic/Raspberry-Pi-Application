package com.example.sensehatdataapp;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;

public class TableGraphView extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_tablegraph_view);




        Button TableBtn= (Button) findViewById(R.id.TableBtn);
        TableBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                //if statements for if angle or env data
                Bundle extras=getIntent().getExtras();
                int ang=extras.getInt("ang");
                int val1=extras.getInt("val1");
                int val2=extras.getInt("val2");
                int val3=extras.getInt("val3");

                int val1A=extras.getInt("val1A");
                int val2A=extras.getInt("val2A");
                int val3A=extras.getInt("val3A");

                Bundle checkedval = new Bundle();
                checkedval.putInt("val1", val1);
                checkedval.putInt("val2", val2);
                checkedval.putInt("val3", val3);

                checkedval.putInt("val1A", val1A);
                checkedval.putInt("val2A", val2A);
                checkedval.putInt("val3A", val3A);

                if(ang==1) {
                    Intent firstIntent = new Intent(getApplicationContext(), Orientation_D_Tables.class);
                    firstIntent.putExtras(checkedval);
                    startActivity(firstIntent);
                }
                else{
                    Intent secIntent = new Intent(getApplicationContext(), Environ_D_Table.class);
                    secIntent.putExtras(checkedval);
                    startActivity(secIntent);
                }

            }
        });


        Button GraphBtn= (Button) findViewById(R.id.GraphBtn);
        GraphBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                //if statements for if angle or env data

                Bundle extras=getIntent().getExtras();
                int ang=extras.getInt("ang");
                int val1=extras.getInt("val1");
                int val2=extras.getInt("val2");
                int val3=extras.getInt("val3");

                int val1A=extras.getInt("val1A");
                int val2A=extras.getInt("val2A");
                int val3A=extras.getInt("val3A");

                Bundle checkedval = new Bundle();
                checkedval.putInt("val1", val1);
                checkedval.putInt("val2", val2);
                checkedval.putInt("val3", val3);

                checkedval.putInt("val1A", val1A);
                checkedval.putInt("val2A", val2A);
                checkedval.putInt("val3A", val3A);




                if(ang==1) {
                    Intent thirdIntent = new Intent(getApplicationContext(), Orientation_D_Graph.class);
                    thirdIntent.putExtras(checkedval);
                    startActivity(thirdIntent);
                }
               else{
                    Intent fourthIntent = new Intent(getApplicationContext(), ShowVar.class);
                    fourthIntent.putExtras(checkedval);
                    startActivity(fourthIntent);
                }

            }
        });
    }
}
