using System.Collections;
using UnityEngine;

public class StartCutscene : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;

    private const float PlayerSpawnDelay = 3f;

    private Animator vanAnimator;

    // Player components suppressed during the intro.
    private PlayerMovement playerMovement;
    private Collider2D playerCollider;
    private SpriteRenderer[] playerRenderers;

    private void Awake()
    {
        vanAnimator = GetComponent<Animator>();

        // Disable this object's collider so A* does not treat it as an obstacle on startup.
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
    }

    private void Start()
    {
        // Suppress the player visually and functionally WITHOUT deactivating the
        // GameObject, so enemies can still find it via FindGameObjectWithTag.
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            playerCollider = player.GetComponent<Collider2D>();
            playerRenderers = player.GetComponentsInChildren<SpriteRenderer>(true);

            if (playerMovement != null)
                playerMovement.enabled = false;

            if (playerCollider != null)
                playerCollider.enabled = false;

            foreach (SpriteRenderer sr in playerRenderers)
                sr.enabled = false;
        }

        StartCoroutine(VanIntroRoutine());
    }

    private IEnumerator VanIntroRoutine()
    {
        // Play the thrown-off-van animation on this GameObject's own Animator.
        if (vanAnimator != null)
            vanAnimator.SetTrigger("Start");

        // Wait for the animation to finish.
        yield return new WaitForSeconds(PlayerSpawnDelay);

        // Fade to black, restore the player and hide the van at peak black, then fade back in.
        if (SceneTransition.Instance != null)
        {
            yield return StartCoroutine(SceneTransition.Instance.FadeOutAndIn(() =>
            {
                RestorePlayer();
                gameObject.SetActive(false);
            }));
        }
        else
        {
            RestorePlayer();
            gameObject.SetActive(false);
        }
    }

    /// <summary>Re-enables all suppressed player components.</summary>
    private void RestorePlayer()
    {
        if (player == null) return;

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (playerCollider != null)
            playerCollider.enabled = true;

        foreach (SpriteRenderer sr in playerRenderers)
            sr.enabled = true;
    }
}
