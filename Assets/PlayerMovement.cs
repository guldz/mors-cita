using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform torso;
    [SerializeField] private Transform legs;

    public int playerHealth = 1;
    public float moveSpeed = 5f;
    Rigidbody2D rb;
    public bool isMoving => rb.linearVelocity.magnitude > 0.1f;

    private Vector2 moveInput;

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
        playerHealth = playerHealth - damageTaken;
        if (playerHealth <= 0)
        {
            Destroy(gameObject);

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
     
        if (other.tag == "Enemy Bullet")
        {
            Debug.Log("Hit by " + other);
            Destroy(gameObject);
        }

    }
}

