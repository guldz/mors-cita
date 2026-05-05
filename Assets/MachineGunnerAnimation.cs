
using UnityEngine;
using UnityEngine.InputSystem;

public class MachineGunnerAnimation : MonoBehaviour

{
    private Animator animator;
    private MachineGunEnemy enemy;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip MachineShoot;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<MachineGunEnemy>();
    }

    private void Update()
    {
        if (animator != null)
        {

        }
    }
    public void GunnerShoot_ani()
    {
        animator.SetTrigger("Mshoot");

        if (audioSource.isPlaying == false)
        {
            audioSource.clip = MachineShoot;
            audioSource.Play();
        }

        if (enemy.isFiring == false)
        {
            audioSource.Stop();
        }


    }


}
