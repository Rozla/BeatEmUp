using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] Rigidbody2D rb2d;

    Vector2 dirInput;


    public enum PlayerState
    {
        IDLE,
        WALK,
        RUN
    }


    // Start is called before the first frame update
    void Start()
    {
        //currentState = PlayerState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //void OnStateEnter()
    //{
    //    switch (currentState)
    //    {
    //        default:
    //            break;
    //    }
    //}

    //void OnStateEnter()
    //{

    //}
}
