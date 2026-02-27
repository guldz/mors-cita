using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LegsAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        bool isMoving = playerMovement != null && playerMovement.isMoving;
        animator.SetBool("moving", isMoving);
    }
}
