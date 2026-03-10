using UnityEngine;
using UnityEngine.InputSystem;

public class MafiaGunnerAnimation : MonoBehaviour
 
{
    private Animator animator;
    private ShooterEnemy enemy;
    
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
    }  


}

