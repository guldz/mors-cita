using UnityEngine;

public class Toilet : MonoBehaviour
{
    public Animator toiletAnimation;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip ToiletBroke;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            toiletAnimation.SetTrigger("toiletShot");
            audioSource.PlayOneShot(ToiletBroke);
        }
    }
}
