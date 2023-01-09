using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_MOVEMENT : MonoBehaviour
{
    [SerializeField] GameObject Boss;
    [SerializeField] float bossSpeed = 5f;
    Vector2 bossDir;
    [SerializeField] Animator animator;
    [SerializeField] BossState currentState;

    Rigidbody2D rb2d;

    public enum BossState
    {
        IDLE,
        WALK
    }
     




    // Start is called before the first frame update
     void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame
    void Update()
    {

        if (Boss != null)
        {
            transform.LookAt(Boss.transform.position);
        }

        


    }

    void OnStateExit()
    {
        switch (currentState)
        {
            case BossState.IDLE:
                break;
            case BossState.WALK:
                break;
            default:
                break;
        }
    }

    void OnStateEnter()
    {
        switch (currentState)
        {
            case BossState.IDLE:
                break;

            case BossState.WALK:

            default:
                break;
        }
    }



    void OnStateUpdate()
    {
        switch (currentState)
        {
            case BossState.IDLE:
                if (bossDir != Vector2.zero)
                {
                    TransitionToState(BossState.WALK);
                }
                break;
            case BossState.WALK:
                break;
            default:
                break;
        }
    }

    void GetInputs()
    {
        bossDir = new Vector2(Input.GetAxisRaw("Horizontal_Boss"), Input.GetAxisRaw("Vertical_Boss"));

        if(bossDir.magnitude != 0)
        {
            animator.SetBool("WALK", true);
        }
    }

    void TransitionToState(BossState nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();

    }
}