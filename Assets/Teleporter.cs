using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    private const string TargetScene = "map2";
    private const string PlayerTag = "Player";

    [SerializeField] private string targetScene = TargetScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(PlayerTag)) return;

        if (SceneTransition.Instance != null)
            SceneTransition.Instance.LoadScene(targetScene);
        else
            SceneManager.LoadScene(targetScene);
    }
}
