using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBehavior : MonoBehaviour
{

    Rigidbody2D rb2d;
    public bool isHolded;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (isHolded)
        {
            rb2d.velocity = GetComponentInParent<Rigidbody2D>().velocity;

        }
    }
}
