using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy04Health : MonoBehaviour
{
    [SerializeField] GameObject[] dieCollectPrefabs;

    public float currentHealth;
    float maxHealth = 50f;

    Vector3 offset0;
    Vector3 offset1;
    Vector3 offset2;
    Vector3 offset3;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        offset0 = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        offset1 = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        offset2 = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        offset3 = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
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
            Instantiate(dieCollectPrefabs[Random.Range(0, dieCollectPrefabs.Length)], transform.position + offset0, transform.rotation);
            Instantiate(dieCollectPrefabs[Random.Range(0, dieCollectPrefabs.Length)], transform.position + offset1, transform.rotation);
            Instantiate(dieCollectPrefabs[Random.Range(0, dieCollectPrefabs.Length)], transform.position + offset2, transform.rotation);
            Instantiate(dieCollectPrefabs[Random.Range(0, dieCollectPrefabs.Length)], transform.position + offset3, transform.rotation);
        }
        else
        {

            GetComponent<Enemy04Behavior>().isHurt = true;
        }

    }
}
