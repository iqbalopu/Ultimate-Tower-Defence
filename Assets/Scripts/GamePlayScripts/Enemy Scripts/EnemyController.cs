using System;
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
    public Animator enemyAnimator;
    private Rigidbody enemyRB;
    public Image healthBar;
    public Transform CenterTransform;
    public ShaderController orcShader;
    private bool isDead = false;
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
        navAgent = GetComponent<NavMeshAgent>();
        if(enemyObject != null) InitializeEnemyDetails(enemyObject);
    }

    public void StartEnemyMovement() {
        enemyRB.isKinematic = false;
        navAgent.enabled = true;
        navAgent.speed = enemyObject.MoveSpeed;
        this.Run();
        navAgent.SetDestination(targetPosition);
        currentStatus = EnemyStatus.MOVING;
        
        // this.Walk();
    }

    public bool IsEnemyDead () {
        return isDead;
    }

    public void SetTargetAndStartPosition(Vector3 target, Vector3 startPos) {
        // Debug.LogError("OPU :: SetTargetAndStartPosition :: current pos= "+this.transform.position+", startpos= "+startPos+", curr local pos= "+transform.localPosition);
        targetPosition = target;
        this.transform.position = startPos;
        // Debug.LogError("OPU :: SetTargetAndStartPosition :: After setting :: current pos= "+this.transform.position+", startpos= "+startPos+", curr local pos= "+transform.localPosition);
    }
    

    public void HoldEnemyMovement() {
        navAgent.isStopped = true;
        this.Idle();
        currentStatus = EnemyStatus.HOLD_ATTACKING;
    }

    public void StartMovementAfterAttacking() {
        navAgent.isStopped = false;
        this.
        currentStatus = EnemyStatus.MOVING;
    }

    public void InitializeDestination () {
        navAgent.SetDestination (targetPosition);
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
        if(isDead) return;
        StopCoroutine(ReduceHealthBar());
        this.SetHealthbarValue(Utills.ConvertRange(0f, enemyObject.Health, 0f, 1f, CurrentHealth));
        float remainingHealth = CurrentHealth - (float)damage;
        float previousHealth = CurrentHealth;
        if (remainingHealth <= 0) {
            healthBar.enabled = false;
            this.SetHealthbarValue(1f);
            CurrentHealth = 0;
            // enemyObject.SetEnemyDead(true);
            isDead = true;
            this.Die();
            // navAgent.enabled = false;
            // navAgent.isStopped = true;
            navAgent.speed = 0f;
            PlayerController.Instance.GetPlayerScore().IncreaseScore(enemyObject.killRewardScore);
            PlayerController.Instance.GetPlayerData().IncreaseGemCount(enemyObject.killRewardGem);
            // Debug.Log("1-->");
            StartCoroutine(EnemyDead());
            return;
        } else {
            CurrentHealth = remainingHealth;
        }
        StartCoroutine(ReduceHealthBar());
    }

    IEnumerator EnemyDead() {
        if(orcShader is not null) orcShader.ShowDeathShade();
        yield return new WaitForSeconds(1.3f);
        // Debug.Log("3-->");
        SetEnemyDied();
    }

     IEnumerator ReduceHealthBar() {
        float currFillAmount = healthBar.fillAmount;
        float updatedFillAmount = Utills.ConvertRange(0f, enemyObject.Health, 0f, 1f, CurrentHealth);
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
        healthBar.enabled = true;
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
        enemyRB.isKinematic = true;
        navAgent.enabled = false;
        CurrentHealth = enemyObject.Health;
        // enemyObject.SetEnemyDead(false);
        isDead = false;
    }

    private void SetEnemyDied() {
        //Let The GameController know that this enemy has Died.
        //We need to move play enemy Death animation and Add reward to Players Wallet.
        this.currentStatus = EnemyStatus.DEAD;
        ToggleEnemy(false);
        if(orcShader is not null) orcShader.ResetShader();
        // Debug.Log("4-->");
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
            BlastParticle ps = PlayerController.Instance.GetPlayerPool().GetObstacleHitParticle();
            ps.PlayParticle(b.transform);
            b.ResetBullet();
            this.DecreaseHealth(b.GetDamage());
        }

        if (other.CompareTag("Tower")) {
            // navAgent.speed = 0f;
            this.Attack();
        }
    }

    private void OnTriggerStay (Collider other) {
        if (other.CompareTag("Tower")) {
            other.GetComponent<TownHall>().DecreaseTowerHealth(enemyObject.AttackDamage);
        }
    }

    public void Walk() {
        enemyAnimator.SetBool("Walk Forward", true);
        enemyAnimator.SetBool("Run Forward", false);
    }

    public void Idle() {
        enemyAnimator.SetBool("Attack", false);
        enemyAnimator.SetBool("Run Forward", false);
    }

    public void Run() {
        enemyAnimator.SetBool("Attack", false);
        enemyAnimator.SetBool("Run Forward", true);
    }

    public void TakeDamage() {
        enemyAnimator.SetTrigger("Take Damage");
    }
    
    public void Attack() {
        enemyAnimator.SetBool("Run Forward", false);
        enemyAnimator.SetBool("Attack", true);
    }

    public void Die() {
        // enemyAnimator.SetBool("Walk Forward", false);
        // enemyAnimator.SetBool("Run Forward", false);
        // enemyAnimator.SetTrigger("Die");
        this.Idle();
    }

    public int GetID() {
        if (enemyObject == null) return -1;
        return enemyObject.EnemyId;
    }

}
