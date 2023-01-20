using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLightBehavior : MonoBehaviour
{
    [SerializeField] LayerMask playerMask;
    [SerializeField] Animator lightAnimator;
    BoxCollider2D bc2d;

    int hitTaken = 0;

    float t;

    // Start is called before the first frame update
    void Start()
    {
        bc2d = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        

        if(Physics2D.OverlapCircle(bc2d.bounds.center, .1f, playerMask) && hitTaken == 0)
        {
            lightAnimator.SetTrigger("FIRSTHIT");
            hitTaken = 1;
            t = 0;
        }

        if (Physics2D.OverlapCircle(bc2d.bounds.center, .1f, playerMask) && hitTaken >= 1 && t > .5f)
        {
            lightAnimator.SetTrigger("SECONDHIT");
        }

        Debug.Log(hitTaken);

    }
}
