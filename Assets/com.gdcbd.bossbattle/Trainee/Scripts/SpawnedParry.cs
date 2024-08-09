using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedParry : MonoBehaviour,  IInteractable
{
    public float bulletSpeed = 10f;
    public float lifeTime = 3f;
    public int damage = 10;
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
    public void interact(GameObject player)
    {
        PlayerActionController playerController = player.GetComponent<PlayerActionController>();
        if (playerController != null)
        {
            playerController.TouchedParry();
        }
    }
}
