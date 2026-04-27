using UnityEngine;

public class bossdeath2 : MonoBehaviour
{
    public Animator Dieanimation2;

    public static bool BossIsDead = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            
            BossIsDead = true;
            Dieanimation2.SetTrigger("boss death2");

        }


    }
}



