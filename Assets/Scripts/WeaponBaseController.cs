using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBaseController : MonoBehaviour
{
    public static WeaponBaseController Instance;
    private List<Base> weaponBases;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
    }

    private void Start() {
        GetAllBasePoints();
    }
    private void GetAllBasePoints() {
        weaponBases = new List<Base>();
        Base[] bases = this.gameObject.GetComponentsInChildren<Base>();
        weaponBases = new List<Base>(bases);
    }

    public List<Vector3> GetAllWeaponBasePositions() {
        if (weaponBases.Count == 0) return new List<Vector3>();
        List<Vector3> basePositions = new List<Vector3>();
        for (int i = 0; i < weaponBases.Count; i++) {
            basePositions.Add(weaponBases[i].GetTurretPosFromBase());
        }
        return basePositions;
    }

    public List<Transform> GetAllWeaponBaseTransform() {
        if (weaponBases.Count == 0) return new List<Transform>();
        List<Transform> baseTransforms = new List<Transform>();
        for (int i = 0; i < weaponBases.Count; i++) {
            baseTransforms.Add(weaponBases[i].GetBaseTransform());
        }
        return baseTransforms;
    }

}
