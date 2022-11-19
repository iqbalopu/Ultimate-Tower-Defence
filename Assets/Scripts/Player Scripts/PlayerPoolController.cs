using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoolController : MonoBehaviour
{
    public int poolCount;
    public float particleTimeOut;
    
    [SerializeField] private GameObject obsParticlePrefab;
    [SerializeField] private Transform particleparent;
    
    private List<BlastParticle> obsParticles = new List<BlastParticle>();

    private void Start () {
        GenerateObsBlastParticlePool();
    }

    private void GenerateObsBlastParticlePool() {
        for (int i = 0; i < poolCount; i++) {
            GameObject g = Instantiate(obsParticlePrefab, particleparent.position, Quaternion.identity, particleparent);
            BlastParticle bp = g.GetComponent<BlastParticle>();
            g.SetActive(false);
            obsParticles.Add(bp);
        }
    }
    
    public BlastParticle GetObstacleHitParticle() {
        for (int i = 0; i < obsParticles.Count; i++) {
            if (!obsParticles[i].gameObject.activeInHierarchy) return obsParticles[i];
        }
        return null;
    }
}
