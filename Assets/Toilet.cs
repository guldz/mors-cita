using UnityEngine;

public class Toilet : MonoBehaviour
{
    public Animator toiletAnimation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            toiletAnimation.SetTrigger("toiletShot");

        }
    }
}
