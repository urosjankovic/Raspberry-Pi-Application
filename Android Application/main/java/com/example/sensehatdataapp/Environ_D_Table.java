package com.example.sensehatdataapp;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.view.View;

import com.example.sensehatdataapp.R;
import com.example.sensehatdataapp.databinding.ActivityEnvirondTableBinding; // Automatically generated class



import androidx.appcompat.app.AppCompatActivity;
import androidx.databinding.DataBindingUtil;
import androidx.lifecycle.ViewModelProvider;
import androidx.recyclerview.widget.LinearLayoutManager;

public class Environ_D_Table extends AppCompatActivity {

    private ActivityEnvirondTableBinding binding;

    // Main view model (can be bind with other activities)
    //private MainViewModelMock viewModel;
    private MainViewModel viewModel;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
       // setContentView(R.layout.activity_environd__table);


        // Create new view model provider with MainViewModel class
        
        viewModel = new ViewModelProvider(this).get(MainViewModel.class);
        //if (savedInstanceState == null) {
            viewModel.Init(this); // Initialize if activity instance state is empty
        //}

        // Data binding utility
        binding = DataBindingUtil.setContentView(this, R.layout.activity_environd__table);
        // Binding data context of activity_main
        binding.setViewModel(viewModel);
        // Binding data context of rv_measurements
        binding.rvMeasurements.setLayoutManager(new LinearLayoutManager(this));
        binding.rvMeasurements.setAdapter(viewModel.getAdapter());

    }




    public void RefreshData(View v){
        Bundle extras=getIntent().getExtras();
        int val1=extras.getInt("val1");
        int val2=extras.getInt("val2");
        int val3=extras.getInt("val3");

        if(val1==1){
            //display data 

        }

        if(val2==1){
            //display data
        }

        if(val3==1){
            //display data
        }
    }
}
