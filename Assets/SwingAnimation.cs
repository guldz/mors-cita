
using UnityEngine;

public class SwingAnimation : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip BatSwing;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlaySwing()
    {
        animator.SetTrigger("Swing");
        audioSource.PlayOneShot(BatSwing);
    }
}

