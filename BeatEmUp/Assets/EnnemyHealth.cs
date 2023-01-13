using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyHealth : MonoBehaviour
{
    [SerializeField] float currentHealth, maxHealth = 50f;

    
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void TakeDamage()
    {
        if(currentHealth > 0)
        {
            currentHealth -= 10f;
        }
    }
}
