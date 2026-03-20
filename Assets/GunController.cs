using TMPro;
using TopDown.Shooting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GunController : MonoBehaviour
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
    [SerializeField] private MuzzleFlash muzzleFlash;



    //shoot point

    private void Start()
    {
        currentAmmo = maxAmmo;
        UpdateUI();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        // Reload input
        if (Keyboard.current.rKey.isPressed)
        {
            Reload();
        }
    }


    public void Shoot()
    {
        if (cooldownTimer < cooldown) return;

        if (useAmmo && currentAmmo <= 0) return;

        cooldownTimer = 0f;

        if (useAmmo)
            currentAmmo--;

        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
        bullet.GetComponent<Projectile>().ShootBullet(firepoint, gameObject.tag);

        muzzleFlashAnimator.SetTrigger("shoot");

        //  ONLY plays when a real shot happens
        if (muzzleFlash != null)
            muzzleFlash.PlayFlash();

        if (useAmmo)
            UpdateUI();
    }

    public void Reload()
    {
        if (!useAmmo) return;

        currentAmmo = maxAmmo;
        UpdateUI();
    }

    private void UpdateUI()
    {
        ammoCounter.UpdateAmmo(currentAmmo, maxAmmo, !useAmmo);
    }




    #region input 
    public void OnShoot()
    {
        Shoot(); 
    }
    #endregion
}
