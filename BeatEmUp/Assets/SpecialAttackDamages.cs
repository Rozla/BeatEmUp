using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackDamages : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnnemyHealth>().TakeDamage();
            collision.gameObject.GetComponent<EnnemyHealth>().TakeDamage();
            collision.gameObject.GetComponent<EnnemyHealth>().TakeDamage();
            collision.gameObject.GetComponent<EnnemyHealth>().TakeDamage();
        }

        if (collision.gameObject.tag == "Enemy04")
        {
            collision.gameObject.GetComponent<Enemy04Health>().TakeDamage();
            collision.gameObject.GetComponent<Enemy04Health>().TakeDamage();
            collision.gameObject.GetComponent<Enemy04Health>().TakeDamage();
            collision.gameObject.GetComponent<Enemy04Health>().TakeDamage();
        }
    }
}
