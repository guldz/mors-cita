using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform torso;
    [SerializeField] private Transform legs;
    [SerializeField] private Animator animator;
    private bool isDead = false;
    public bool IsDead => isDead;

    public bool freezePlayer; 

    private const int DeadSortingOrder = 0;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip HitBat;
    [SerializeField] private AudioClip PlayerBoom;

    [Header("Camera Shake")]
    [SerializeField] private CinemachineImpulseSource impulseSource;


    public int playerHealth = 1;
    public float moveSpeed = 5f;
    Rigidbody2D rb;
    public bool isMoving = false;
    [SerializeField] float dashForce = 20f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 0f;
    private bool isDashing = false;
    private bool isInvincible = false;

    public GameObject gameOverScreen;

    private Vector2 moveInput;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        GetMovementInput();
        Move();
        LookAtMouse();
        Move();
        Rotatelegs();
        Dash();

    }

    private void GetMovementInput()
    {

        moveInput = Keyboard.current.wKey.isPressed ? Vector2.up : Vector2.zero;
        if (Keyboard.current.sKey.isPressed) moveInput += Vector2.down;
        if (Keyboard.current.aKey.isPressed) moveInput += Vector2.left;
        if (Keyboard.current.dKey.isPressed) moveInput += Vector2.right;

        moveInput = moveInput.normalized;
        isMoving = moveInput != Vector2.zero;
    }

    private void Move()
    {
        transform.position += (Vector3)(moveInput * moveSpeed * Time.deltaTime);
    }

    IEnumerator DashCoroutine()
    {
        isDashing = true;
        isInvincible = true;

        rb.linearVelocity = moveInput * dashForce;

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;
        isDashing = false;
        isInvincible = false;
    }

    void Dash()
    {
        if (isMoving && dashCooldown <= 0 && !isDashing)
        {
            if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
            {
                StartCoroutine(DashCoroutine());
                dashCooldown = 3f;
            }
        }

        dashCooldown -= Time.deltaTime;
    }

    /// <summary>Resets the dash cooldown, allowing an immediate dash.</summary>
    public void ResetDashCooldown()
    {
        dashCooldown = 0f;
    }


    private void LookAtMouse()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = mousePos - (Vector2)torso.position;
        torso.up = direction;
        //transform.up = (Vector3)(mousePos - new Vector2(transform.position.x, transform.position.y));
    }

    private void Rotatelegs()
    {
        if (moveInput != Vector2.zero)
        {
            legs.up = moveInput;
        }
    }

    public void TakingDamage(int damageTaken)
    {
        if (isDead || isInvincible) return;

        isDead = true;

        audioSource.PlayOneShot(HitBat);
       

        animator.SetTrigger("Die");

        if (impulseSource != null)
            impulseSource.GenerateImpulse();

        // Push dead body behind living entities
        StartCoroutine(ApplyDeadSortingOrder());

        // destroy torso so gun disappears
        Destroy(torso.gameObject);

        // disable shooting
        GunController gun = GetComponentInChildren<GunController>();
        if (gun != null)
            gun.enabled = false;

        // disable collider so enemies stop hitting the corpse
        GetComponent<Collider2D>().enabled = false;

        // Disable player movement
        this.enabled = false;

        gameOverScreen.SetActive(true);


    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy Bullet"))
        {
            if (isDead || isInvincible) return;

            Debug.Log("bullet hit");

            isDead = true;
            if (impulseSource != null)
                impulseSource.GenerateImpulse();

            // Play death animation
            animator.SetTrigger("Die");

            // Push dead body behind living entities
            StartCoroutine(ApplyDeadSortingOrder());

            // Destroy torso so the gun disappears
            Destroy(torso.gameObject);

            // Disable shooting
            GunController gun = GetComponentInChildren<GunController>();
            if (gun != null)
                gun.enabled = false;

            // Disable player movement
            this.enabled = false;

            gameOverScreen.SetActive(true);
        }
    }

    /// <summary>
    /// Freezes the player for the boss-kill explosion sequence.
    /// Disables legs and gun without destroying the torso or showing the game over screen.
    /// </summary>
    public void FreezeForBoom()
    {
        if (isDead) return;

        isDead = true;
        audioSource.PlayOneShot(PlayerBoom);

        // Disable legs so the player cannot move or rotate them
        if (legs != null)
            legs.gameObject.SetActive(false);

        // Disable shooting via the canShoot guard so Input System callbacks can't fire a shot
        GunController gun = GetComponentInChildren<GunController>();
        if (gun != null)
            gun.DisableShooting();

        MachineGunController machineGun = GetComponentInChildren<MachineGunController>();
        if (machineGun != null)
            machineGun.DisableShooting();

        // Freeze movement
        this.enabled = false;
    }

    /// <summary>Waits one frame then forces all SpriteRenderers behind living entities.</summary>
    private System.Collections.IEnumerator ApplyDeadSortingOrder()
    {
        yield return null;
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            sr.sortingOrder = DeadSortingOrder;
    }

 



}

