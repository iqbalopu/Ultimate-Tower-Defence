using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class TownHall : MonoBehaviour {
    public Image healthBar;
    [SerializeField] private float maxHealth_;
    [SerializeField] private Gradient healthGradiant_;
    private float currentHealth_;
    private bool isGameOver_ = false;

    private void Awake () {
        GameController.Instance.StartGamePlay += ResetTower;
    }

    private void ResetTower () {
        isGameOver_ = false;
    }

    public void DecreaseTowerHealth(int damage) {
        if(isGameOver_) return;
        StopCoroutine(ReduceHealthBar());
        this.SetHealthbarValue(Utills.ConvertRange(currentHealth_, maxHealth_, 0f, 1f, 0f));
        float remainingHealth = currentHealth_ - (float)damage;
        float previousHealth = currentHealth_;
        if (remainingHealth <= 0) {
            GameController.Instance.GameOver();
            isGameOver_ = true;
        } else {
            currentHealth_ = remainingHealth;
        }
        StartCoroutine(ReduceHealthBar());
    }
    
    
    IEnumerator ReduceHealthBar() {
        float currFillAmount = healthBar.fillAmount;
        float updatedFillAmount = Utills.ConvertRange(currentHealth_, maxHealth_, 0f, 1f, 0f);
        float difference = updatedFillAmount - currFillAmount;
        while(currFillAmount < updatedFillAmount) {
            yield return new WaitForEndOfFrame();
            difference -= Time.deltaTime;
            currFillAmount += Time.deltaTime;
            this.SetHealthbarValue(currFillAmount);
        }

        this.SetHealthbarValue(updatedFillAmount);
    }
    
    private void SetHealthbarValue(float fillValue) {
        healthBar.fillAmount = fillValue;
        healthBar.color = healthGradiant_.Evaluate(healthBar.fillAmount);
    }
}
