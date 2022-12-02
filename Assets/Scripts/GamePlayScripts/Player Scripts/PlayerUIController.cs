using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] public TextMeshProUGUI gemText;
    [SerializeField] public GameObject gemObject;
    [SerializeField] public GameObject scoreObject;
    [SerializeField] public Animator scoreAnim;
    [SerializeField] public Transform WeaponUIParent;
    [SerializeField] public GameObject WeaponActionUI;

    private PlayerController controller_;

    private void Awake () {
        controller_ = GetComponent<PlayerController> ();
        GameController.Instance.ExitGamePlay += OnExitGameplay;
        GameController.Instance.HideAllWeaponUI += HideAllWeaponUI;
        controller_.ShowWeaponUI += ShowWeaponUI;
        controller_.HideWeaponUI += HideWeaponUI;
        controller_.ShowWeaponAction += ShowWeaponAction;
        GameController.Instance.StartGamePlay += StartGameUIUpdate;
    }

    private void Start () {
        HideWeaponUI();
        HideAllWeaponUI();
        this.HideScoreCard();
        this.HideGemObject();
        controller_.GetPlayerData ().UpdateGemText += UpdateGemText;
        controller_.GetPlayerScore ().UpdateScore += UpdateScoreText;
        SetGemText(""+controller_.GetPlayerData().GetGemCount());
    }

    private void OnExitGameplay () {
        HideScoreCard();
        HideGemObject();
    }

    private void UpdateScoreText () {
        scoreText.text = "" + controller_.GetPlayerScore().GetPlayerScore();
        scoreAnim.SetTrigger("Increase");
    }

    private void UpdateGemText () {
        SetGemText ("" + controller_.GetPlayerData ().GetGemCount ());
    }
    
    
    
    public void SetGemText(string s) {
        gemText.text = s;
    }

    private void ShowWeaponUI () {
        WeaponUIParent.gameObject.SetActive(true);
    }
    
    private void HideWeaponUI () {
        WeaponUIParent.gameObject.SetActive(false);
    }

    private void ShowScoreCard () {
        scoreObject.SetActive(true);
    }
    
    private void HideScoreCard () {
        scoreObject.SetActive(false);
    }

    private void ShowGemObject () {
        gemObject.SetActive(true);
    }
    
    private void HideGemObject () {
        gemObject.SetActive(false);
    }

    private void ShowWeaponAction () {
        WeaponActionUI.SetActive(true);
    }
    
    private void HideWeaponAction () {
        WeaponActionUI.SetActive(false);
    }
    
    public void HideAllWeaponUI () {
        // Debug.LogError("Calling from PlayerGamePlayController");
        HideWeaponUI();
        HideWeaponAction();
    }

    private void StartGameUIUpdate () {
        ShowGemObject ();
        ShowScoreCard();
    }
    
    
}
