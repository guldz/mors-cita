using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Enemylegs : MonoBehaviour
{
    private Animator animator;
    private lookatplayerscript enemylegs;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemylegs = transform.parent.parent.GetComponentInParent<lookatplayerscript>();
    }

    private void Update()
    {
        bool eMoving =enemylegs != null && enemylegs.eMoving;
        animator.SetBool("eMoving", eMoving);
        
    }
}
