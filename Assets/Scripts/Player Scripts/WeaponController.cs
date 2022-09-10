using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform currentTargetTransform;
    public Weapon thisWeapon;
    public Transform RotateBody;
    private int CurrentHealth;
    public EnemyDetector detector;
    public Transform muzzleTransform;
    private float fireCountDown = 0f;
    private bool isActive;
    
    

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
            Shoot();
            fireCountDown = PlayerGamePlayController.Instance.DivideCount / thisWeapon.FireRate;
        }

        fireCountDown -= Time.deltaTime;
    }

    private void Shoot() {
        Bullet bullet = PlayerGamePlayController.Instance.GetBulletToShoot();
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if(bullet != null) {
            bullet.transform.position = muzzleTransform.position;
            bullet.SetTargetTransform(currentTargetTransform);
            //Vector3 direction = currentTargetTransform.position - bullet.transform.position;
            bullet.SetBulletDamage(thisWeapon.AttackDamage);
            bullet.gameObject.SetActive(true);
            //bulletRb.velocity = new Vector3(direction.x, direction.y, direction.z) * PlayerGamePlayController.Instance.bulletSpeed;
        }
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
