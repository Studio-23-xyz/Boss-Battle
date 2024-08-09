using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedBullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float lifeTime = 3f;
    public int damage = 20;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector2.left * bulletSpeed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

