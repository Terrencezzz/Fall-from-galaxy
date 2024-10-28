using UnityEngine;

public class GunScript : MonoBehaviour
{
    [Header("Gun Settings")]
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 0.1f;

    [Header("References")]
    public Camera fpsCam;
    public GameObject bulletHolePrefab;
    public GameObject bloodEffectPrefab;
    public AudioSource shootingSound;

    public GameObject autoGunPrefab;
    private GameObject currentGun;
    public PlayerController playerController;

    private float nextTimeToShoot = 0f;
    private int gunType = 1;  // 1 = Default gun, 2 = Auto gun
    public bool autoGunActive = false;

    void Update()
    {
        HandleShooting();
        if (autoGunActive) HandleWeaponSwitching();
    }

    void HandleShooting()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot && gunType == 2)
        {
            nextTimeToShoot = Time.time + fireRate;
            Shoot();
        }
    }

    void HandleWeaponSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchGun(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchGun(2);
        }
    }

    void SwitchGun(int type)
    {
        if (type != gunType)
        {
            if (currentGun != null)
                Destroy(currentGun);
                gunType = type;
                playerController.isHand = true;

            if (type == 2 && autoGunActive)
            {
                currentGun = Instantiate(autoGunPrefab, transform.position, transform.rotation);
                currentGun.transform.SetParent(transform);
                gunType = type;
                playerController.isHand = false;
            }

            
        }
    }

    void Shoot()
    {
        if (shootingSound != null)
            shootingSound.Play();

        int layerMask = ~LayerMask.GetMask("Player");

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, layerMask))
        {
            //Debug.Log($"Hit: {hit.transform.name}");

            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyDamage enemy = hit.collider.GetComponent<EnemyDamage>();
                enemy.TakeDamage();
                if (enemy != null)
                {
                    enemy.TakeDamage();
                    SpawnBloodEffect(hit);
                }
            }
            else
            {
                SpawnBulletHole(hit);
            }
        }
    }

    void SpawnBulletHole(RaycastHit hit)
    {
        if (bulletHolePrefab != null)
        {
            GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
            bulletHole.transform.rotation *= Quaternion.Euler(0, 0, 90);
            Destroy(bulletHole, 5f);  // Destroy after 10 seconds to avoid clutter
        }
    }

    void SpawnBloodEffect(RaycastHit hit)
    {
        if (bloodEffectPrefab != null)
        {
            GameObject bloodEffect = Instantiate(bloodEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(bloodEffect, 2f);  // Destroy after 2 seconds
        }
    }
}
