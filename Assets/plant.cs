using UnityEngine;

public class plant : MonoBehaviour
{
    public Animator plantAnimation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            plantAnimation.SetTrigger("Plantshot");

        }
    }
}
