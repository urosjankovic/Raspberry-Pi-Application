package com.example.sensehatdataapp;

import android.view.LayoutInflater;
import android.view.ViewGroup;

import com.example.sensehatdataapp.databinding.ItemMeasurementOriBinding;

import java.util.List;

import androidx.annotation.NonNull;

import androidx.recyclerview.widget.RecyclerView;

public class MeasurementsAdapter_Ori extends
        RecyclerView.Adapter<MeasurementsAdapter_Ori.MeasurementViewHolder> {

    public class MeasurementViewHolder extends RecyclerView.ViewHolder {

        // Since our layout file is item_measurement_data.xml,
        // our auto generated binding class is ItemMeasurementDataBinding.
        //
        // "underscores" to "Pascal case"
        private ItemMeasurementOriBinding binding;

        /**
         * @brief MeasurementViewHolder parametric constructor.
         * @param binding Data binding for single 'recyclerview' item.
         */
        public MeasurementViewHolder(ItemMeasurementOriBinding binding) {
            super(binding.getRoot());
            this.binding = binding;
        }

        /**
         * @brief Binds measurement view model with 'recyclerview' item.
         * @param measurement View model for single measurement item.
         */
        public void bind(MeasurementViewModel measurement) {
            binding.setMeasurement(measurement);
            binding.executePendingBindings();
        }
    }

    // Store a member variable for the measurements
    private List<MeasurementViewModel> mMeasurements;

    /**
     * @brief MeasurementsAdapter_Ori parametric constructor.
     * @param measurements Measurements view models array.
     */
    public MeasurementsAdapter_Ori(List<MeasurementViewModel> measurements) {
        mMeasurements = measurements;
    }

    @NonNull
    @Override
    public MeasurementsAdapter_Ori.MeasurementViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        LayoutInflater layoutInflater = LayoutInflater.from(parent.getContext());
        ItemMeasurementOriBinding itemBinding = ItemMeasurementOriBinding.inflate(layoutInflater, parent, false);
        return new MeasurementsAdapter_Ori.MeasurementViewHolder(itemBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull MeasurementsAdapter_Ori.MeasurementViewHolder holder, int position) {
        MeasurementViewModel measurement = mMeasurements.get(position);
        holder.bind(measurement);
    }

    @Override
    public int getItemCount() {
        return mMeasurements != null ? mMeasurements.size() : 0;
    }
}


