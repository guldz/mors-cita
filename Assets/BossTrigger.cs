using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public Animator BossAnimation;
   

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BossAnimation.SetTrigger("Trigger");
            
        }

      
    }
}