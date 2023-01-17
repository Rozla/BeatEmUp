using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachineBehavior : MonoBehaviour
{

    [SerializeField] Animator machineAnimator;
    [SerializeField] GameObject[] cansToSpawn;

    int hitTaken;

    // Start is called before the first frame update
    void Start()
    {
        hitTaken = 0;
    }

    // Update is called once per frame
    void Update()
    {
        machineAnimator.SetInteger("HITCOUNT", hitTaken);
        Debug.Log(hitTaken);
    }

    public void TakeDamage()
    {
        if (hitTaken < 3)
        {
            hitTaken += 1;
            Instantiate(cansToSpawn[Random.Range(0, cansToSpawn.Length)], transform.position, transform.rotation);
        }
    }

}
