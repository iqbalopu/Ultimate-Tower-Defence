using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreController : MonoBehaviour {
    private int playerScore_ = 0;
    public Action UpdateScore;
    private PlayerController controller_;

    private void Awake () {
        controller_ = GetComponent<PlayerController> ();
        controller_.ResetPlayerScore += ResetPlayerScore;
    }

    public int GetPlayerScore () {
        return playerScore_;
    }

    public void IncreaseScore (int value) {
        playerScore_ += value;
        if(UpdateScore is not null) UpdateScore ();
    }

    private void ResetPlayerScore () {
        playerScore_ = 0;
        if(UpdateScore is not null) UpdateScore ();
    }


}
