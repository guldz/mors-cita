using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

public class ShooterEnemy : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private float shootinginterval = 1.0f;
    private float shootTimer;
    private GunController gun;

    public int EnemyHealth = 1;
    public float radius = 10;
    [Range(1, 360)] public float angle = 45f;

    public LayerMask targetLayer;
    public LayerMask obstructionLayer;

    public float chargeSpeed = 6f;
    public float stopDistance = 0.2f;

    private GameObject playerRef;
    private PlayerMovement playerMovement;

    public bool hasLineOfSight { get; private set; }

    private Vector2 lastKnownPosition;
    private bool hasTargetPosition;
    private AIPath ai;



    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        playerMovement = playerRef.GetComponent<PlayerMovement>();

        StartCoroutine(FOVCheck());

        ai = GetComponent<AIPath>();
        ai.canMove = true;

        gun = GetComponentInChildren<GunController>();
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (hasLineOfSight && playerRef != null)
        {
            // Remember where player was
            lastKnownPosition = playerRef.transform.position;
            hasTargetPosition = true;

            Vector2 dir = (playerRef.transform.position - transform.position).normalized;
            if (dir != Vector2.zero)
            {
                transform.up = dir; 
            }

            // Shoot
            if (shootTimer >= shootinginterval)
            {
                gun.Shoot();
                shootTimer = 0f;
            }
        }
        else
        {
            Vector2 dir = ai.desiredVelocity;
            if (dir != Vector2.zero)
            {
                transform.up = dir;
            }
            // Player not visible → go investigate last position
            if (hasTargetPosition)
            {
                MoveToLastKnownPosition();
            }
        }
    }



    private void MoveToLastKnownPosition()
    {
        Vector2 direction = (lastKnownPosition - (Vector2)transform.position);
        float distance = direction.magnitude;

        if (distance > stopDistance)
        {
            direction.Normalize();

            transform.position += (Vector3)(direction * chargeSpeed * Time.deltaTime);

            // Rotate toward movement
            transform.up = direction;
        }
        else
        {
            // Player not visible → go investigate last known position
            if (hasTargetPosition)
            {
                ai.destination = lastKnownPosition;
                ai.canMove = true;

                // Stop if reached last known position
                if (Vector2.Distance(transform.position, lastKnownPosition) < stopDistance)
                {
                    hasTargetPosition = false;
                    ai.canMove = false;
                }
            }
            else
            {
                ai.canMove = false;
            }
        }


    }

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
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        if (rangeCheck.Length > 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))
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
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().TakingDamage(1);
        }

        if (other.tag == "Bullet")
        {
            Debug.Log("Hit by " + other);
            Destroy(gameObject);
        }


    }

    public void TakingDamage(int damageTaken)
    {
        EnemyHealth = EnemyHealth - damageTaken;
        if (EnemyHealth <= 0)
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




