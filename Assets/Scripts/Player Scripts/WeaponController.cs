using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    [FormerlySerializedAs ("thisWeapon")] [SerializeField] private Weapon thisWeaponData;
    [SerializeField] private Transform RotateBody;
    private bool isActive;
    private WeaponAttack thisWeaponAttack_;
    private WeaponAreaDetection thisArea_;
    private WeaponHealth thisHealth_;
    private WeaponPool m_ThisPool;

    public Action Initialize;
    public Action Shoot;

    private void Awake() {
        thisWeaponAttack_ = GetComponent<WeaponAttack> ();
        thisArea_ = GetComponent<WeaponAreaDetection> ();
        thisHealth_ = GetComponent<WeaponHealth> ();
        m_ThisPool = GetComponent<WeaponPool> ();
    }

    public void InitializeWeapon(Weapon weapon) {
        if(Initialize is not null) Initialize ();
        thisWeaponData = weapon;
        isActive = true;
    }

    public void ResetTurret() {
        CancelInvoke("UpdateNearestEnemy");
        isActive = true;
        Initialize ();
    }

    public Weapon GetThisWeaponData () {
        return this.thisWeaponData;
    }

    // Update is called once per frame
    void Update() {
        if(!GameController.Instance.IsGameRunning()) return;
        if (!isActive) return;
        if (thisArea_.GetCurrentTargetTransform() == null) {
            return;
        }

        if(RotateBody is not null) RotateBodyObject ();
        if (Shoot is not null) {
            thisWeaponAttack_.SetDirectionForShooting(thisArea_.GetCurrentTargetTransform().position - RotateBody.position);
            Shoot ();
        }
    }

    private void RotateBodyObject () {
        Vector3 dir = thisArea_.GetCurrentTargetTransform().position - RotateBody.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(RotateBody.rotation, lookRotation, Time.deltaTime*GameController.Instance.turretSmoothFactor).eulerAngles;
        RotateBody.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    public void SetPositionofWeapon(Transform pos) {
        this.transform.position = pos.position;
        this.transform.rotation = pos.rotation;
    }

    public WeaponAttack GetWeaponAttack () {
        return thisWeaponAttack_;
    }
    
    public WeaponAreaDetection GetWeaponArea () {
        return thisArea_;
    }
    
    public WeaponHealth GetWeaponHealth () {
        return thisHealth_;
    }
    
    public WeaponPool GetWeaponPool () {
        return m_ThisPool;
    }

    public int GetWeaponSellCost() {
        return thisWeaponData.WeaponCost;
    }

    public Sprite GetShopSprite() {
        return thisWeaponData.turretImage;
    }

}
