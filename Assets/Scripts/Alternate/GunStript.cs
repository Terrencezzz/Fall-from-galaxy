using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 0.1f;

    public Camera fpsCam;
    public GameObject bulletHolePrefab;

    private float nextTimeToShoot = 0f;
    public AudioSource shootingSound;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (shootingSound != null)
        {
            shootingSound.Play();
        }
        int layerMask = 1 << LayerMask.NameToLayer("Player");
        layerMask = ~layerMask;

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
