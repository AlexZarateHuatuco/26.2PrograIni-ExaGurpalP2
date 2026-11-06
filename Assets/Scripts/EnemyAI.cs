/*
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Wander,
        Chase,
        Search
    }

    public Transform player;

    private NavMeshAgent agent;

    public float detectionRange = 10f;

    public float wanderSpeed = 2f;
    public float chaseSpeed = 4f;
    public float wanderRadius = 15f;
    public float destinationDistance = 1.5f;

    public float searchDuration = 2f;

    private EnemyState currentState = EnemyState.Wander;

    private Vector3 lastKnownPosition;

    private float searchTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = wanderSpeed;

        SetRandomDestination();
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Wander:
                Wander();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Search:
                Search();
                break;
        }
    }

    bool PlayerInRange()
    {
        if (player == null)
        {
            return false;
        }

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance <= detectionRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Wander()
    {
        agent.speed = wanderSpeed;

        if (PlayerInRange())
        {
            currentState = EnemyState.Chase;
            return;
        }

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= destinationDistance)
            {
                SetRandomDestination();
            }
        }
    }

    void Chase()
    {
        agent.speed = chaseSpeed;

        if (PlayerInRange())
        {
            lastKnownPosition = player.position;

            agent.SetDestination(player.position);
        }
        else
        {

            searchTimer = searchDuration;

            currentState = EnemyState.Search;
        }
    }

    void Search()
    {
        agent.speed = chaseSpeed;

        if (PlayerInRange())
        {
            currentState = EnemyState.Chase;
            return;
        }

        searchTimer = searchTimer - Time.deltaTime;

        if (searchTimer <= 0f)
        {
            currentState = EnemyState.Wander;

            SetRandomDestination();
        }
        else
        {
            agent.SetDestination(lastKnownPosition);
        }
    }

    void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;

        randomDirection = randomDirection + transform.position;

        NavMeshHit hit;

        bool foundPosition = NavMesh.SamplePosition(
            randomDirection,
            out hit,
            wanderRadius,
            NavMesh.AllAreas
        );

        if (foundPosition)
        {
            agent.SetDestination(hit.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );
    }
}
*/
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Wander,
    Chase,
    Search
}

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    public PlayerController PlayerControllerReference
    {
        get;
        private set;
    }

    public Transform Player
    {
        get;
        private set;
    }

    public NavMeshAgent Agent
    {
        get;
        private set;
    }

    [SerializeField] private float wanderSpeed = 2f;
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float wanderRadius = 15f;
    [SerializeField] private float destinationDistance = 1.5f;

    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float searchDuration = 2f;

    [SerializeField] private int damage = 10;

    private EnemyState currentState;

    private Vector3 lastKnownPosition;

    private float searchTimer;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();

        if (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>();
        }

        PlayerControllerReference = playerController;

        if (PlayerControllerReference != null)
        {
            Player = PlayerControllerReference.transform;
        }
    }

    private void Start()
    {
        Agent.speed = wanderSpeed;

        currentState = EnemyState.Wander;

        SetRandomDestination();
    }

    private void Update()
    {
        if (PlayerControllerReference == null)
        {
            return;
        }

        switch (currentState)
        {
            case EnemyState.Wander:
                Wander();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Search:
                Search();
                break;
        }
    }

    private bool PlayerInRange()
    {
        float distance = Vector3.Distance(
            transform.position,
            Player.position
        );

        if (distance <= detectionRange)
        {
            return true;
        }

        return false;
    }

    private void Wander()
    {
        Agent.speed = wanderSpeed;

        if (PlayerInRange())
        {
            currentState = EnemyState.Chase;
            return;
        }

        if (!Agent.pathPending)
        {
            if (Agent.remainingDistance <= destinationDistance)
            {
                SetRandomDestination();
            }
        }
    }

    private void Chase()
    {
        Agent.speed = chaseSpeed;

        if (PlayerInRange())
        {
            lastKnownPosition = Player.position;

            Agent.SetDestination(Player.position);
        }
        else
        {
            lastKnownPosition = Player.position;

            searchTimer = searchDuration;

            currentState = EnemyState.Search;

            Agent.SetDestination(lastKnownPosition);
        }
    }

    private void Search()
    {
        Agent.speed = wanderSpeed;

        if (PlayerInRange())
        {
            currentState = EnemyState.Chase;
            return;
        }

        searchTimer -= Time.deltaTime;

        if (searchTimer <= 0f)
        {
            currentState = EnemyState.Wander;

            SetRandomDestination();
        }
    }

    private void SetRandomDestination()
    {
        Vector3 randomPosition =
            Random.insideUnitSphere * wanderRadius;

        randomPosition += transform.position;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(
            randomPosition,
            out hit,
            wanderRadius,
            NavMesh.AllAreas))
        {
            Agent.SetDestination(hit.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController detectedPlayer =
            other.GetComponentInParent<PlayerController>();

        if (detectedPlayer == null)
        {
            return;
        }

        if (detectedPlayer != PlayerControllerReference)
        {
            return;
        }

        PlayerControllerReference.ModificarVida(-damage);

        Debug.Log("El enemigo ha causado " + damage + " puntos de daño.");
    }
}