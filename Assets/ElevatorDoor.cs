using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    public Animator elevatordoorAnimator;
    public Collider2D doorCollider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            elevatordoorAnimator.SetTrigger("open");
            doorCollider.enabled = false; // Now enemies can path through   
        }

        Animator playerAnimator = other.GetComponent<Animator>();
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Punch");
        }
    }
    //maybe add a slower animation for the elevatoe doors idk?
}
