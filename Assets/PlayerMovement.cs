using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform torso;
    [SerializeField] private Transform legs;
    [SerializeField] private Animator animator;
    private bool isDead = false;
    public bool IsDead => isDead; 


    public int playerHealth = 1;
    public float moveSpeed = 5f;
    Rigidbody2D rb;
    public bool isMoving = false; 
    

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
        if (isDead) return;

        isDead = true;

        animator.SetTrigger("Die");

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
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy Bullet"))
        {
            if (isDead) return;

            Debug.Log("bullet hit");

            isDead = true;

            // Play death animation
            animator.SetTrigger("Die");

            // Destroy torso so the gun disappears
            Destroy(torso.gameObject);

            // Disable shooting
            GunController gun = GetComponentInChildren<GunController>();
            if (gun != null)
                gun.enabled = false;

            // Disable player movement
            this.enabled = false;
        }
    }





}

