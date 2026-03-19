using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator doorAnimator;
    public Collider2D doorCollider;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnimator.SetTrigger("Open");
            doorCollider.enabled = false; // Now enemies can path through   
        }

        Animator playerAnimator = other.GetComponent<Animator>();
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Punch");
        }
    }
}


