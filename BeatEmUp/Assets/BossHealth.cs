using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{

    [SerializeField] float maxHealth = 50f;
    [SerializeField] float currentHealth;

    
    float t = 0f;
    float hurtTimer = 0f;
    public bool isDead;
    public bool isHurt;


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
            isDead = true;
            t += Time.deltaTime;
            if (t > 1.5f)
            {

                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage()
    {
        if (currentHealth > 0)
        {
            isHurt = true;
            currentHealth -= 10f;
        }
    }
}
