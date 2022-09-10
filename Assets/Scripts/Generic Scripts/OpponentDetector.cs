using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OpponentDetector : MonoBehaviour
{
    [SerializeField] private Transform parentTransform;
    private Dictionary<string, EnemyController> opponentListInAreaForEnemy = new Dictionary<string, EnemyController>();
    //private Dictionary<string, DefenceComponent> opponentListInAreaForPlayer = new Dictionary<string, DefenceComponent>();

    private void OnTriggerEnter(Collider other) {

        if(parentTransform.tag == "Enemy") {
            if (other.CompareTag("Tower")){

            }else if (other.CompareTag("Weapon")) {

            }


        }else if(parentTransform.tag == "Weapon") {

        }
    }

    public void OnTriggerExit(Collider other) {
        if (transform.tag == "Enemy") {
            RemoveOpponentFromListEnemy(other.transform.name);
        }else if(transform.tag == "Weapon") {
            RemoveOpponentFromListPlayer(other.transform.name);
        }
    }


    private void OnTriggerStay(Collider other) {
        if (parentTransform.tag == "Enemy") {
            if (other.CompareTag("Tower")) {
                // Tower is at range. Can hit now.
                // Stop movement of 
            } else if (other.CompareTag("Weapon")) {
                //Check if the enemy can deal damage to Weapons. if True=> Deal Damage to the Weapon
            }


        } else if (parentTransform.tag == "Weapon") {
            
        }
    }

    private void CheckAndAddOpponentToListEnemy(string opponentName, EnemyController eController) {
        if (!opponentListInAreaForEnemy.ContainsKey(opponentName)) {
            opponentListInAreaForEnemy.Add(opponentName, eController);
        }
    }

    private void RemoveOpponentFromListEnemy(string opponentName) {
        if (opponentListInAreaForEnemy.ContainsKey(opponentName)) {
            opponentListInAreaForEnemy.Remove(opponentName);
        }
    }

    private void CheckAndAddOpponentToListPlayer(string opponentName, Vector3 pos) {
        //if (!opponentListInAreaForPlayer.ContainsKey(opponentName)) {
        //    opponentListInArea.Add(opponentName, pos);
        //}
    }

    private void RemoveOpponentFromListPlayer(string opponentName) {
        //if (opponentListInArea.ContainsKey(opponentName)) {
        //    opponentListInArea.Remove(opponentName);
        //}
    }


}
