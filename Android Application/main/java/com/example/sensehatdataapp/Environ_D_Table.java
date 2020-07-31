package com.example.sensehatdataapp;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.view.View;

import com.example.sensehatdataapp.databinding.ActivityEnvirondTableBinding; // Automatically generated class




import androidx.databinding.DataBindingUtil;
import androidx.lifecycle.ViewModelProvider;
import androidx.recyclerview.widget.LinearLayoutManager;

/**
 * @brief Class for environmental data table
 */
public class Environ_D_Table extends AppCompatActivity {

    private ActivityEnvirondTableBinding binding;

    // Main view model (can be bind with other activities)
    //private MainViewModelMock viewModel;
    private MainViewModel viewModel;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
       setContentView(R.layout.activity_environd__table);




        // Create new view model provider with MainViewModel class

        viewModel = new ViewModelProvider(this).get(MainViewModel.class);
        if (savedInstanceState == null) {
            viewModel.Init(this); // Initialize if activity instance state is empty
        }

        // Data binding utility
        binding = DataBindingUtil.setContentView(this, R.layout.activity_environd__table);
        // Binding data context of activity_main
        binding.setViewModel(viewModel);
        // Binding data context of rv_measurements
        binding.rvMeasurements.setLayoutManager(new LinearLayoutManager(this));
        binding.rvMeasurements.setAdapter(viewModel.getAdapterA());


    }



}
