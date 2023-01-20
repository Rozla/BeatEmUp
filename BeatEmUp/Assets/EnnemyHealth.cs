using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyHealth : MonoBehaviour
{
    [SerializeField] public float currentHealth, maxHealth = 50f;
    [SerializeField] Animator animator;
    [SerializeField] GameObject shadow;
    [SerializeField] GameObject lootRecordPrefab;
    [SerializeField] float spawnRadius = 1.5f;
    
    int currentLootCount = 0;
    int maxLootCount = 3;


    void Start()
    {
        currentHealth = maxHealth;
       
    }

    void Update()
    {
        if(currentHealth <= 0 )
        {
            animator.SetTrigger("DEAD");
            Destroy(shadow);
            if (currentLootCount <= maxLootCount)
            {
                Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
                Instantiate(lootRecordPrefab, spawnPosition, Quaternion.identity);
                currentLootCount ++;
            }
        }
    }
    public void TakeDamage()
    {
        if(currentHealth > 0)
        {
            currentHealth -= 10f;
            animator.SetTrigger("HURT");
        }
       
    }
    
}
