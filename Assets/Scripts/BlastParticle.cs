using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastParticle : MonoBehaviour
{
    public ParticleSystem thisParticle;

    public void PlayParticle(Transform trans) {
        this.transform.position = trans.position;
        this.gameObject.SetActive(true);
        thisParticle.Play();
        Invoke("ResetParticle", PlayerGamePlayController.Instance.particleTimeOut);
    }

    private void ResetParticle() {
        this.gameObject.SetActive(false);
        this.transform.localPosition = Vector3.zero;
    }
}
