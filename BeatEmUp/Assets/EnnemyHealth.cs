using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyHealth : MonoBehaviour
{
    [SerializeField] public float currentHealth, maxHealth = 50f;
    [SerializeField] Animator animator;
   
    [SerializeField] GameObject lootRecordPrefab;
    [SerializeField] float spawnRadius = 3f;
    
    int currentLootCount = 0;
    int maxLootCount = 1;


    void Start()
    {
        currentHealth = maxHealth;
       ;
    }

    void Update()
    {
        if(currentHealth <= 0 )
        {
           
            animator.SetTrigger("IsDead");
            Destroy(gameObject);
           
        }
        if(currentLootCount < maxLootCount && currentHealth <= 0)
        {
            //Random.insideUnitCircle pour générer un vecteur aléatoire dans un cercle unitaire
            // Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            Instantiate(lootRecordPrefab, transform.position, transform.rotation);
            //currentLootCount++;
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
