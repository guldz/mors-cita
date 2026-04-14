using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Gunnerlegs : MonoBehaviour
{
    private Animator animator;
    private ShooterEnemy gunnerlegs;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gunnerlegs = transform.parent.parent.GetComponentInParent<ShooterEnemy>();
    }

    private void Update()
    {
        bool shMove = gunnerlegs != null && !gunnerlegs.SHdead && gunnerlegs.shMove;
        animator.SetBool("shMove", shMove);
    }
}