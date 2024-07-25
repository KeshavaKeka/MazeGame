using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIBattle2 : MonoBehaviour
{
    private PlayerControllerBattle2 script;
    private Transform player;
    public NavMeshAgent playerAgent;
    public GameObject pla;
    public Light torchLight; // Reference to the enemy's torch light
    public float detectionRadius = 10f; // Radius within which the enemy can detect the player with the torch
    public float closeDetectionRadius = 1f; // Smaller radius for close detection
    public float chaseSpeed = 4f;
    public float wanderSpeed = 2f;
    public float wanderRadius = 5f; // Radius within which the enemy will wander
    public float chaseDuration = 5f; // How long the enemy will chase the player after losing sight
    public float checkRate = 0.5f; // How frequently to check for the player
    public GameManagerBattle2 gameManager;
    private NavMeshAgent navAgent;
    private Vector3 startPosition;
    private float lastCheckTime;
    private float lastSightTime;
    private bool isChasing;
    private float zLowerLimit = -18; // Update z lower limit
    private float zUpperLimit = 22;  // Update z upper limit
    private float xLowerLimit = -14; // Add x lower limit
    private float xUpperLimit = 14;  // Add x upper limit
    private Animator anim;

    void Start()
    {
        anim = GameObject.Find("Idle").GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBattle2>();
        pla = GameObject.Find("Player");
        playerAgent = GameObject.Find("Player").GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        script = GameObject.Find("Player").GetComponent<PlayerControllerBattle2>();
        navAgent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;
        anim.SetBool("walking", false);
        StartCoroutine(Wander());
    }

    void Update()
    {
        if (Time.time > lastCheckTime + checkRate)
        {
            lastCheckTime = Time.time;
            CheckForPlayer();
        }

        if (isChasing)
        {
            navAgent.speed = chaseSpeed;
            navAgent.SetDestination(player.position);

            if (Time.time > lastSightTime + chaseDuration)
            {
                isChasing = false;
                anim.SetBool("walking", false);
                StartCoroutine(Wander());
            }
        }
    }

    void CheckForPlayer()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            // Check if player is within the light cone of the torch
            if (angleToPlayer <= torchLight.spotAngle / 2)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius))
                {
                    if (hit.transform == player)
                    {
                        isChasing = true;
                        lastSightTime = Time.time;
                        playerAgent.SetDestination(pla.transform.position);
                        script.canMove = false;
                        gameManager.isActive = false;
                        gameManager.GameOver();
                        StopCoroutine(Wander());
                    }
                }
            }
        }

        // Close detection
        if (!isChasing && distanceToPlayer <= closeDetectionRadius)
        {
            RaycastHit hit;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, closeDetectionRadius))
            {
                if (hit.transform == player)
                {
                    isChasing = true;
                    lastSightTime = Time.time;
                    StopCoroutine(Wander());
                }
            }
        }
    }

    IEnumerator Wander()
    {
        while (!isChasing)
        {
            navAgent.speed = wanderSpeed;
            Vector3 newPos;
            do
            {
                newPos = RandomNavSphere(startPosition, wanderRadius, -1);
            } while (newPos.z > zUpperLimit || newPos.z < zLowerLimit || newPos.x > xUpperLimit || newPos.x < xLowerLimit);
            anim.SetBool("walking", true);
            navAgent.SetDestination(newPos);
            yield return new WaitForSeconds(2);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}