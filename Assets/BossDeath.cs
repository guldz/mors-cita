using UnityEngine;

public class BossDeath : MonoBehaviour
{
    public Animator Dieanimation;

    /// <summary>Set to true the moment either boss die hitbox is triggered. Used by PlayerBoom to gate the explosion trigger.</summary>
    public static bool BossIsDead = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            BossIsDead = true;
            Dieanimation.SetTrigger("boss death");
        }
    }
}
