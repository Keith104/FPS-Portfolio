using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stuff")]
    [SerializeField] CharacterController controller;

    [Header("Movement Settings")]
    [SerializeField] int speed;
    [SerializeField] int sprintMod;

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

    float initialScale;
    float currentScale;
    Vector3 originalPosition;
    Vector3 moveDir;
    Vector3 playerVel;
    int jumpCount;
    float shootTimer;
    bool isCrouched;

    void Start()
    {
        initialScale = transform.localScale.y;
        currentScale = initialScale;
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        Crouch();
        Sprint();
        Movement();
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

        Jump();

        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;

        if (Input.GetButton("Fire1") && shootTimer > shootRate)
            Shoot();
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVel.y = jumpVel;
            jumpCount++;
        }
    }

    void Shoot()
    {
        shootTimer = 0;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance, ~ignoreLayer))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
                dmg.TakeDamage(shootDamage);
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
}
