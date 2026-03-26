using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// Finds the persistent player by tag on Awake and assigns it as the
/// CinemachineCamera tracking target. Handles cases where the player
/// carries over via DontDestroyOnLoad and is not part of the scene.
/// </summary>
[RequireComponent(typeof(CinemachineCamera))]
public class CinemachineCameraSetup : MonoBehaviour
{
    private const string PlayerTag = "Player";

    private void Awake()
    {
        CinemachineCamera cinemachineCamera = GetComponent<CinemachineCamera>();

        GameObject player = GameObject.FindWithTag(PlayerTag);
        if (player == null)
        {
            Debug.LogWarning("CinemachineCameraSetup: No GameObject with tag 'Player' found.");
            return;
        }

        cinemachineCamera.Target.TrackingTarget = player.transform;
        cinemachineCamera.Follow = player.transform;
    }
}
