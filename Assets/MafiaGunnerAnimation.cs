using UnityEngine;
using UnityEngine.InputSystem;

public class MafiaGunnerAnimation : MonoBehaviour
 
{
    private Animator animator;
    private ShooterEnemy enemy;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip MachineShoot;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<ShooterEnemy>(); 
    }

    private void Update()
    {
        if (animator != null)
        {
            
        }
    }
    public void GunnerShoot_ani()
    {
        animator.SetTrigger("Enemy shoot");
        audioSource.PlayOneShot(MachineShoot);
    }  


}

