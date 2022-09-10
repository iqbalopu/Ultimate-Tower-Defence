using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHandler : MonoBehaviour
{

    public enum EntityType {
        UNKNOWN,
        WEAPON,
        ENEMY
    }

    public EntityType ObjectEntityType =  EntityType.UNKNOWN;

    public void ToggleEntityPresense(bool shouldBePresentInScene) {
        this.gameObject.SetActive(shouldBePresentInScene);
    }
}
