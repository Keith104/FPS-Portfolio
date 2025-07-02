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

    Vector3 moveDir;
    Vector3 playerVel;

    int jumpCount;

    float shootTimer;
    void Update()
    {
        Sprint();

        Movement();
    }

    void Movement()
    {
        shootTimer += Time.deltaTime;

        if(controller.isGrounded)
        {
            playerVel = Vector3.zero;
            jumpCount = 0;
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);
        controller.Move(moveDir * speed * Time.deltaTime);

        Jump();

        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;

        if(Input.GetButton("Fire1") && shootTimer > shootRate)
        {
            Shoot();
        }

    }   

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVel.y = jumpVel;
            jumpCount++;
        }
    }

    void Sprint()
    {
        if(Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    void Shoot()
    {
        shootTimer = 0;

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);

        RaycastHit hit;

        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance, ~ignoreLayer))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if(dmg != null)
            {
                dmg.TakeDamage(shootDamage);
            }
        }
    }
}
