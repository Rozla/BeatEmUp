using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{

    [SerializeField] float maxHealth = 50f;
    [SerializeField] float currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
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
