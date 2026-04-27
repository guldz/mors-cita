using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerBoom : MonoBehaviour
{
    [SerializeField] private Animator boomAnimator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!BossDeath.BossIsDead && !bossdeath2.BossIsDead) return;

        PlayerMovement movement = other.GetComponent<PlayerMovement>();
        if (movement != null)
            movement.FreezeForBoom();

        if (boomAnimator != null)
            boomAnimator.SetTrigger("Boom");

        StartCoroutine(LoadSceneAfterDelay(2f)); // match animation length
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("End");
    }
}
