using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "enemy_", menuName = "ScriptableObjects/Enemy", order = 1)]
public class Enemy : ScriptableObject
{
    public enum WeaponType {
        SINGLE,
        AREA
    }
    public enum AttackType {
        MEELE,
        RANGED
    }

    #region Enemy Attributes
    public string Name;
    public int Level;
    public float MoveSpeed;
    public float Health;
    public int AttackDamage;
    public WeaponType EnemyWeaponType;
    public AttackType attackType;
    public int killRewardScore;
    public int killRewardGem;
    private bool IsDead = false;
    public int EnemyId;
    public float AttackRate;
    public GameObject enemyPrefab;
    public Gradient healthBarGradient;
    #endregion

    #region Skin Card
    public Image enemyCardImage;
    #endregion


    #region Functions
    //write neccessary functions here for this scriptable Object

    public void SetEnemyDead(bool value) {
        IsDead = value;
    }

    public bool IsEnemyDead () {
        return IsDead;
    }
    #endregion
}
