using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugPanel : MonoBehaviour {

    public static DebugPanel Instance;
    public GameObject panelObject;
    private bool isPanelActive = false;
    [SerializeField] private TextMeshProUGUI att_1_value;
    [SerializeField] private TextMeshProUGUI att_2_value;
    [SerializeField] private TextMeshProUGUI att_3_value;
    [SerializeField] private Slider att_4_silder_value;

    public Action SliderValueChanged;
    
    private void Awake () {
        if (Instance == null) Instance = this;
    }

    public void ToggleDebugConsole () {
        isPanelActive = !isPanelActive;
        panelObject.SetActive(isPanelActive);
    }

    public void Change_Attr_1_Value (string value) {
        att_1_value.text = value;
    }
    
    public void Change_Attr_2_Value (string value) {
        att_2_value.text = value;
    }
    
    public void Change_Attr_3_Value (string value) {
        att_3_value.text = value;
    }

    public void OnSliderValueChange () {
        SliderValueChanged ();
    }

    public void ChangeSliderValue (float value, float orginalMin, float orginalMax) {
        att_4_silder_value.value = Utills.ConvertRange (orginalMin, orginalMax, 0f, 1f, value);
    }

    public float GetZoomSliderValue (float orginalMin, float orginalMax) {
        return Utills.ConvertRange(0f, 1f, orginalMin, orginalMax, att_4_silder_value.value);
    }


}
