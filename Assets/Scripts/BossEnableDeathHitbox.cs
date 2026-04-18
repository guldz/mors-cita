using UnityEngine;

/// <summary>
/// StateMachineBehaviour attached to the "Shoot pc" state in The Boss.controller.
/// Enables the boss die1 hitbox the moment the Shoot pc animation begins,
/// so BossDeath can only be triggered after the PC shoot animation has played.
/// </summary>
public class BossEnableDeathHitbox : StateMachineBehaviour
{
    private const string BossDie1Path = "PC/boss die1";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform bossDie1 = animator.transform.Find(BossDie1Path);
        if (bossDie1 == null)
        {
            Debug.LogWarning($"BossEnableDeathHitbox: could not find '{BossDie1Path}' under {animator.name}.");
            return;
        }

        Collider2D col = bossDie1.GetComponent<Collider2D>();
        if (col != null)
            col.enabled = true;
    }
}
