using UnityEngine;

public class enemyflash : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>(); 
    }

    public void PlayFlash()
    {
        animator.SetTrigger("eFlash");
    }
}
