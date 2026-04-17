using UnityEngine;

public class bossdeath2 : MonoBehaviour
{
    public Animator Dieanimation2;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Dieanimation2.SetTrigger("boss death2");


        }


    }
}



