using UnityEngine;
using UnityEngine.InputSystem;

public class MafiaGunnerAnimation : MonoBehaviour
 
{
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
       
    }


    private void Update()
    {
        

        //if (Mouse.current.rightButton.wasPressedThisFrame)
        //{
        //animator.SetTrigger("Enemy shoot");
        //}
    }
    public void GunnerShoot_ani()
    {
        animator.SetTrigger("Enemy shoot");
    }  


}

