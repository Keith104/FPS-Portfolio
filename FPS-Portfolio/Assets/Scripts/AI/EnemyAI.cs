using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] protected int health;
    [SerializeField] protected Renderer model;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Transform shootPos;
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected float searchDist;
    [SerializeField] protected int searchPauseTime;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected int FOV;
    [SerializeField] protected int faceTargetSpeed;
    [SerializeField] protected int amountToScore;
    [SerializeField] protected bool tutorial;
    [SerializeField] protected bool stationary;

    [SerializeField] int animSpeedTrans;
    [SerializeField] Animator animate;

    protected GameObject player;

    Color colorOg;

    float searchTime;
    float angleToPlayer;
    float stoppingDistOg;
    protected float shootTimer;

    bool playerInTrigger;
    bool isAttacking;
    private bool isStunned;

    Vector3 startingPos;
    Vector3 playerDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        startingPos = transform.position;
        colorOg = model.material.color;
        stoppingDistOg = agent.stoppingDistance;
        player = GameManager.instance.Player;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        setAnimations();
        if (agent.remainingDistance < 0.01f)
        {
            searchTime += Time.deltaTime;
        }

        if (player == playerInTrigger && !CanSeePlayer())
        {
            SearchCheck();
        }
        else if(isStunned == false)
        {
            SearchCheck();
        }
    }

    void setAnimations()
    {
        float agentSpeedCur = agent.velocity.normalized.magnitude;
        float animSpeedCur = animate.GetFloat("Speed");


        animate.SetFloat("Speed", Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animSpeedTrans));
    }
    
    public void TakeDamage(int amount, Damage.damagetype damagetype)
    {
        Debug.Log("I'm not crazy");
        Debug.Log(damagetype);
        if (damagetype != Damage.damagetype.stun)
            health -= amount;
        else if (damagetype == Damage.damagetype.stun)
        {
            isStunned = true;
            StartCoroutine(stunTime(amount));
        }

        if (!stationary)
        {
            agent.SetDestination(player.transform.position);
        }

        StartCoroutine(FlashRed());

        if (health <= 0)
        {
            if (!tutorial)
            {
                GameManager.instance.UpdateGameGoal(-1);
            }
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
    IEnumerator stunTime(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        isStunned = false;
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

        if(!stationary)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(ranPos, out hit, searchDist, 1);
            agent.SetDestination(hit.position);
        }
    }

    public virtual bool CanSeePlayer()
    {
        if(isStunned == false)
            return false;

        playerDir = player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        RaycastHit hit;
        if ((Physics.Raycast(transform.position, playerDir, out hit)))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= FOV)
            {
                shootTimer += Time.deltaTime;
                if (shootTimer >= attackCooldown)
                {
                    Shoot();
                }

                if (!stationary)
                {
                    agent.SetDestination(player.transform.position);
                }

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
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, faceTargetSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);
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

        animate.SetTrigger("Shoot");

        Instantiate(bullet, shootPos.position, Quaternion.LookRotation(playerDir));
    }
}
