using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate1 = 0.1f;

    public Camera fpsCam;
    public GameObject bulletHolePrefab;
    public AudioSource shootingSound;

    private float nextTimeToShoot = 0f;


    public GameObject autoGun;
    public GameObject currentGun;

    bool autoGunActive = true;

    private int gunType = 1;

    bool aim = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot && gunType == 2)
        {
            nextTimeToShoot = Time.time + fireRate1;
            Shoot();
        }

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
            if (type == 1 && currentGun != null)
            {
                Destroy(currentGun);
            }
            else if (type == 2 && autoGunActive)
            {
                if (!currentGun) Destroy(currentGun);
                currentGun = Instantiate(autoGun, transform.position, transform.rotation);
                currentGun.transform.SetParent(this.transform);
            }
            gunType = type;
        }
    }

    void Shoot()
    {
        if (shootingSound != null)
        {
            shootingSound.Play();
        }

        int layerMask = ~LayerMask.GetMask("Player");

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, layerMask))
        {

            Debug.Log(hit.transform.name);

            if (hit.collider != null)
            {
                SpawnBulletHole(hit);
            }
        }
    }

    void SpawnBulletHole(RaycastHit hit)
    {
        // Spawn the bullet hole at the point of impact
        GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));

        // Adjust the bullet hole rotation based on the surface normal
        bulletHole.transform.rotation *= Quaternion.Euler(0, 0, 90);

        // Optionally, destroy the bullet hole after some time
        Destroy(bulletHole, 10f); // Destroy after 10 seconds to avoid clutter
    }
}
