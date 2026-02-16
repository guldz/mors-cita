using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    
    void Start()
    {
        
    }

    
    void Update()
    {
        LookAtMouse();
        

    }

    private void LookAtMouse()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); //current position type shit idiot!
        transform.up = (Vector3)(mousePos - new Vector2(transform.position.x, transform.position.y));
    }

 
}
