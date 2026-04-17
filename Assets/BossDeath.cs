using UnityEngine;

public class BossDeath : MonoBehaviour
{
    public Animator Dieanimation;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Dieanimation.SetTrigger("boss death");
            
            
        }


    }


}
