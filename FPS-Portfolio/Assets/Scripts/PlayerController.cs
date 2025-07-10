using NUnit.Framework.Constraints;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("Player Stuff")]
    [SerializeField] CharacterController controller;
    [SerializeField] int health;
    [SerializeField] float hpBarLerpSpeed;
    [SerializeField] AudioSource source;
    [SerializeField] Animator animate;
    public WeaponSelection Gun; // Takes in a weapon Selection script, that then holds equipment for the type of weapon

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
    [SerializeField] float scaleSpeed;
    [SerializeField] float maxHeight;

    [Header("Respawn Settings")]
    [SerializeField] Transform PlayerTransform;
    [SerializeField] Transform RespawnPoint;

    float stepTimer;
    float initialScale;
    float currentScale;
    Vector3 originalPosition;
    Vector3 moveDir;
    Vector3 playerVel;
    int jumpCount;
    float shootTimer;
    bool isCrouched;
    bool playerDead;

    int hpOrig;

    float hpBarTarget;
    

    void Start()
    {
        initialScale = transform.localScale.y;
        currentScale = initialScale;
        originalPosition = transform.localPosition;

        hpOrig = Mathf.Max(health, 1);
        hpBarTarget = 1f;

    }

    void Update()
    {
        Crouch();
        Sprint();
        Movement();

        if(Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(1);
        }

        //if(playerDead)
        //{
        //    RespawnPlayer();
        //}

        var bar = UIManager.instance.playerHPBar;
        float next = Mathf.Lerp(bar.fillAmount, hpBarTarget, hpBarLerpSpeed * Time.deltaTime);
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

        if(controller.isGrounded && moveDir.magnitude > 0f)
        {
            stepTimer += Time.deltaTime;
            if(stepTimer >= stepInterval)
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
        if(!GameManager.instance.GetPause())
        {
            //AudioManager.instance.AudioGunShot(source);
            //shootTimer = 0;
            //RaycastHit hit;
            //if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance, ~ignoreLayer))
            //{
            //    IDamage dmg = hit.collider.GetComponent<IDamage>();
            //    if (dmg != null)
            //        dmg.TakeDamage(shootDamage);
            //    Gun.shoot();
            //}

            //This calls weapon selection shoot
            Gun.shoot();
        }
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
        if(Input.GetButtonDown("Crouch"))
        {
            isCrouched = true;
        }else if (Input.GetButtonUp("Crouch"))
        {
            isCrouched = false;
        }

        float targetScale = isCrouched ? maxHeight : initialScale;

        currentScale = Mathf.MoveTowards(currentScale, targetScale, scaleSpeed * Time.deltaTime);

        Vector3 scale = transform.localScale;
        scale.y = currentScale;
        transform.localScale = scale;

        Vector3 pos = originalPosition;
        pos.y = (initialScale - currentScale) * 0.5f;
        transform.localPosition = pos;
    }

    public void TakeDamage(int amount)
    {
        health = Mathf.Max(health - amount, 0);
        hpBarTarget = Mathf.Clamp01((float)health / hpOrig);
        AudioManager.instance.AudioHurt(source);
        StartCoroutine(damageFlashScreen());

        if (health <= 0 && !playerDead)
        {
            playerDead = true;
            Debug.Log("Player Dead");
            GameManager.instance.Lose();
        }
    }

    public void RespawnPlayer()
    {
        controller.enabled = false;

        PlayerTransform.position = RespawnPoint.position;

        controller.enabled = true;

        health = hpOrig;
        hpBarTarget = 1f;
    }

    IEnumerator damageFlashScreen()
    {
        UIManager.instance.playerDamagePanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        UIManager.instance.playerDamagePanel.SetActive(false);
    }

}
