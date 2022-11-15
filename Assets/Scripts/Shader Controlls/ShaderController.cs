using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShaderController : MonoBehaviour {
    
    public Renderer MatRenderer;

    public List<Material> sharedMats = new List<Material> ();

    private float currentDissolveValue = 0f;
    [Range(0f, 1f)]
    public float DesolveRange;
    // Start is called before the first frame update
    void Start() {
        GetAllMaterials ();
    }

    private void GetAllMaterials () {
        MatRenderer.GetMaterials (sharedMats);
    }

    public void ShowDeathShade () {
        // Debug.Log("2-->");
        StartCoroutine (DecreaseVisibility ());
    }

    IEnumerator DecreaseVisibility () {
        while (currentDissolveValue < 1f) {
            // Debug.Log("Calling increase desolve, current Value= "+currentDissolveValue+", time Value= "+(Time.deltaTime));
            currentDissolveValue += DesolveRange;
            for (int i = 0; i < sharedMats.Count; i++) {
                sharedMats[i].SetFloat ("_DissolveAmount", currentDissolveValue);
            }

            yield return new WaitForSeconds(0.1f);
        }

        currentDissolveValue = 1f;
        for (int i = 0; i < sharedMats.Count; i++) {
            sharedMats[i].SetFloat ("_DissolveAmount", currentDissolveValue);
        }
    }

    private void Update () {
        // if (Input.GetKeyDown (KeyCode.Space) && currentDissolveValue > 0f) {
        //     StopAllCoroutines();
        //     currentDissolveValue = 0f;
        //     for (int i = 0; i < sharedMats.Count; i++) {
        //         sharedMats[i].SetFloat ("_DissolveAmount", currentDissolveValue);
        //     }
        //     StartCoroutine (DecreaseVisibility ());
        // }
    }

    public void ResetShader () {
        currentDissolveValue = 0f;
        for (int i = 0; i < sharedMats.Count; i++) {
            sharedMats[i].SetFloat ("_DissolveAmount", currentDissolveValue);
        }
    }
}
