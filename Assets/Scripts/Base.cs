using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    public Transform weaponPos;
    private bool IsBaseOccupied;
    private Weapon currentAssignedWeapon;
    private WeaponController currWeaponController;

    public void ShowWeaponOptions() {

    }

    public Vector3 GetTurretPosFromBase() {
        return weaponPos.position;
    }

    public Transform GetBaseTransform() {
        return weaponPos;
    }

    public void SetBaseOccupency(bool value) {
        IsBaseOccupied = value;
    }

    public bool CanUseBase() {
        return !IsBaseOccupied;
    }

    public Weapon GetCurrentAssignedWeapon() {
        return currentAssignedWeapon;
    }

    public void SetCurrentAssignedWeapon(Weapon currWeapon) {
        currentAssignedWeapon = currWeapon;
    }

    public WeaponController GetCurrentAssignedWeaponController() {
        return currWeaponController;
    }

    public void SetCurrentAssignedWeaponController(WeaponController currWeapon) {
        currWeaponController = currWeapon;
    }

    public void HideAreaMeshForCurrentWeapon () {
        if(currWeaponController != null) currWeaponController.HideWeaponArea();
    }
}
