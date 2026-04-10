using UnityEngine;
using System.Collections;

public class ElevatorDoor : MonoBehaviour
{
    public Animator elevatordoorAnimator;
    public Collider2D doorCollider;

    private bool isOpening = false; // prevents multiple triggers

    public void DisableCollider()
    {
        doorCollider.enabled = false;
    }

    private void Start()
    {
        // Door starts closed and blocking
        doorCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpening)
        {
            isOpening = true; // lock forever after first use

            Animator playerAnimator = other.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("Press");
            }

            StartCoroutine(OpenDoorWithDelay());
        }
    }

    private IEnumerator OpenDoorWithDelay()
    {
        yield return new WaitForSeconds(1f);

        elevatordoorAnimator.SetTrigger("open");
        doorCollider.enabled = false; 

       
    }
}
