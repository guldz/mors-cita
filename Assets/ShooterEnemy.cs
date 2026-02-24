using System.Collections;
using UnityEngine;
using Pathfinding;

public class ShooterEnemy : MonoBehaviour
{
    private enum State
    {
        Idle,
        Chase,
        Attack
    }

    private State currentState;

    [Header("Shooting")]
    [SerializeField] private float shootinginterval = 1f;
    private float shootTimer;
    private GunController gun;

    [Header("Vision")]
    public float radius = 10;
    [Range(1, 360)] public float angle = 45f;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;

    [Header("Movement")]
    public float stopDistance = 0.5f;

    private GameObject playerRef;
    private AIPath ai;

    private Vector2 lastKnownPosition;
    private bool hasLineOfSight;

    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        ai = GetComponent<AIPath>();
        gun = GetComponentInChildren<GunController>();

        ai.enableRotation = false;

        currentState = State.Idle;

        StartCoroutine(FOVCheck());
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                break;

            case State.Chase:
                HandleChase();
                break;

            case State.Attack:
                HandleAttack();
                break;
        }
    }

    // STATES 

    void HandleIdle()
    {
        ai.canMove = false;

        if (hasLineOfSight)
        {
            lastKnownPosition = playerRef.transform.position;
            currentState = State.Attack;
        }
    }

    void HandleChase()
    {
        ai.canMove = true;
        ai.destination = lastKnownPosition;

        RotateTowards(ai.desiredVelocity);

        if (hasLineOfSight)
        {
            currentState = State.Attack;
            return;
        }

        if (Vector2.Distance(transform.position, lastKnownPosition) < stopDistance)
        {
            currentState = State.Idle;
        }
    }

    void HandleAttack()
    {
        if (!hasLineOfSight)
        {
            currentState = State.Chase;
            return;
        }

        ai.canMove = false;

        lastKnownPosition = playerRef.transform.position;

        Vector2 dir = (playerRef.transform.position - transform.position).normalized;
        RotateTowards(dir);

        if (shootTimer >= shootinginterval)
        {
            gun.Shoot();
            shootTimer = 0f;
        }
    }

    // HELPERS 

    void RotateTowards(Vector2 dir)
    {
        if (dir != Vector2.zero)
            transform.up = dir;
    }

    // VISION 

    private IEnumerator FOVCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FOV();
        }
    }

    private void FOV()
    {
        Collider2D[] rangeCheck =
            Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        if (rangeCheck.Length > 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector2 directionToTarget =
                (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, directionToTarget) < angle / 2)
            {
                float distanceToTarget =
                    Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(
                        transform.position,
                        directionToTarget,
                        distanceToTarget,
                        obstructionLayer))
                {
                    hasLineOfSight = true;
                    return;
                }
            }
        }

        hasLineOfSight = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(gameObject); 
        } 
        
        }


    private void OnDrawGizmos()
    {
        // Draw detection radius
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);

        // Forward direction
        Vector3 forward = transform.up;

        // Left and right cone edges
        Vector3 leftBoundary = Quaternion.Euler(0, 0, angle / 2) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, -angle / 2) * forward;

        // Vision cone
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * radius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * radius);

        // Forward look line
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + forward * radius);

        // Line to player if detected
        if (hasLineOfSight && playerRef != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, playerRef.transform.position);
        }


    }


}




