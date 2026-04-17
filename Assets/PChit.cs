using UnityEngine;

public class PChit : MonoBehaviour
{
    public Animator PCanimation;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            PCanimation.SetTrigger("Pc shoot");

        }


    }
}
