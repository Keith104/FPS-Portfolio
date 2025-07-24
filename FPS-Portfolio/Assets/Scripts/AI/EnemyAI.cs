using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] protected float health;
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
    [SerializeField] protected GameObject[] drops;

    protected GameObject player;



    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip[] audioHurt;
    [SerializeField] float audioHurtVol;
    [SerializeField] AudioClip[] audioStep;
    [SerializeField] float audioStepVol;
    [SerializeField] AudioClip[] audioShoot;
    [SerializeField] float audioShootVol;

    bool isSprinting;
    bool isPlayingStep;

    Color colorOg;

    float searchTime;
    float angleToPlayer;
    float stoppingDistOg;
    protected float shootTimer;

    bool playerInTrigger;
    bool isAttacking;
    private bool isStunned;
    private bool isSmoked;

    Vector3 startingPos;
    protected Vector3 playerDir;

    public virtual void Start()
    {
        startingPos = transform.position;
        colorOg = model.material.color;
        stoppingDistOg = agent.stoppingDistance;
        player = GameManager.instance.Player;
        agent = GetComponent<NavMeshAgent>();
    }

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
        else
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
    
    public void TakeDamage(float amount, Damage.damagetype damagetype)
    {
        if (damagetype != Damage.damagetype.stun && damagetype != Damage.damagetype.smoke)
            health -= amount;
        else if (damagetype == Damage.damagetype.stun)
        {
            isStunned = true;
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            StartCoroutine(StunTime(amount));
        }
        else if (damagetype == Damage.damagetype.smoke)
        {
            isSmoked = true;
            StartCoroutine(SmokeTime(amount));
        }

        if (!stationary)
        {
            agent.SetDestination(player.transform.position);
        }

        StartCoroutine(FlashRed());

        audio.PlayOneShot(audioHurt[Random.Range(0, audioHurt.Length)], audioHurtVol);

        if (health <= 0)
        {
            
            int randNum = Random.Range(0, 10);

            if (randNum == 10)
            {
                Drops();
            }

            GameManager.instance.UpdateTotalScoreText(amountToScore);
            GameManager.instance.UpdateWaveScoreText(amountToScore);
            StartCoroutine(DestroyAfterDelay());
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        agent.isStopped = true;
        animate.SetTrigger("Dead");
        yield return new WaitForSeconds(2.5f);
        if (!tutorial)
        {
            GameManager.instance.UpdateGameGoal(-1);
        }
        Destroy(gameObject);
    }

    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        model.material.color = colorOg;
    }

    IEnumerator StunTime(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        isStunned = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }

    IEnumerator SmokeTime(float smokeTime)
    {
        yield return new WaitForSeconds(smokeTime);
        isSmoked = false;
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
            if (!isPlayingStep)
            {
                StartCoroutine(playStep());
            }
        }
    }

    IEnumerator playStep()
    {
        isPlayingStep = true;
        audio.PlayOneShot(audioStep[Random.Range(0, audioStep.Length)], audioStepVol);

        if (isSprinting)
        {
            yield return new WaitForSeconds(0.3f);
        }
        else
            yield return new WaitForSeconds(0.5f);
        isPlayingStep = false;
    }

    public virtual bool CanSeePlayer()
    {
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

    public virtual void Shoot()
    {
        if (isStunned == false)
        {
            shootTimer = 0;

            animate.SetTrigger("Shoot");

            audio.PlayOneShot(audioShoot[Random.Range(0, audioShoot.Length)], audioShootVol);

            if(isSmoked == false)
                Instantiate(bullet, shootPos.position, transform.localRotation);
            else if(isSmoked == true)
                Instantiate(bullet, shootPos.position, Random.rotation);
        }
    }

    void Drops()
    {
        Instantiate(drops[Random.Range(0, drops.Length)], transform.position, transform.rotation);
    }
}
