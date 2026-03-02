using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LegsAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement PlayerMovement;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        PlayerMovement = transform.parent.parent.GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        bool isMoving =PlayerMovement != null && PlayerMovement.isMoving;
        animator.SetBool("moving", isMoving);
        
    }
}
