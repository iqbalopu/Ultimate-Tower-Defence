using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class EnemyController : MonoBehaviour {
    private NavMeshAgent navAgent;
    private Vector3 targetPosition;
    private Enemy enemyObject;
    private Animator enemyAnimator;
    private Rigidbody enemyRB;
    public Image healthBar;
    public Transform CenterTransform;
    public enum EnemyStatus {
        IDLE,
        MOVING,
        HOLD_ATTACKING,
        DEAD
    }

    public EnemyStatus currentStatus = EnemyStatus.IDLE;
    private float CurrentHealth;

    //Walk Forward
    //Run Forward
    //Jump
    //Attack 01
    //Attack 02
    //Take Damage
    //Die


    private void Awake() {
        enemyRB = GetComponent<Rigidbody>();
        enemyAnimator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        if(enemyObject != null) InitializeEnemyDetails(enemyObject);
    }

    public void StartEnemyMovement() {
        enemyRB.isKinematic = false;
        navAgent.enabled = true;
        navAgent.speed = enemyObject.MoveSpeed;
        navAgent.SetDestination(targetPosition);
        currentStatus = EnemyStatus.MOVING;
        this.Walk();
    }

    public float ConvertRange(float RangeACurrentValue, float RangeAMax, float RangeAMin, float RangeBMax, float RangeBMin) {
        return (((RangeACurrentValue - RangeAMin) * (RangeBMax - RangeBMin)) / (RangeAMax - RangeAMin)) + RangeBMin;
    }

    public void SetTargetAndStartPosition(Vector3 target, Vector3 startPos) {
        targetPosition = target;
        this.transform.position = startPos;
    }
    

    public void HoldEnemyMovement() {
        navAgent.isStopped = true;
        this.Idle();
        currentStatus = EnemyStatus.HOLD_ATTACKING;
    }

    public void StartMovementAfterAttacking() {
        navAgent.isStopped = false;
        this.Walk();
        currentStatus = EnemyStatus.MOVING;
    }

    public void InitializeEnemyDetails(Enemy eComponent) {
        CurrentHealth = eComponent.Health;
        enemyRB.isKinematic = true;
        navAgent.enabled = false;
        navAgent.speed = eComponent.MoveSpeed;
        enemyObject = eComponent;
        this.SetHealthbarValue(1f);
    }

    private void SetHealthbarValue(float fillValue) {
        healthBar.fillAmount = fillValue;
        healthBar.color = enemyObject.healthBarGradient.Evaluate(healthBar.fillAmount);
    }

    public void DecreaseHealth(int damage) {
        StopCoroutine(ReduceHealthBar());
        this.SetHealthbarValue(this.ConvertRange(CurrentHealth, enemyObject.Health, 0f, 1f, 0f));
        float remainingHealth = CurrentHealth - (float)damage;
        float previousHealth = CurrentHealth;
        if (remainingHealth <= 0) {
            healthBar.enabled = false;
            this.SetHealthbarValue(1f);
            CurrentHealth = 0;
            this.Die();
            enemyObject.SetStatus(true);
            navAgent.isStopped = true;
            PlayerGamePlayController.Instance.IncreaseScore(enemyObject.killRewardScore, enemyObject.killRewardGem);
            StartCoroutine(EnemyDead());
            return;
        } else {
            CurrentHealth = remainingHealth;
        }
        StartCoroutine(ReduceHealthBar());
    }

    IEnumerator EnemyDead() {
        yield return new WaitForSeconds(1.5f);
        EnemyDied();
    }

     IEnumerator ReduceHealthBar() {
        float currFillAmount = healthBar.fillAmount;
        float updatedFillAmount = this.ConvertRange(CurrentHealth, enemyObject.Health, 0f, 1f, 0f);
        float difference = updatedFillAmount - currFillAmount;
        while(currFillAmount < updatedFillAmount) {
            yield return new WaitForEndOfFrame();
            difference -= Time.deltaTime;
            currFillAmount += Time.deltaTime;
            this.SetHealthbarValue(currFillAmount);
        }

        this.SetHealthbarValue(updatedFillAmount);
    }

    public void ResetEnemy() {
        this.currentStatus = EnemyStatus.IDLE;
        ToggleEnemy(false);
        
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
        enemyRB.isKinematic = true;
        navAgent.enabled = false;
    }

    private void EnemyDied() {
        //Let The GameController know that this enemy has Died.
        //We need to move play enemy Death animation and Add reward to Players Wallet.
        this.currentStatus = EnemyStatus.DEAD;
        
        ToggleEnemy(false);
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
    }

    public void ToggleEnemy(bool value) {
        this.gameObject.SetActive(value);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Bullet")) {
            //Decrease health of the enemy{
            // Get Damage from BulletComponent and minus the damage from current health
            Bullet b = other.gameObject.GetComponent<Bullet>();
            BlastParticle ps = PlayerGamePlayController.Instance.GetParticleToBlast();
            ps.PlayParticle(b.transform);
            b.ResetBullet();
            this.DecreaseHealth(b.GetDamage());
        }

        if (other.CompareTag("Tower")) {
            this.currentStatus = EnemyStatus.IDLE;
            ToggleEnemy(false);
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
            CurrentHealth = enemyObject.Health;
            this.SetHealthbarValue(1f);
        }
    }

    public void Walk() {
        enemyAnimator.SetBool("Walk Forward", true);
        enemyAnimator.SetBool("Run Forward", false);
    }

    public void Idle() {
        enemyAnimator.SetBool("Walk Forward", false);
        enemyAnimator.SetBool("Run Forward", false);
    }

    public void Run() {
        enemyAnimator.SetBool("Walk Forward", false);
        enemyAnimator.SetBool("Run Forward", true);
    }

    public void TakeDamage() {
        enemyAnimator.SetTrigger("Take Damage");
    }

    public void Die() {
        enemyAnimator.SetBool("Walk Forward", false);
        enemyAnimator.SetBool("Run Forward", false);
        enemyAnimator.SetTrigger("Die");
    }

    public int GetID() {
        if (enemyObject == null) return -1;
        return enemyObject.EnemyId;
    }

}
