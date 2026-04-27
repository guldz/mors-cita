using UnityEngine;

public class playerBoom : MonoBehaviour
{
    public Animator PlayerBoom;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBoom.SetTrigger("Boom");

        }


    }
}

