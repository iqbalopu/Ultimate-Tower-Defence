using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start() {
        if(enemyPool.Count == 0) GenerateEnemyPool();
    }

    public void StartGame() {
        //PlayerGamePlayController.Instance.SetTurretUI();
        PlayerGamePlayController.Instance.ToggleScoreCard(true);
        PlayerGamePlayController.Instance.ToggleGemObject(true);
        InvokeRepeating("ReleaseWave", waitBeforeGameStart, WaitBeforeNextWave);

    }

    private void ReleaseWave() {
        for (int i = 0; i < enemyPerWave; i++) {
            EnemyController eC = GetEnemy();
            if (eC == null) break;
            float rand = Random.Range(MinXValue, MaxXValue);
            Vector3 startPos = new Vector3(rand, startPoint.position.y, startPoint.position.z);
            float randXpos = Random.Range(MinXValue, MaxXValue);
            eC.SetTargetAndStartPosition(new Vector3(randXpos, targetTransform.position.y, targetTransform.position.z), startPos);
            eC.ToggleEnemy(true);
            eC.StartEnemyMovement();
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
            GameObject g = Instantiate(temp.enemyPrefab, enemyParent.position, Quaternion.identity, enemyParent);
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
}
