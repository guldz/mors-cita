using Pathfinding;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.WSA;

public class lookatplayerscript : MonoBehaviour
{
    public SwingAnimation meleeSwing; 
    public bool eMoving;

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



    private Animator animator;
    public GameObject torso; 
    private Collider2D enemyCollider;


    private bool mafiaDead = false;
    public bool MafiaDead => mafiaDead;

    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        playerMovement = playerRef.GetComponent<PlayerMovement>();

        ai = GetComponent<AIPath>();
        ai.canMove = true;

        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();

        StartCoroutine(FOVCheck());
    }


    void Update()
    {
        if (playerMovement != null && playerMovement.IsDead)
        {
            ai.canMove = false; 
            return; 
        }

        if (hasLineOfSight && playerRef != null)
        {
            lastKnownPosition = playerRef.transform.position;
            hasTargetPosition = true;
        }

        if (hasTargetPosition)
        {
            ai.destination = lastKnownPosition;
            ai.canMove = true;
        }
        else
        {
            ai.canMove = false;
        }

        eMoving = ai.velocity.magnitude > 0.1f;

        // Rotate toward movement
        Vector2 dir = ai.desiredVelocity;
        if (dir != Vector2.zero)
        {
            transform.up = dir;
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
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (player != null && !player.IsDead)
            {
                player.TakingDamage(1);

                if (meleeSwing != null)
                    meleeSwing.PlaySwing();
            }
        }


        if (other.CompareTag("Bullet"))
        {
            if (mafiaDead) return;

            Debug.Log("bullet hit");

            mafiaDead = true;

            // Remove bullet
            Destroy(other.gameObject);

            // Stop movement
            ai.canMove = false;
            this.enabled = false; // disables this script AFTER current frame

            // Disable gun
            GunController gun = GetComponentInChildren<GunController>();
            if (gun != null)
                gun.enabled = false;

            // Disable collider so it doesn't get hit again
            if (enemyCollider != null)
                enemyCollider.enabled = false;

            // Play death animation
            if (animator != null)
                animator.SetTrigger("MafiaDie");

            // Optional: destroy torso (ONLY if you really want this)
            if (torso != null)
            Destroy(torso);

            
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



