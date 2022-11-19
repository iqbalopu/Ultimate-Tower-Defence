using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAreaDetection : MonoBehaviour {
    private WeaponController thisController_;
    private Transform currentTargetTransform_;
    public EnemyDetector detector;
    
    private void Awake () {
        thisController_ = GetComponent<WeaponController> ();
        thisController_.Initialize += InitializeArea;
    }

    private void InitializeArea () {
        if (detector != null) detector.SetColliderRange(thisController_.GetThisWeaponData().RangeRadius);
        StartCheckingForEnemy();
    }
    
    public Transform GetCurrentTargetTransform () {
        return this.currentTargetTransform_;
    }
    
    private void StartCheckingForEnemy() {
        InvokeRepeating("UpdateNearestEnemy", 0.1f, Time.deltaTime);
    }
    
    private void UpdateNearestEnemy() {
        currentTargetTransform_ = detector.GetUpdatedTarget();
        //Debug.Log("Turrett Name= " + this.thisWeapon.Name + ", currentTargetTransform after Update= " + currentTargetTransform);
    }
    
}
