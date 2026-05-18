using UnityEngine;

public class ExplodingPlant : MonoBehaviour

{
    public Animator BoomPlantAnimation;
    public Collider2D PlantCollidor;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip explodingPlant;


    public void DisableCollider()
    {
        PlantCollidor.enabled = false;
    }
    private void Start()
    {

        PlantCollidor.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Bullet"))
        {
            BoomPlantAnimation.SetTrigger("PlantExplode");
            PlantCollidor.enabled = false;
            audioSource.PlayOneShot(explodingPlant);
        }
    }
}

