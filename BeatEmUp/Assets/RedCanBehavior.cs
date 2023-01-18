using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCanBehavior : MonoBehaviour
{

    float t = 5f;

    float moveTimer = 0f;

    float dirX;
    float dirY;

    private void Start()
    {
        dirX = Random.Range(9f, 11f);
        dirY = Random.Range(-5f, 5f);
    }

    private void Update()
    {

        moveTimer += Time.deltaTime;

        if (moveTimer > .2f && moveTimer < .5f)
        {
            transform.Translate(Vector3.down * dirX * Time.deltaTime);
            transform.Translate(Vector3.right * dirY * Time.deltaTime);
        }


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
