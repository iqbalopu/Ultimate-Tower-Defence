using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup SettingsUIGroup;
    [SerializeField] private bool isInverseCameraGrip = false;
    [SerializeField] private Toggle InverseCameraGrip;
    [SerializeField] private Animator GameOverAnimator;

    

    private void Start () {
        HideSettingsPopUp ();
        isInverseCameraGrip = InverseCameraGrip.isOn;
    }

    public void ShowSettingsPopUp () {
        SettingsUIGroup.alpha = 1f;
        SettingsUIGroup.blocksRaycasts = true;
    }
    
    public void HideSettingsPopUp () {
        SettingsUIGroup.alpha = 0f;
        SettingsUIGroup.blocksRaycasts = false;
    }

    public void OnInverseToggle () {
        Debug.Log("Toggling");
        isInverseCameraGrip = InverseCameraGrip.isOn;
    }

    public bool IsInverseActive () {
        return isInverseCameraGrip;
    }

    public void ShowGameOver () {
        GameOverAnimator.SetTrigger("show");
    }

    public void HideGameOver () {
        GameOverAnimator.SetTrigger("hide");
    }
    
}
