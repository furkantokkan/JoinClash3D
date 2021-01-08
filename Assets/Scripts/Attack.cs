using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damageAmount = 5;

   public void DealDamage()
    {
        Transform targetToDealDamage = GetComponent<AICombat>().target;
        if (targetToDealDamage != null)
        {
            Health targetHealth = targetToDealDamage.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damageAmount);
            }
        }

    }
}
