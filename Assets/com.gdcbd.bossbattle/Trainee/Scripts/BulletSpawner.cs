using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject parrybulletPrefab;
    public GameObject whitebulletPrefab;
    public Transform firePoint;
    public float fireInterval = 1f;

    void Start()
    {
        StartCoroutine(FireBullets());
    }
    IEnumerator FireBullets()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireInterval);
            ShootBullet();
        }
    }
    void ShootBullet()
    {
        GameObject bulletPrefab = UnityEngine.Random.value > 0.5f ? parrybulletPrefab : whitebulletPrefab;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
