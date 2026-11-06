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

// Estados de comportamiento del enemigo
public enum EnemyState
{
    Wander,
    Chase,
    Search
}

public class EnemyAI : MonoBehaviour
{
    // ==========================================
    // REFERENCIAS
    // ==========================================

    [Header("Referencias")]

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


    // ==========================================
    // CONFIGURACIÓN DEL ENEMIGO
    // ==========================================

    [Header("Configuración de movimiento")]

    [SerializeField] private float wanderSpeed = 2f;
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float wanderRadius = 15f;
    [SerializeField] private float destinationDistance = 1.5f;


    // ==========================================
    // CONFIGURACIÓN DE DETECCIÓN
    // ==========================================

    [Header("Detección")]

    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float searchDuration = 2f;


    // ==========================================
    // CONFIGURACIÓN DEL DAÑO
    // ==========================================

    [Header("Daño al jugador")]

    [SerializeField] private int damage = 10;


    // ==========================================
    // VARIABLES INTERNAS
    // ==========================================

    private EnemyState currentState;

    private Vector3 lastKnownPosition;

    private float searchTimer;


    // ==========================================
    // INICIALIZACIÓN
    // ==========================================

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();

        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
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


    // ==========================================
    // ACTUALIZACIÓN
    // ==========================================

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


    // ==========================================
    // DETECCIÓN DEL JUGADOR
    // ==========================================

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


    // ==========================================
    // ESTADO: WANDER
    // ==========================================

    private void Wander()
    {
        Agent.speed = wanderSpeed;

        // Si el jugador entra en el rango, comenzar a perseguirlo
        if (PlayerInRange())
        {
            currentState = EnemyState.Chase;
            return;
        }

        // Si el enemigo llegó a su destino, elegir otro
        if (!Agent.pathPending)
        {
            if (Agent.remainingDistance <= destinationDistance)
            {
                SetRandomDestination();
            }
        }
    }


    // ==========================================
    // ESTADO: CHASE
    // ==========================================

    private void Chase()
    {
        Agent.speed = chaseSpeed;

        // Si el jugador sigue dentro del rango, perseguirlo
        if (PlayerInRange())
        {
            lastKnownPosition = Player.position;

            Agent.SetDestination(Player.position);
        }
        else
        {
            // Guardar la última posición conocida del jugador
            lastKnownPosition = Player.position;

            // Preparar el tiempo de búsqueda
            searchTimer = searchDuration;

            // Cambiar al estado de búsqueda
            currentState = EnemyState.Search;

            Agent.SetDestination(lastKnownPosition);
        }
    }


    // ==========================================
    // ESTADO: SEARCH
    // ==========================================

    private void Search()
    {
        Agent.speed = wanderSpeed;

        // Si el jugador vuelve a entrar en el rango,
        // regresar inmediatamente a la persecución
        if (PlayerInRange())
        {
            currentState = EnemyState.Chase;
            return;
        }

        // Reducir el tiempo de búsqueda
        searchTimer -= Time.deltaTime;

        // Comprobar si terminó el tiempo de búsqueda
        if (searchTimer <= 0f)
        {
            currentState = EnemyState.Wander;

            SetRandomDestination();
        }
    }


    // ==========================================
    // ELEGIR DESTINO ALEATORIO
    // ==========================================

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


    // ==========================================
    // DAÑO AL JUGADOR
    // ==========================================

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

        if (PlayerControllerReference.VidaActual > 0)
        {
            PlayerControllerReference.VidaActual =
                PlayerControllerReference.VidaActual - damage;

            if (PlayerControllerReference.VidaActual < 0)
            {
                PlayerControllerReference.VidaActual = 0;
            }

            Debug.Log(
                "Vida del jugador: " +
                PlayerControllerReference.VidaActual
            );
        }
    }
}