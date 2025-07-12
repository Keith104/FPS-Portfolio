using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] int health;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] float searchDist;
    [SerializeField] int searchPauseTime;
    [SerializeField] float attackCooldown;
    [SerializeField] int FOV;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int amountToScore;

    GameObject player;

    Color colorOg;

    float searchTime;
    float angleToPlayer;
    float stoppingDistOg;
    float shootTimer;

    bool playerInTrigger;
    bool isAttacking;

    Vector3 startingPos;
    Vector3 playerDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPos = transform.position;
        colorOg = model.material.color;
        stoppingDistOg = agent.stoppingDistance;
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < 0.01f)
        {
            searchTime += Time.deltaTime;
        }

        if (player == playerInTrigger && !CanSeePlayer())
        {
            SearchCheck();
        }
        else
        {
            SearchCheck();
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        agent.SetDestination(player.transform.position);

        StartCoroutine(FlashRed());

        if (health <= 0)
        {
            GameManager.instance.UpdateGameGoal(-1);
            GameManager.instance.UpdateTotalScoreText(amountToScore);
            GameManager.instance.UpdateWaveScoreText(amountToScore);
            Destroy(gameObject);
        }
    }

    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOg;
    }

    void SearchCheck()
    {
        if (searchTime >= searchPauseTime && agent.remainingDistance < 0.01f)
        {
            Search();
        }
    }
    void Search()
    {
        searchTime = 0;
        agent.stoppingDistance = 0;

        Vector3 ranPos = Random.insideUnitSphere * searchDist;
        ranPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, searchDist, 1);
        agent.SetDestination(hit.position);
    }

    bool CanSeePlayer()
    {
        playerDir = player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        Debug.DrawRay(transform.position, playerDir);

        RaycastHit hit;
        if ((Physics.Raycast(transform.position, playerDir, out hit)))
        {
            Debug.Log("RayCast hit");
            if (hit.collider.CompareTag("Player") && angleToPlayer <= FOV)
            {
                shootTimer += Time.deltaTime;
                if (shootTimer >= attackCooldown)
                {
                    Shoot();
                }

                agent.SetDestination(player.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                    FaceTarget();

                agent.stoppingDistance = stoppingDistOg;
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

    void FaceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, playerDir.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, faceTargetSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            agent.stoppingDistance = 0;
        }
    }
    void Shoot()
    {
        shootTimer = 0;
        Instantiate(bullet, shootPos.position, transform.rotation);
    }
}
