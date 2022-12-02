using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [Header("Attributes")]
    public float bulletSpeed;
    public float DivideCount;

    [Header("Assignable Unity Component")]
    [FormerlySerializedAs ("turretParent")] public Transform weaponParent;
    
    private Vector3 currentSelectedBasePos;
    private Base currSelectedBase;
    private List<WeaponController> playerTurrets = new List<WeaponController>();
    private bool IsShowingBaseHighlight;

    private PlayerPoolController playerPool_;
    private PlayerUIController playerUI_;
    private PlayerScoreController playerScore_;
    private PlayerInputController playerInput_;
    private PlayerData playerData_;

#region Actions

    public Action ProcessInput;
    public Action ShowWeaponUI;
    public Action HideWeaponUI;
    public Action ShowWeaponAction;
    public Action HideWeaponAction;
    public Action ResetPlayerScore;
#endregion
    
    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
    
        playerPool_ = GetComponent<PlayerPoolController> ();
        playerUI_ = GetComponent<PlayerUIController> ();
        playerInput_ = GetComponent<PlayerInputController> ();
        playerScore_ = GetComponent<PlayerScoreController> ();
        playerData_ = GetComponent<PlayerData> ();
    }

    private void Start() {
        
        GetAllTurrets();
        this.ResetPlayer();
         
        //SetWeaponsInPosition();
    }

    

    private void Update() {
        if (!GameController.Instance.IsGameRunning() || IsShowingBaseHighlight) return;

        if (Input.GetMouseButtonDown(0)) {
            ProcessInput ();
        }
    }

    public PlayerData GetPlayerData () {
        return playerData_;
    }
    
    public PlayerInputController GetPlayerInput () {
        return playerInput_;
    }
    
    public PlayerUIController GetPlayerUI () {
        return playerUI_;
    }
    
    public PlayerPoolController GetPlayerPool() {
        return playerPool_;
    }
    
    public PlayerScoreController GetPlayerScore () {
        return playerScore_;
    }

    

    public void CheckAndShowWeaponsToDisplayForCurrBase(Base selectedBase) {
        currSelectedBase = selectedBase;
        currentSelectedBasePos = currSelectedBase.GetTurretPosFromBase();
        if (selectedBase.CanUseBase()) {
            //Show the UI for selecting weapon
            if(ShowWeaponUI is not null) ShowWeaponUI ();
        } else {
            //Show the UI for weapon action

            if (ShowWeaponAction is not null) ShowWeaponAction ();
        }
    }

    public void LevelUpSelectedWeapon() {
        //here will be the code to levelup selected weapon

        if (HideWeaponAction is not null) HideWeaponAction ();
    }

    public void SellSelectedWeapon() {
        Weapon wp = currSelectedBase.GetCurrentAssignedWeapon();
        WeaponController wc = currSelectedBase.GetCurrentAssignedWeaponController();
        if (wp != null && wc != null) {
            playerData_.IncreaseGemCount(wp.WeaponCost);
            Destroy(wc.gameObject);
            currSelectedBase.SetCurrentAssignedWeapon(null);
            currSelectedBase.SetCurrentAssignedWeaponController(null);
            currSelectedBase.SetBaseOccupency(false);
        }

        if (HideWeaponAction is not null) HideWeaponAction ();
    }

    public void OnWeaponSelected(Weapon selectedWeapon) {
        //Get the Weapon prefab using weaponIndex and Set the Weapon
        if(playerData_.GetGemCount() < selectedWeapon.WeaponCost) {
            Debug.LogError("Dont have enough Balance");
        } else {
            GameObject g = Instantiate(selectedWeapon.weaponPrefab, currentSelectedBasePos, currSelectedBase.GetBaseTransform().rotation);
            WeaponController wc = g.GetComponent<WeaponController>();
            wc.InitializeWeapon(selectedWeapon);
            currSelectedBase.SetCurrentAssignedWeapon(selectedWeapon);
            currSelectedBase.SetCurrentAssignedWeaponController(wc);
            currSelectedBase.SetBaseOccupency(true);
            playerData_.DeductGemCount(selectedWeapon.WeaponCost);
        }

        HideWeaponUI ();
    }
    

    public Vector3 GetCurrentBasePos() {//Check for null reff where we call this function
        return currentSelectedBasePos;
    }

    //public void SetTurretUI() {
    //    int turretCount = playerTurrets.Count;
    //    for (int i = 0; i < turretCount; i++) {
    //        GameObject g = Instantiate(weaponUiPrefab, WeaponUIParent);
    //        g.GetComponent<Image>().sprite = playerTurrets[i].GetShopSprite();
    //    }
    //}
    

    private void GetAllTurrets() {
        WeaponController[] turrets = weaponParent.GetComponentsInChildren<WeaponController>();
        for (int i = 0; i < turrets.Length; i++) {
            playerTurrets.Add(turrets[i]);
        }
    }

    //public void EnableTurrets() {
    //    if (playerTurrets.Count == 0) return;
    //    for (int i = 0; i < playerTurrets.Count; i++) {
    //        playerTurrets[i].StartCheckingForEnemy();
    //    }
    //}

    public void ResetTurrets() {
        if (playerTurrets.Count == 0) return;
        for (int i = 0; i < playerTurrets.Count; i++) {
            Destroy(playerTurrets[i].gameObject);
        }
    }

    private void SetWeaponsInPosition() {
        List<Transform> basePositions = WeaponBaseController.Instance.GetAllWeaponBaseTransform();
        for (int i = 0; i < playerTurrets.Count; i++) {
            playerTurrets[i].SetPositionofWeapon(basePositions[i].transform);
        }
    }

    public void ResetPlayer() {
       if(ResetPlayerScore is not null) ResetPlayerScore ();
    }

}
