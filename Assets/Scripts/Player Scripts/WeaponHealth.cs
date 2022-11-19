using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHealth : MonoBehaviour {
    private WeaponController thisController_;
    private int CurrentHealth;

    private void Awake () {
        thisController_ = GetComponent<WeaponController> ();
        thisController_.Initialize += InitCurrentHealth;
    }

    public void InitCurrentHealth () {
        CurrentHealth = thisController_.GetThisWeaponData().Health;
    }
    
    public void DecreaseHealth(){}
    
}
