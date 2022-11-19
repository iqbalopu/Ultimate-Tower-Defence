using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveController : MonoBehaviour {
    public int WaitBeforeNextWave;
    public Enemy[] enemyPrefabs;
    public Transform enemyParent;
    public Transform startPoint;
    public Transform targetTransform;
    public int enemyPerWave;
    public float MaxXValue, MinXValue;
    private List<EnemyController> enemyPool =  new List<EnemyController>();
    public int poolCount;
    public float waitBeforeGameStart;
    [Range(0f, 5f)]public float IntervalInbetweenEnemies;

    private void Awake () {
        GameController.Instance.StartGamePlay += StartGame;
    }

    // Start is called before the first frame update
    void Start() {
        if(enemyPool.Count == 0) GenerateEnemyPool();
    }

    private void StartGame() {
        //PlayerGamePlayController.Instance.SetTurretUI();
        InvokeRepeating("Release", waitBeforeGameStart, WaitBeforeNextWave);

    }

    private void Release () {
        StartCoroutine (ReleaseWave ());
    }

    IEnumerator ReleaseWave () {
        int temp = 0;
        while (temp<enemyPerWave) {
            Debug.Log("Temp Value= "+temp);
            EnemyController eC = GetEnemy();
            if (eC == null) break;
            float tempRand = Random.Range (MinXValue, MaxXValue);
            Vector3 startPos = new Vector3(tempRand, startPoint.position.y, startPoint.position.z);
            eC.ResetEnemy();
            eC.SetTargetAndStartPosition(new Vector3(tempRand, targetTransform.position.y, targetTransform.position.z), startPos);
            // eC.InitializeDestination();
            eC.ToggleEnemy(true);
            eC.StartEnemyMovement();
            temp++;
            yield return new WaitForSeconds (IntervalInbetweenEnemies);
            
        }
    }


    private EnemyController GetEnemy() {
        if (enemyPool.Count == 0) return null;
        for (int i = 0; i < enemyPool.Count; i++) {
            if (enemyPool[i].gameObject.activeInHierarchy) continue;
            return enemyPool[i];
        }
        return null;
    }

    

    private void GenerateEnemyPool() {
        for (int i = 0; i < poolCount; i++) {
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            Enemy temp = enemyPrefabs[randomIndex];
            GameObject g = Instantiate(temp.enemyPrefab, enemyParent.position, enemyParent.rotation, enemyParent);
            EnemyController eCon = g.GetComponent<EnemyController>();
            eCon.ToggleEnemy(false);
            eCon.InitializeEnemyDetails(temp);
            enemyPool.Add(eCon);
        }
    }

    public void ResetWave() {
        CancelInvoke("ReleaseWave");
        for (int i = 0; i < enemyPool.Count; i++) {
            enemyPool[i].ResetEnemy();
        }
    }

    public void ResetGame () {
        CancelInvoke("Release");
    }
}
