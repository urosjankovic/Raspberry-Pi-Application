package com.example.sensehatdataapp;

import org.json.JSONArray;

public interface TableVolleyListener {
    void onError(String message);
    void onResponse(JSONArray response);
}
