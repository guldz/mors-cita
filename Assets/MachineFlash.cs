using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MachineFlash : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayFlash()
    {
        animator.SetTrigger("Mflash");
    }
}

