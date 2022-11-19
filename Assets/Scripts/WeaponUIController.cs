using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUIController : MonoBehaviour
{
    public void ButtonPressed_CancelWeaponUI () {
        PlayerController.Instance.GetPlayerUI().HideAllWeaponUI();
    }
    
}
