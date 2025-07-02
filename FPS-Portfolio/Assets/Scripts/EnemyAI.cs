using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] int health;

    [SerializeField] Renderer model;

    [SerializeField] NavMeshAgent agent;

    [SerializeField] Transform player;

    [SerializeField] LayerMask whatIsGround, whatIsPlayer;

    Color colorOg;

    //Variables for Searching
    [SerializeField] Vector3 walkPath;
    [SerializeField] bool pathSet;
    [SerializeField] float walkRange;

    //Variables for Attacking
    [SerializeField] float attackCooldown;
    bool isAttacking;

    //For checking states
    [SerializeField] float sightRange, attackRange;
    [SerializeField] bool inSightRange, inAttackRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOg = model.material.color;
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.F)) TakeDamage(1); Debug purposes

        inSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        inAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (player == inSightRange && player != inAttackRange)
        {
            ChasePlayer();
        }
        else if (player == inSightRange && player == inAttackRange)
        {
            AttackPlayer();
        }
        else
        {
            Search();
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        StartCoroutine(FlashRed());

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOg;
    }

    private void Search()
    {
        if (!pathSet)
        {
            SearchWalkPoint();
        }

        if (pathSet) 
        {
            agent.SetDestination(walkPath);
        }

        Vector3 distanceToDest = transform.position - walkPath;

        if (distanceToDest.magnitude <= 1f)
        {
            pathSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        //Gives the enemy a place to walk to
        float randomRangeX = Random.Range(-walkRange, walkRange);
        float randomRangeZ = Random.Range(-walkRange, walkRange);

        walkPath = new Vector3(transform.position.x + randomRangeX, transform.position.y ,transform.position.x + randomRangeZ);

        //Hopefully this makes it so they doesn't fall off the map
        if(Physics.Raycast(walkPath, -transform.up, 2f, whatIsGround))
        {
            pathSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player.position);

        if (!isAttacking)
        {
            //they shoot

            isAttacking = true;

        }
    }
}
