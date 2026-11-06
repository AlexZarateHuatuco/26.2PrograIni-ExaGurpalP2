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