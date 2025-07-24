using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using NUnit.Framework.Constraints;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("Player Stuff")]
    [SerializeField] CharacterController controller;
    [SerializeField] float health;
    [SerializeField] float hpBarLerpSpeed;
    [SerializeField] AudioSource source;
    [SerializeField] Animator animate;
    public WeaponSelection Gun;
    public SwappingSystem swappingSystem;

    [Header("Movement Settings")]
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] float stepInterval;

    [Header("Jump Settings")]
    [SerializeField] int jumpVel;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    [Header("Shoot Settings")]
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDistance;
    [SerializeField] LayerMask ignoreLayer;

    [Header("Crouch Settings")]
    [SerializeField] float crouchHeight;
    [SerializeField] float heightAdjustSpeed;
    [SerializeField] float headCheckDistance;
    [SerializeField] LayerMask obstacleMask;

    [Header("Respawn Settings")]
    [SerializeField] Transform PlayerTransform;
    [SerializeField] Transform RespawnPoint;
    [SerializeField] int maxRespawns;

    CapsuleCollider col;
    float standingHeight;
    Vector3 standingCenter;
    Vector3 crouchCenter;
    float ctrlStandingHeight;
    Vector3 ctrlStandingCenter;
    Vector3 ctrlCrouchCenter;

    float stepTimer;
    Vector3 originalPosition;
    Vector3 moveDir;
    Vector3 playerVel;
    int jumpCount;
    float shootTimer;
    bool isCrouched;
    bool playerDead;
    float hpOrig;
    float hpBarTarget;

    void Start()
    {
        col = GetComponent<CapsuleCollider>();
        standingHeight = col.height;
        standingCenter = col.center;
        crouchCenter = new Vector3(standingCenter.x, crouchHeight / 2f, standingCenter.z);

        ctrlStandingHeight = controller.height;
        ctrlStandingCenter = controller.center;
        ctrlCrouchCenter = new Vector3(ctrlStandingCenter.x, crouchHeight / 2f, ctrlStandingCenter.z);

        originalPosition = transform.localPosition;
        hpOrig = Mathf.Max(health, 1);
        hpBarTarget = 1f;
        maxRespawns = DifficultyManager.instance.GetDifficulty().maxRespawns;
    }

    void Update()
    {
        Crouch();
        Sprint();
        Movement();

        if (Input.GetKeyDown(KeyCode.K))
            TakeDamage(1, Damage.damagetype.stationary);

        if (maxRespawns <= 0)
            GameManager.instance.respawnButton.interactable = false;

        var bar = UIManager.instance.playerHPBar;
        float next = Mathf.Lerp(bar.fillAmount, hpBarTarget, hpBarLerpSpeed * Time.unscaledDeltaTime);
        bar.fillAmount = Mathf.Clamp01(next);
    }

    void Movement()
    {
        shootTimer += Time.deltaTime;
        if (controller.isGrounded)
        {
            playerVel.y = -2f;
            jumpCount = 0;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speed * Time.deltaTime);

        if (controller.isGrounded && moveDir.magnitude > 0f)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                AudioManager.instance.AudioMovement(source);
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = stepInterval;
        }

        Jump();
        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;

        if (Input.GetButton("Fire1") && shootTimer > shootRate && !GameManager.instance.GetPause())
            Shoot();
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            AudioManager.instance.AudioJump(source);
            playerVel.y = jumpVel;
            jumpCount++;
        }
    }

    void Shoot()
    {
        if (!GameManager.instance.GetPause())
            Gun.shoot();
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
            speed *= sprintMod;
        else if (Input.GetButtonUp("Sprint"))
            speed /= sprintMod;
    }

    void Crouch()
    {
        if (Input.GetButton("Crouch"))
            isCrouched = true;
        else if (isCrouched)
        {
            Debug.Log("CROUCH BITCH");
            Vector3 worldCenter = transform.position + col.center;
            Vector3 rayOrigin = worldCenter + Vector3.up * (col.height * 0.5f);
            if (!Physics.Raycast(rayOrigin, Vector3.up, headCheckDistance, obstacleMask))
                isCrouched = false;
        }
        Debug.Log("WHY NOT CROUCH");
        float targetHeight = isCrouched ? crouchHeight : standingHeight;
        col.height = Mathf.MoveTowards(col.height, targetHeight, heightAdjustSpeed * Time.deltaTime);
        Vector3 targetCenter = isCrouched ? crouchCenter : standingCenter;
        col.center = Vector3.MoveTowards(col.center, targetCenter, heightAdjustSpeed * Time.deltaTime);
        Debug.Log("KILL ME");
        float ctrlTargetHeight = isCrouched ? crouchHeight : ctrlStandingHeight;
        controller.height = Mathf.MoveTowards(controller.height, ctrlTargetHeight, heightAdjustSpeed * Time.deltaTime);
        Vector3 ctrlTargetCenter = isCrouched ? ctrlCrouchCenter : ctrlStandingCenter;
        controller.center = Vector3.MoveTowards(controller.center, ctrlTargetCenter, heightAdjustSpeed * Time.deltaTime);
    }

    public void TakeDamage(float amount, Damage.damagetype damagetype)
    {
        if (damagetype != Damage.damagetype.stun && damagetype != Damage.damagetype.smoke)
        {
            health = Mathf.Max(health - amount, 0);
            hpBarTarget = Mathf.Clamp01((float)health / hpOrig);
        }

        if(damagetype == Damage.damagetype.stun)
        {
            StartCoroutine(stunFlashScreen(amount));
        }
        else if (amount > 0)
        {
            AudioManager.instance.AudioHurt(source);
            StartCoroutine(damageFlashScreen());
        }
        else if(amount < 0)
        {
            StartCoroutine(healFlashScreen());
            if(health > hpOrig)
                health = hpOrig;
        }

            Debug.Log("Ouch");

        if (health <= 0 && !playerDead)
        {
            playerDead = true;
            GameManager.instance.Lose();
        }
    }


    public void RespawnPlayer()
    {
        if (maxRespawns > 0)
        {
            playerDead = false;
            controller.enabled = false;
            PlayerTransform.position = RespawnPoint.position;
            controller.enabled = true;
            health = hpOrig;
            hpBarTarget = 1f;
            maxRespawns--;
        }
    }

    IEnumerator damageFlashScreen()
    {
        UIManager.instance.playerDamagePanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        UIManager.instance.playerDamagePanel.SetActive(false);
    }

    IEnumerator healFlashScreen()
    {
        UIManager.instance.playerHealPanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        UIManager.instance.playerHealPanel.SetActive(false);
    }
    IEnumerator stunFlashScreen(float stunTime)
    {
        UIManager.instance.playerStunPanel.SetActive(true);
        yield return new WaitForSeconds(stunTime);
        UIManager.instance.playerStunPanel.SetActive(false);
    }
}
