using Pathfinding;
using System.Collections;
using UnityEngine;

public class MachineGunEnemy : MonoBehaviour
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
    private MachineGunController gun;

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

    public bool shMove;
    private MachineGunFlash gunFlash;

    private Animator animator;
    public GameObject torso;
    private Collider2D enemyCollider;

    private bool shdead = false;
    public bool SHdead => shdead;

    private const int DeadSortingOrder = 0;

    void Start()
    {
        gunFlash = GetComponentInChildren<MachineGunFlash>();
        playerRef = GameObject.FindGameObjectWithTag("Player");
        ai = GetComponent<AIPath>();
        gun = GetComponentInChildren<MachineGunController>();

        ai.enableRotation = false;

        currentState = State.Idle;

        StartCoroutine(FOVCheck());

        animator = GetComponentInChildren<Animator>();
        enemyCollider = GetComponent<Collider2D>();
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

        shMove = ai.canMove && ai.desiredVelocity.magnitude > 0.1f;
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
            transform.GetChild(0).GetComponent<MafiaGunnerAnimation>().GunnerShoot_ani();
            animator.SetTrigger("Mshoot");
            gunFlash?.PlayFlash();

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
            if (shdead) return;

            shdead = true;

            Destroy(other.gameObject);

            if (ai != null)
                ai.canMove = false;

            if (ai != null)
                ai.enabled = false;

            if (gun != null)
                gun.enabled = false;

            if (enemyCollider != null)
                enemyCollider.enabled = false;

            if (animator != null)
                animator.SetTrigger("SHdead");

            StartCoroutine(ApplyDeadSortingOrder());

            this.enabled = false;
        }
    }

    /// <summary>Waits one frame then forces all child SpriteRenderers behind living entities.</summary>
    private IEnumerator ApplyDeadSortingOrder()
    {
        yield return null;
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            sr.sortingOrder = DeadSortingOrder;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);

        Vector3 forward = transform.up;

        Vector3 leftBoundary = Quaternion.Euler(0, 0, angle / 2) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, -angle / 2) * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * radius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * radius);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + forward * radius);

        if (hasLineOfSight && playerRef != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, playerRef.transform.position);
        }
    }
}
