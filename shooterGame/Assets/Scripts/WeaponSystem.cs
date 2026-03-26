using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [Header("Weapon Stats")]
    public float damage = 25f;
    public float range = 100f;
    public float fireRate = 10f; // Shots per second

    [Header("Ammo")]
    public int maxAmmo = 30;
    public int currentAmmo;
    public float reloadTime = 1.5f;

    [Header("References")]
    public Camera playerCamera;
    public ParticleSystem muzzleFlash;

    private float nextTimeToFire = 0f;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading)
            return;

        // Reload
        if (Input.GetKeyDown(KeyCode.R) || (Input.GetButton("Fire1") && currentAmmo <= 0))
        {
            StartCoroutine(Reload());
            return;
        }

        // Shoot (hold to fire)
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        currentAmmo--;

        if (muzzleFlash != null)
            muzzleFlash.Play();

        // Raycast from camera center
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, range))
        {
            // Try to damage whatever we hit
            PlayerHealth target = hit.collider.GetComponent<PlayerHealth>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            // Debug: show where shots land
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * hit.distance, Color.red, 0.5f);
        }
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
