using TMPro;
using TopDown.Shooting;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MachineGunController : MonoBehaviour
{
    [Header("Ammo")]
    [SerializeField] private int maxAmmo = 8;
    [SerializeField] private bool useAmmo = true;
    private int currentAmmo;

    [SerializeField] private AmmoCounter ammoCounter;

    [Header("Cooldown")]
    [SerializeField] private float cooldown = 0.25f;
    private float cooldownTimer;

    [Header("Refrences")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firepoint;
    [SerializeField] private Animator muzzleFlashAnimator;
    [SerializeField] private string muzzleFlashTrigger = "shoot";
    [SerializeField] private MuzzleFlash muzzleFlash;
    [SerializeField] private Animator animator;

    [Header("Camera Shake")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    private PlayerMovement ownerPlayer;

    private void Awake()
    {
        // Discard any muzzleFlash reference that doesn't belong to this GameObject's hierarchy.
        // This prevents a stale cross-reference (e.g. pointing at the player) from triggering
        // the wrong flash when this GunController is used on an enemy.
        if (muzzleFlash != null && !muzzleFlash.transform.IsChildOf(transform))
            muzzleFlash = null;
    }

    private void Start()
    {
        currentAmmo = maxAmmo;
        UpdateUI();

        // Non-null only when this controller belongs to the player's hierarchy.
        ownerPlayer = GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        // Reload input
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Reload();
            if (animator != null)
                animator.SetTrigger("Reload");
        }
    }

    /// <summary>Fires a bullet if the cooldown has elapsed.</summary>
    public void Shoot()
    {
        if (cooldownTimer < cooldown) return;

        if (useAmmo && currentAmmo <= 0) return;

        cooldownTimer = 0f;

        if (useAmmo)
            currentAmmo--;

        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
        bullet.GetComponent<Projectile>().ShootBullet(firepoint, gameObject.tag, ownerPlayer, impulseSource);

        muzzleFlashAnimator.SetTrigger(muzzleFlashTrigger);

        // Camera shake on shoot
        if (impulseSource != null)
            impulseSource.GenerateImpulse();

        // Only call PlayFlash if the MuzzleFlash belongs to this GameObject's own hierarchy.
        if (muzzleFlash != null && muzzleFlash.transform.IsChildOf(transform))
            muzzleFlash.PlayFlash();

        if (useAmmo)
            UpdateUI();
    }

    /// <summary>Restores ammo to max and updates the UI counter.</summary>
    public void Reload()
    {
        if (!useAmmo) return;

        currentAmmo = maxAmmo;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (ammoCounter == null) return;
        ammoCounter.UpdateAmmo(currentAmmo, maxAmmo, !useAmmo);
    }

    #region Input
    /// <summary>Called by the Input System when the shoot action is performed.</summary>
    public void OnShoot()
    {
        Shoot();
    }
    #endregion
}
