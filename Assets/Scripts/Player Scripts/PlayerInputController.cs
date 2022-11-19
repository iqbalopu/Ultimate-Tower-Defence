using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private int BaseLayer;
    private PlayerController controller_;
    
    
    public Action BaseSelected;

    private void Awake () {
        controller_ = GetComponent<PlayerController> ();
        controller_.ProcessInput += ProcessInput;
    }

    private void Start () {
        BaseLayer = LayerMask.NameToLayer("BaseLayer");
    }
    
    private void ProcessInput () {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        if (hit) {
            // Debug.LogError("PlayerGamePlayController :: hit name= "+hitInfo.transform.name);
            if (hitInfo.transform.gameObject.layer == BaseLayer) {
                Base currBase = hitInfo.transform.GetComponent<Base>();
                // Debug.LogError("OPU :: hit object= "+hitInfo.transform.name);
                if (currBase is not null) {
                    if(BaseSelected is not null) BaseSelected ();
                    controller_.CheckAndShowWeaponsToDisplayForCurrBase(currBase);
                } //CheckAndShowWeaponsToDisplayForCurrBase(currBase);
            }
                
        }
    }
}
