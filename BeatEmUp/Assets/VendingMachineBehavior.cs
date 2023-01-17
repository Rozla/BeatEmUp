using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachineBehavior : MonoBehaviour
{

    [SerializeField] Animator machineAnimator;

    int hitTaken = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        machineAnimator.SetInteger("HITCOUNT", hitTaken);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && hitTaken < 3)
        {
            hitTaken += 1;
        }
    }
}
