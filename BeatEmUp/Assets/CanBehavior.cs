using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBehavior : MonoBehaviour
{


    Rigidbody2D rb2d;
    BoxCollider2D bc2d;

    float throwHeight = 1f;
    float throwDistance = 5f;

    float t;

    float rotationFix;

    [SerializeField] float isRight;

    [HideInInspector]
    public bool isHolded;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if(isHolded && transform.parent != null)
        {
            isRight = transform.parent.rotation.y;
        }

        if(isRight == 1)
        {
            rotationFix = -1f;
        }

        if (isRight == 0)
        {
            rotationFix = 1f;
        }

        Throw();
    }

    private void Throw()
    {
        if (isHolded && Input.GetButtonDown("Attack1"))
        {
            t += Time.deltaTime;
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            float throwForce = Mathf.Sqrt(-2 * Physics2D.gravity.y * throwHeight * rb2d.mass);
            rb2d.AddForce(new Vector2(throwDistance * rotationFix, throwForce), ForceMode2D.Impulse);
            rb2d.drag = -5f;
            transform.parent = null;
            if (t >= .5f)
            {
                t = 0f;
                rb2d.bodyType = RigidbodyType2D.Kinematic;
                bc2d.isTrigger = false;
                isHolded = false;
            }
        }
    }
}
