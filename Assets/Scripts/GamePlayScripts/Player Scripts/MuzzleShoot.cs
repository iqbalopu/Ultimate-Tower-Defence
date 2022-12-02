using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleShoot : MonoBehaviour {
    [SerializeField] private GameObject MuzzleFlash;
    [SerializeField] private WeaponController thisController_;
    private void Awake () {
        thisController_.GetWeaponAttack().Shootbullet += Shootbullet;
    }

    
    
    private void Shootbullet() {
        if (GameController.Instance.CurrentShootStyle == GameController.ShootStyle.OBJECT_POOLING) {
            Bullet bullet = thisController_.GetWeaponPool().GetBulletToShoot();
            if(bullet != null) {
                CancelInvoke("HideFlash");
                HideFlash();
                bullet.transform.position = this.transform.position;
                bullet.SetTargetTransform(null);
                Vector3 directionTemp = thisController_.GetWeaponArea().GetCurrentTargetTransform().position - bullet.transform.position;
                directionTemp.Normalize();
                bullet.SetBulletDamage(thisController_.GetThisWeaponData().AttackDamage);
                bullet.gameObject.SetActive(true);
                bullet.ShootSelf(directionTemp);
                MuzzleFlash.SetActive(true);
                Invoke("HideFlash", 0.2f);
            }
        }else if (GameController.Instance.CurrentShootStyle == GameController.ShootStyle.RAYCAST) {
            //Another way- the raycastWay
            Vector3 direction = thisController_.GetWeaponArea().GetCurrentTargetTransform().position - this.transform.parent.position;
            if (Physics.Raycast (this.transform.position, direction , out RaycastHit hit, float.MaxValue)) {
                // Debug.Log("Hitting Enemy "+hit.transform.tag);
                if (hit.transform.CompareTag ("Enemy")) {
                    if (!hit.transform.GetComponent<EnemyController> ().IsEnemyDead ()) {
                        TrailRenderer trail = thisController_.GetWeaponPool().GetTrailForShoot();
                        trail.transform.position = this.transform.position;
                        trail.transform.rotation = Quaternion.identity;
                        MuzzleFlash.SetActive(true);
                        Invoke("HideFlash", 0.2f);
                        StartCoroutine (SpawnTrail (trail, hit));
                    }
                }
            }
        }
    }
    
    IEnumerator SpawnTrail (TrailRenderer trail, RaycastHit hit) {
        trail.gameObject.SetActive(true);
        float time = 0;
        Vector3 startPos = trail.transform.position;
        while (time < 1) {
            trail.transform.position = Vector3.Lerp(startPos,hit.point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }
    
        trail.transform.position = hit.point;
        DoDamage (hit);
        trail.gameObject.SetActive(false);
    }

    private void DoDamage (RaycastHit hit) {
        EnemyController ec =  hit.transform.GetComponent<EnemyController>();
        if (ec != null) {
            ec.DecreaseHealth (thisController_.GetThisWeaponData().AttackDamage);
            BlastParticle bp = thisController_.GetWeaponPool().GetParticleToBlast();
            bp.PlayParticle(hit.point);
        }
    }
    
    private void HideFlash() {
        MuzzleFlash.SetActive(false);
    }
}
