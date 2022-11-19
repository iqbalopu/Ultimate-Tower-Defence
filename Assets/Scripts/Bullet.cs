using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody bulletRB;
    private int Damage;
    private Transform target;

    public void SetTargetTransform(Transform _target) {
        target = _target;
    }

    public void ResetBullet() {
        target = null;
        this.gameObject.SetActive(false);
    }

    public void SetBulletDamage(int dmg) {
        Damage = dmg;
    }

    public int GetDamage() {
        return Damage;
    }

    //private void Update() {
    //    if(target == null) {
    //        this.ResetBullet();
    //        return;
    //    }

    //    //Vector3 distance = Vector3.Distance()
    //    Vector3 tempTargetPos = new Vector3(target.position.x, target.position.y + 0.3f, target.position.z);
    //    Vector3 dir = tempTargetPos - transform.position;
    //    float distanceThisFrame = PlayerGamePlayController.Instance.bulletSpeed * Time.deltaTime;

    //    if (dir.magnitude <= distanceThisFrame) {
    //        HitTarget();
    //        return;
    //    }

    //    transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    //    transform.LookAt(target);
    //}

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Path") || other.CompareTag("Obstacle")) {
            BlastParticle bp = PlayerController.Instance.GetPlayerPool().GetObstacleHitParticle();
            bp.PlayParticle(this.transform);
            this.ResetBullet();
        }
    }

    public void ShootSelf(Vector3 direction) {
        bulletRB.velocity = new Vector3(direction.x, direction.y, direction.z) * PlayerController.Instance.bulletSpeed;
    }

    public void HitTarget() {
        this.ResetBullet();
    }
}
