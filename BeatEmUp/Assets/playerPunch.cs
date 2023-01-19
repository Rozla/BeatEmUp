using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPunch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Boss")
        {
            collision.gameObject.GetComponent<BossHealth>().TakeDamage();
        }

        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnnemyHealth>().TakeDamage();
        }

        if(collision.gameObject.tag == "VENDINGMACHINE")
        {
            collision.gameObject.GetComponent<VendingMachineBehavior>().TakeDamage();
        }

        if (collision.gameObject.tag == "Enemy04")
        {
            collision.gameObject.GetComponent<Enemy04Health>().TakeDamage();
        }

    }
}
