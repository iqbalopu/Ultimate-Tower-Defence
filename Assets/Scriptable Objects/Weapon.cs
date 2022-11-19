using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "weapon_", menuName = "ScriptableObjects/Weapon", order = 2)]
public class Weapon : ScriptableObject {

    public enum DamageType {
        NONE,
        SINGLE,
        AREA
    }

    public string Name;
    public int Level;
    public int Health;
    public float FireRate;
    public int AttackDamage;
    public int WeaponID;
    public float RangeRadius;
    public GameObject weaponPrefab;
    public DamageType damageType = DamageType.NONE;
    public Sprite turretImage;
    public int WeaponCost;
    public GameObject BulletPrefab;
    public GameObject BlastParticle;
    public TrailRenderer TrailObject;

}
