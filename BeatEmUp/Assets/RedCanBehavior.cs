using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCanBehavior : MonoBehaviour
{

    float t = 5f;

    private void Update()
    {
        t -= Time.deltaTime;

        if(t <= 0f)
        {
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeHealth();
            Destroy(gameObject);
        }
    }
}