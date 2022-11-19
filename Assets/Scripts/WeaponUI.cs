using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public Weapon ThisWeapon;
    public Image weaponButtonImage;
    private int weaponIndex;

    private void Start() {
        SetWeaponUI();
    }

    private void SetWeaponUI() {
        if (ThisWeapon != null) {
            weaponButtonImage.sprite = ThisWeapon.turretImage;
            weaponIndex = ThisWeapon.WeaponID;
        }
    }

    public void OnClickWeaponButton() {
        Debug.Log("Clicking For WeaponID= " + ThisWeapon.WeaponID);
        PlayerInputController.Instance.OnWeaponSelected(ThisWeapon);
    }
}
