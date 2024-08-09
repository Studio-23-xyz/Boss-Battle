using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 100;
    private int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Player Health: " + currentHealth);
        if(currentHealth < 0)
        {
            Debug.Log("Dead!");
        }
    }
}
