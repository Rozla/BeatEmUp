using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMovement : MonoBehaviour
{
    [SerializeField] GameObject Boss;
    [SerializeField] float bossSpeed = 5f;
    Vector2 bossDir;
    [SerializeField] Animator animator;
    [SerializeField] BossState currentState;
    [SerializeField] GameObject player;

    private bool player_detected = false;

    Vector2 distance;

    Vector2 posPlayer;




    Rigidbody2D rb2d;

    public enum BossState
    {
        IDLE,
        WALK,
        ATTACK,
        SLAM,
        DEATH,
        TAUNT

    }



    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        distance = new Vector2(1f, 1f);
    }



    void Update()
    {


        if (Boss != null)
        {
            transform.LookAt(Boss.transform.position);
        }

        OnStateUpdate();

        posPlayer = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
    }

    void OnStateEnter()
    {
        switch (currentState)
        {
            case BossState.IDLE:
                break;
            case BossState.WALK:
                animator.SetBool("WALK", true);
                break;
            case BossState.ATTACK:
                break;
            case BossState.SLAM:
                break;
            case BossState.DEATH:
                break;
            case BossState.TAUNT:
                break;
            default:
                break;
        }
    }



    void OnStateUpdate()
    {
        switch (currentState)
        {
            case BossState.IDLE:
                if (true)
                {

                }
                break;
            case BossState.WALK:
                break;
            case BossState.ATTACK:
                break;
            case BossState.SLAM:
                break;
            case BossState.DEATH:
                break;
            case BossState.TAUNT:
                break;
            default:
                break;
        }


    }

    void OnStateExit()
    {
        switch (currentState)
        {
            case BossState.IDLE:
                break;
            case BossState.WALK:
                animator.SetBool("WALK", false);
                break;
            case BossState.ATTACK:
                break;
            case BossState.SLAM:
                break;
            case BossState.DEATH:
                break;
            case BossState.TAUNT:
                break;
            default:
                break;
        }
    }





    //void GetInputs()
    //{
    //    bossDir = new Vector2(Input.GetAxisRaw("HorizontalBoss"), Input.GetAxisRaw("VerticalBoss"));

    //    if (bossDir.magnitude != 0)
    //    {
    //        animator.SetBool("WALK", true);
    //    }
    //}

    void TransitionToState(BossState nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();

    }

    private void FixedUpdate()
    {
        rb2d.velocity = posPlayer.normalized * bossSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rb2d.velocity = Vector2.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (true)
        {

        }
        
    }
}
