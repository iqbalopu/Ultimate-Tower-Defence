using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPool : MonoBehaviour {
    
    private WeaponController thisController_;
    private List<Bullet> bullets = new List<Bullet>();
    private List<TrailRenderer> trailPool_ = new List<TrailRenderer> ();
    private List<BlastParticle> blastParticles = new List<BlastParticle> ();
    private GameObject blastParticle;
    private GameObject bulletPrefab;
    private TrailRenderer bulletTrail;
    
    [SerializeField] private Transform bulletParentTransform;
    [SerializeField] private Transform particleparent;
    [SerializeField] private Transform trailParent;
    
    public int MaxPoolCount;
    
    private void Start () {
        thisController_ = GetComponent<WeaponController> ();
        SetPrefabInstances ();
        GenerateTrailPool ();
        GenerateBulletPool ();
        GenerateBlastParticlePool ();
    }

    private void SetPrefabInstances () {
        blastParticle = thisController_.GetThisWeaponData ().BlastParticle;
        bulletPrefab = thisController_.GetThisWeaponData ().BulletPrefab;
        bulletTrail = thisController_.GetThisWeaponData ().TrailObject;
    }

    private void GenerateTrailPool() {
        for (int i = 0; i < MaxPoolCount; i++) {
            TrailRenderer trail = Instantiate(bulletTrail, transform.position, Quaternion.identity, transform);
            trail.gameObject.SetActive(false);
            trailPool_.Add(trail);
        }
    }

    public TrailRenderer GetTrailForShoot () {
        for (int i = 0; i < trailPool_.Count; i++) {
            if (!trailPool_[i].gameObject.activeInHierarchy) return trailPool_[i];
        }
        return null;
    }
    
    private void GenerateBulletPool() {
        if (bulletPrefab is not null) {
            for (int i = 0; i < MaxPoolCount; i++) {
                GameObject g = Instantiate(bulletPrefab, bulletParentTransform.position, Quaternion.identity, transform);
                g.SetActive(false);
                bullets.Add(g.GetComponent<Bullet>());
            }
        }
    }
    
    public Bullet GetBulletToShoot() {
        for (int i = 0; i < bullets.Count; i++) {
            if (!bullets[i].gameObject.activeInHierarchy) return bullets[i];
        }
        return null;
    }
    
    private void GenerateBlastParticlePool() {
        for (int i = 0; i < MaxPoolCount; i++) {
            GameObject g = Instantiate(blastParticle, particleparent.position, Quaternion.identity, transform);
            BlastParticle bp = g.GetComponent<BlastParticle>();
            g.SetActive(false);
            blastParticles.Add(bp);
        }
    }
    
    public BlastParticle GetParticleToBlast() {
        for (int i = 0; i < blastParticles.Count; i++) {
            if (!blastParticles[i].gameObject.activeInHierarchy) return blastParticles[i];
        }
        return null;
    }
}
