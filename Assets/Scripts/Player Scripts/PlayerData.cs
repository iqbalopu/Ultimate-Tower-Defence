using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {
    [SerializeField] private int gemCount_;

    public Action UpdateGemText;
    public int GetGemCount () {
        return gemCount_;
    }

    public void IncreaseGemCount (int value) {
        gemCount_ += value;
        if(UpdateGemText is not null) UpdateGemText ();
    }

    public void DeductGemCount (int value) {
        gemCount_ -= value;
        if (gemCount_ < 0) gemCount_ = 0;
        if(UpdateGemText is not null) UpdateGemText ();
    }
    

}
