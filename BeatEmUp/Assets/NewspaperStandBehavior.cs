using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewspaperStandBehavior : MonoBehaviour
{
    [SerializeField] Sprite broken;
    [SerializeField] SpriteRenderer srStand;
    [SerializeField] LayerMask playerMask;


    private void Update()
    {
        if(Physics2D.OverlapCircle(transform.position, .3f, playerMask))
        {
            srStand.GetComponent<SpriteRenderer>().sprite = broken;
        }
    }
}
