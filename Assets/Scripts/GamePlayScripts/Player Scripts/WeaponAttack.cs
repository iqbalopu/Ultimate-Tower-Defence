using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour {
    private WeaponController thisController_;
    private float fireCountDown = 0f;
    private Vector3 directionToShoot;
    public Action Shootbullet;
    private void Awake () {
        thisController_ = GetComponent<WeaponController> ();
        thisController_.Shoot += TriggerShoot;
    }

    public void SetDirectionForShooting (Vector3 dir) {
        directionToShoot = dir;
    }

    private void TriggerShoot () {
        if(fireCountDown <= 0) {
            // Shoot(directionToShoot);
            if (Shootbullet is not null) Shootbullet ();
            fireCountDown = PlayerController.Instance.DivideCount / thisController_.GetThisWeaponData().FireRate;
        }
        fireCountDown -= Time.deltaTime;
    }
    
    
}
