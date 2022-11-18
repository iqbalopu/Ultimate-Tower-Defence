using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private Transform currentTargetTransform;
    public Weapon thisWeapon;
    public EnemyDetector detector;
    [SerializeField] private Transform RotateBody;
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private GameObject MuzzleFlash;
    [SerializeField] private LayerMask Mask;
    [SerializeField] private TrailRenderer bulletTrail;
    private float fireCountDown = 0f;
    private bool isActive;
    private int CurrentHealth;

    private void Awake() {
        //if (thisWeapon != null) InitializeWeapon(thisWeapon);
        //detector = this.transform.GetComponentInChildren<EnemyDetector>();
    }

    public void InitializeWeapon(Weapon weapon) {
        CurrentHealth = weapon.Health;
        thisWeapon = weapon;
        if (detector != null) detector.SetColliderRange(this.thisWeapon.RangeRadius);
        isActive = true;
        StartCheckingForEnemy();
    }

    private void StartCheckingForEnemy() {
        InvokeRepeating("UpdateNearestEnemy", 0.1f, Time.deltaTime);
    }

    private void UpdateNearestEnemy() {
        currentTargetTransform = detector.GetUpdatedTarget();
        //Debug.Log("Turrett Name= " + this.thisWeapon.Name + ", currentTargetTransform after Update= " + currentTargetTransform);
    }

    public void ResetTurret() {
        CancelInvoke("UpdateNearestEnemy");
        isActive = true;
        CurrentHealth = thisWeapon.Health;
    }

    // Update is called once per frame
    void Update() {
        if(!GameController.Instance.IsGameRunning()) return;
        if (!isActive) return;
        if (currentTargetTransform == null) {
            return;
        }

        Vector3 dir = currentTargetTransform.position - RotateBody.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(RotateBody.rotation, lookRotation, Time.deltaTime*GameController.Instance.turretSmoothFactor).eulerAngles;
        RotateBody.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        //Debug.Log("Rotating Rotate body of " + thisWeapon.Name);
        if(fireCountDown <= 0) {
            Shoot(dir);
            fireCountDown = PlayerGamePlayController.Instance.DivideCount / thisWeapon.FireRate;
        }

        fireCountDown -= Time.deltaTime;
    }

    private void Shoot(Vector3 direction) {
        if (GameController.Instance.CurrentShootStyle == GameController.ShootStyle.OBJECT_POOLING) {
            Bullet bullet = PlayerGamePlayController.Instance.GetBulletToShoot();
            if(bullet != null) {
                CancelInvoke("HideFlash");
                HideFlash();
                bullet.transform.position = muzzleTransform.position;
                bullet.SetTargetTransform(null);
                Vector3 directionTemp = currentTargetTransform.position - bullet.transform.position;
                directionTemp.Normalize();
                bullet.SetBulletDamage(thisWeapon.AttackDamage);
                bullet.gameObject.SetActive(true);
                bullet.ShootSelf(directionTemp);
                MuzzleFlash.SetActive(true);
                Invoke("HideFlash", 0.2f);
                //bulletRb.velocity = new Vector3(direction.x, direction.y, direction.z) * PlayerGamePlayController.Instance.bulletSpeed;
            }
        }else if (GameController.Instance.CurrentShootStyle == GameController.ShootStyle.RAYCAST) {
            //Another way- the raycastWay
            if (Physics.Raycast (muzzleTransform.position, direction, out RaycastHit hit, float.MaxValue)) {
                // Debug.Log("Hitting Enemy "+hit.transform.tag);
                if (hit.transform.CompareTag ("Enemy")) {
                    if (!hit.transform.GetComponent<EnemyController> ().IsEnemyDead ()) {
                        TrailRenderer trail = Instantiate (bulletTrail, muzzleTransform.position, Quaternion.identity);
                        MuzzleFlash.SetActive(true);
                        Invoke("HideFlash", 0.2f);
                        StartCoroutine (SpawnTrail (trail, hit));
                    }
                }
            }
        }
    }

    private Vector3 GetDirection () {
        return muzzleTransform.forward;
    }

    IEnumerator SpawnTrail (TrailRenderer trail, RaycastHit hit) {
        float time = 0;
        Vector3 startPos = trail.transform.position;
        while (time < 1) {
            trail.transform.position = Vector3.Lerp(startPos,hit.point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }

        trail.transform.position = hit.point;
        EnemyController ec =  hit.transform.GetComponent<EnemyController>();
        if (ec != null) {
            ec.DecreaseHealth (thisWeapon.AttackDamage);
            BlastParticle bp = PlayerGamePlayController.Instance.GetParticleToBlast();
            bp.PlayParticle(hit.point);
        }
        Destroy(trail.gameObject);
    }

    private void HideFlash() {
        MuzzleFlash.SetActive(false);
    }

    public void SetPositionofWeapon(Transform pos) {
        this.transform.position = pos.position;
        this.transform.rotation = pos.rotation;
    }

    public int GetWeaponSellCost() {
        return thisWeapon.WeaponCost;
    }

    public Sprite GetShopSprite() {
        return thisWeapon.turretImage;
    }

}
