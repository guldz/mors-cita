using TopDown.Shooting;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Cooldown")]
    [SerializeField] private float cooldown = 0.25f;
    private float cooldownTimer;

    [Header("Refrences")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firepoint;
    [SerializeField] private Animator muzzleFlashAnimator;

    //shoot point

    private void Update()
    {
        cooldownTimer += Time.deltaTime; 
    }

    private void Shoot()
    {
        if (cooldownTimer < cooldown) return;

        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation, null);
        bullet.GetComponent<Projectile>().ShootBullet(firepoint);

        muzzleFlashAnimator.SetTrigger("shoot");
        Debug.Log("shoot");
        cooldownTimer = 0; 
    }

    #region input 
    private void OnShoot()
    {
        Shoot(); 
    }
    #endregion
}
