
using UnityEngine;

public class SwingAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlaySwing()
    {
        animator.SetTrigger("Swing");
    }
}

