using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerGamePlayController : MonoBehaviour
{
    public static PlayerGamePlayController Instance;
    [Header("Attributes")]
    public int poolCount;
    public float bulletSpeed;
    public float DivideCount;
    public float particleTimeOut;
    public int playerScore = 0;
    public float MaxDistanceFromEnemy;
    public int GemCount;

    [Header("Assignable Unity Component")]
    public Transform bulletParentTransform;
    public Transform turretParent;
    public GameObject bulletPrefab_;
    public GameObject blastParticle;
    public Transform particleparent;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gemText;
    public GameObject gemObject;
    public GameObject scoreObject;
    public Animator scoreAnim;
    public Transform WeaponUIParent;
    public GameObject WeaponActionUI;
    //public GameObject weaponUiPrefab;
    public RectTransform BG;
    private Vector3 currentSelectedBasePos;
    private Base currSelectedBase;

    private List<Bullet> bullets = new List<Bullet>();
    private List<WeaponController> playerTurrets = new List<WeaponController>();
    private List<BlastParticle> blastParticles = new List<BlastParticle>();
    private int BaseLayer;
    private int WeaponLayer;
    private bool IsShowingBaseHighlight;
    
    



    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
    }

    private void Start() {
        ToggleWeaponUI(false);
        ToggleWeaponActions(false);
        GetAllTurrets();
        this.ToggleScoreCard(false);
        this.ToggleGemObject(false);
        this.ResetPlayer();
        SetGemText(""+GemCount);
        if (bullets.Count == 0) GenerateBulletPool();
        if(blastParticles.Count == 0) GenerateParticlePool();
        //SetWeaponsInPosition();
        BaseLayer = LayerMask.NameToLayer("BaseLayer");
        WeaponLayer = LayerMask.NameToLayer("WeaponLayer");
    }

    private void Update() {
        if (!GameController.Instance.IsGameRunning() || IsShowingBaseHighlight) return;

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit) {
                if (hitInfo.transform.gameObject.layer == BaseLayer) {
                    Base currBase = hitInfo.transform.GetComponent<Base>();
                    CheckAndShowWeaponsToDisplayForCurrBase(currBase);
                    //turretParent.position = hitInfo.transform.GetComponent<Base>().GetTurretPosFromBase();
                }
            }
        }
    }



    private void ToggleWeaponUI(bool value) {
        WeaponUIParent.gameObject.SetActive(value);
    }

    private void ToggleWeaponActions(bool value) {
        WeaponActionUI.SetActive(value);
    }

    private void CheckAndShowWeaponsToDisplayForCurrBase(Base selectedBase) {
        currSelectedBase = selectedBase;
        currentSelectedBasePos = currSelectedBase.GetTurretPosFromBase();
        if (selectedBase.CanUseBase()) {
            //Show the UI for selecting weapon
            ToggleWeaponUI(true);
        } else {
            //Show the UI for weapon action
            ToggleWeaponActions(true);
        }
    }

    public void LevelUpSelectedWeapon() {
        //here will be the code to levelup selected weapon

        this.ToggleWeaponActions(false);
    }

    public void SellSelectedWeapon() {
        Weapon wp = currSelectedBase.GetCurrentAssignedWeapon();
        WeaponController wc = currSelectedBase.GetCurrentAssignedWeaponController();
        if (wp != null && wc != null) {
            GemCount += wp.WeaponCost;
            SetGemText("" + GemCount);
            Destroy(wc.gameObject);
            currSelectedBase.SetCurrentAssignedWeapon(null);
            currSelectedBase.SetCurrentAssignedWeaponController(null);
            currSelectedBase.SetBaseOccupency(false);
        }
        this.ToggleWeaponActions(false);
    }

    public void OnWeaponSelected(Weapon selectedWeapon) {
        //Get the Weapon prefab using weaponIndex and Set the Weapon
        if(GemCount < selectedWeapon.WeaponCost) {
            Debug.LogError("Dont have enough Balance");
        } else {
            GameObject g = Instantiate(selectedWeapon.weaponPrefab, currentSelectedBasePos, currSelectedBase.GetBaseTransform().rotation);
            WeaponController wc = g.GetComponent<WeaponController>();
            wc.InitializeWeapon(selectedWeapon);
            currSelectedBase.SetCurrentAssignedWeapon(selectedWeapon);
            currSelectedBase.SetCurrentAssignedWeaponController(wc);
            currSelectedBase.SetBaseOccupency(true);
            GemCount -= selectedWeapon.WeaponCost;
            SetGemText("" + GemCount);
        }
        
        ToggleWeaponUI(false);
    }

    public void AddGemForKill(int rewardGem) {
        GemCount += rewardGem;
        SetGemText(""+GemCount);
    }

    private void SetGemText(string s) {
        gemText.text = s;
    }

    public Vector3 GetCurrentBasePos() {//Check for null reff where we call this function
        return currentSelectedBasePos;
    }

    //public void SetTurretUI() {
    //    int turretCount = playerTurrets.Count;
    //    for (int i = 0; i < turretCount; i++) {
    //        GameObject g = Instantiate(weaponUiPrefab, WeaponUIParent);
    //        g.GetComponent<Image>().sprite = playerTurrets[i].GetShopSprite();
    //    }
    //}

    private void GenerateBulletPool() {
        for (int i = 0; i < poolCount; i++) {
            GameObject g = Instantiate(bulletPrefab_, bulletParentTransform.position, Quaternion.identity, bulletParentTransform);
            g.SetActive(false);
            bullets.Add(g.GetComponent<Bullet>());
        }
    }

    private void GenerateParticlePool() {
        for (int i = 0; i < poolCount; i++) {
            GameObject g = Instantiate(blastParticle, particleparent.position, Quaternion.identity, particleparent);
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


    public Bullet GetBulletToShoot() {
        for (int i = 0; i < bullets.Count; i++) {
            if (!bullets[i].gameObject.activeInHierarchy) return bullets[i];
        }
        return null;
    }

    private void GetAllTurrets() {
        WeaponController[] turrets = turretParent.GetComponentsInChildren<WeaponController>();
        for (int i = 0; i < turrets.Length; i++) {
            playerTurrets.Add(turrets[i]);
        }
    }

    //public void EnableTurrets() {
    //    if (playerTurrets.Count == 0) return;
    //    for (int i = 0; i < playerTurrets.Count; i++) {
    //        playerTurrets[i].StartCheckingForEnemy();
    //    }
    //}

    public void ResetTurrets() {
        if (playerTurrets.Count == 0) return;
        for (int i = 0; i < playerTurrets.Count; i++) {
            playerTurrets[i].ResetTurret();
        }
    }

    public void ToggleScoreCard(bool value) {
        scoreObject.SetActive(value);
    }
    public void ToggleGemObject(bool value) {
        gemObject.SetActive(value);
    }

    public void IncreaseScore(int reward, int rewardGem) {
        playerScore += reward;
        GemCount += rewardGem;
        SetGemText("" + GemCount);
        scoreText.text = "" + playerScore;
        scoreAnim.SetTrigger("Increase");
    }

    private void SetWeaponsInPosition() {
        List<Transform> basePositions = WeaponBaseController.Instance.GetAllWeaponBaseTransform();
        for (int i = 0; i < playerTurrets.Count; i++) {
            playerTurrets[i].SetPositionofWeapon(basePositions[i].transform);
        }
    }

    public void ResetPlayer() {
        playerScore = 0;
        scoreText.text = "" + playerScore;
    }

}
