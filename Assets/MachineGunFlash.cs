using UnityEngine;

public class MachineGunFlash : MonoBehaviour
{
    private const string FlashTrigger = "Mflash";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>Plays the machine gun muzzle flash animation.</summary>
    public void PlayFlash()
    {
        animator.SetTrigger(FlashTrigger);
    }
}
