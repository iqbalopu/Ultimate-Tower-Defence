using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyDetector : MonoBehaviour
{
    private Dictionary<EnemyController, int> enemyInRange = new Dictionary<EnemyController, int>();
    private float MaxDistanceForDetection;


    public void SetColliderRange(float radius) {
        this.transform.localScale = new Vector3(radius, radius, radius);
        MaxDistanceForDetection = radius + (radius * 0.5f) + (radius * 0.3f);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            EnemyController eC = other.gameObject.GetComponent<EnemyController>();
            if (!enemyInRange.ContainsKey(eC)) {
                //Debug.Log("ParentName= " + parentTransform.name + ",Adding enemy Name= " + eC.transform.name);
                enemyInRange.Add(eC, eC.GetID());
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy")) {
            EnemyController eC = other.gameObject.GetComponent<EnemyController>();
            if (enemyInRange.ContainsKey(eC)) {
                //Debug.Log("ParentName= " + parentTransform.name + ",Removing enemy Name= " + eC.transform.name);
                enemyInRange.Remove(eC);
            }
        }
    }


    public Transform GetUpdatedTarget() {
        Transform targetTransform = null;
        float shortestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        Dictionary<EnemyController, int> tempRange = enemyInRange;
        foreach (KeyValuePair<EnemyController, int> item in tempRange) {
            float distance = Vector3.Distance(this.transform.parent.position, item.Key.transform.position);
            //Debug.Log("Enemy Detector :: Parent Name= " + this.transform.parent.name + ", Distance= " + distance + ", EnemyName= " + item.Key.transform.name);
            if (distance > MaxDistanceForDetection) {
                continue;
            }
            if(distance < shortestDistance) {
                shortestDistance = distance;
                closestEnemy = item.Key.transform;
            }

            if(closestEnemy != null) {
                targetTransform = closestEnemy;
            } else {
                targetTransform = null;
            }
        }

        //targetTransform = targetTransform.GetComponent<EnemyController>().CenterTransform;

        //if(targetTransform != null) {
        //    targetTransform = targetTransform.GetComponent<EnemyController>().CenterTransform;
        //}

        //if (targetTransform != null) Debug.Log("Enemy Detector :: return Transform name= " + targetTransform.name);
        return targetTransform;
    }
}
