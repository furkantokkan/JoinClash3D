using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 100;
    public int armor = 0;

    public void TakeDamage(int amount)
    {
        int totalDamage = amount - armor;

        if (totalDamage <= 0)
        {
            totalDamage = 1;
        }

        health -= totalDamage;

        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(this.gameObject);
    }
}
