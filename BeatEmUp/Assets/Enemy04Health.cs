using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy04Health : MonoBehaviour
{

    public float currentHealth;
    float maxHealth = 50f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        currentHealth -= 10f;

        if (currentHealth <= 0)
        {
            GetComponent<Enemy04Behavior>().isDead = true;
        }
        else
        {

            GetComponent<Enemy04Behavior>().isHurt = true;
        }

    }
}
