using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyHealth : MonoBehaviour
{
    [SerializeField] public float currentHealth, maxHealth = 50f;
    [SerializeField] Animator animator;
    
    [SerializeField] GameObject [] lootRecordPrefab;
    Vector3 offset0;
    Vector3 offset1;
    Vector3 offset2;
    Vector3 offset3;


    void Start()
    {
        currentHealth = maxHealth;
        offset0 = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        offset1 = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        offset2 = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        offset3 = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
    }

    void Update()
    {
        if(currentHealth <= 0 )
        {
            animator.SetTrigger("DEAD");
            Instantiate(lootRecordPrefab[Random.Range(0, lootRecordPrefab.Length)], transform.position + offset0, transform.rotation);
            Instantiate(lootRecordPrefab[Random.Range(0, lootRecordPrefab.Length)], transform.position + offset1, transform.rotation);
            Instantiate(lootRecordPrefab[Random.Range(0, lootRecordPrefab.Length)], transform.position + offset2, transform.rotation);
            Instantiate(lootRecordPrefab[Random.Range(0, lootRecordPrefab.Length)], transform.position + offset3, transform.rotation);

            Destroy(gameObject);

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
