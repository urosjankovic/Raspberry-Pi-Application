package com.example.sensehatdataapp;



import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;


import com.example.sensehatdataapp.databinding.ActivityOdTablesBinding;

import androidx.databinding.DataBindingUtil;
import androidx.lifecycle.ViewModelProvider;
import androidx.recyclerview.widget.LinearLayoutManager;
/**
 * @brief handles Orientation data table view
 */
public class Orientation_D_Tables extends AppCompatActivity {

    private ActivityOdTablesBinding binding;

    // Main view model (can be bind with other activities)
    //private MainViewModelMock viewModel;
    private MainViewModel viewModel;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_od__tables);

        // Create new view model provider with MainViewModel class

        viewModel = new ViewModelProvider(this).get(MainViewModel.class);
        if (savedInstanceState == null) {
            viewModel.Init2(this); // Initialize if activity instance state is empty
        }

        // Data binding utility
        binding = DataBindingUtil.setContentView(this, R.layout.activity_od__tables);
        // Binding data context of activity_main
        binding.setViewModel(viewModel);
        // Binding data context of rv_measurements
        binding.rvMeasurements.setLayoutManager(new LinearLayoutManager(this));
        binding.rvMeasurements.setAdapter(viewModel.getAdapterB());
    }



}
