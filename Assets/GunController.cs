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


    public void Shoot()
    {
        if (cooldownTimer < cooldown) return;

        cooldownTimer = 0f;

        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
        bullet.GetComponent<Projectile>().ShootBullet(firepoint);

        GameObject enemybullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
       enemybullet.GetComponent<Projectile>().ShootBullet(firepoint);

        muzzleFlashAnimator.SetTrigger("shoot");
    }



    #region input 
    public void OnShoot()
    {
        Shoot(); 
    }
    #endregion
}
