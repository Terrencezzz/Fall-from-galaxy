using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate1 = 0.1f;

    public Camera fpsCam;
    public GameObject bulletHolePrefab;
    public AudioSource shootingSound;

    private float nextTimeToShoot = 0f;

    public GameObject autoGunPrefab;
    private GameObject currentGun;

    private int gunType = 1;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot && gunType == 2)
        {
            nextTimeToShoot = Time.time + fireRate1;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchGun(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchGun(2);
    }

    void SwitchGun(int type)
    {
        if (type != gunType)
        {
            if (currentGun != null)
                Destroy(currentGun);

            if (type == 2 && autoGunPrefab != null)
            {
                currentGun = Instantiate(autoGunPrefab, transform.position, transform.rotation);
                currentGun.transform.SetParent(this.transform);
            }
            gunType = type;
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
            //Debug.Log(hit.transform.name);
            if (hit.collider != null)
            {
                EnemyDamage enemy = hit.collider.GetComponent<EnemyDamage>();
                if (enemy != null)
                    enemy.TakeDamage();
                else
                    SpawnBulletHole(hit);
            }
        }
    }

    void SpawnBulletHole(RaycastHit hit)
    {
        GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
        bulletHole.transform.rotation *= Quaternion.Euler(0, 0, 90);
        Destroy(bulletHole, 5f);
    }
}
