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

    public int NombreDeSec = 2;



    bool attack;

    Vector2 distance;

    Vector2 posPlayer;
    private void Start()
    {
        StartCoroutine("wait");
        rb2d = GetComponent<Rigidbody2D>();
        distance = new Vector2(1f, 1f);
    }

    IEnumerable wait()
    {
        yield return new WaitForSeconds(NombreDeSec);
        
    }




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

    

    void Update()
    {


        if (Boss != null)
        {
            transform.LookAt(Boss.transform.position);
        }

        OnStateUpdate();

        posPlayer = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);

        if (bossDir != posPlayer)
        {
            attack = false;
            StartCoroutine("wait");
        }
    }

    void OnStateEnter()
    {
        switch (currentState)
        {
            case BossState.IDLE:

                animator.SetBool("WALK", false);
                rb2d.velocity = Vector2.zero;
                break;
            case BossState.WALK:
                animator.SetBool("WALK", true);
                break;
            case BossState.ATTACK:
                animator.SetTrigger("ATTACK");
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
                if (bossDir != Vector2.zero)
                {

                    TransitionToState(BossState.WALK);

                }

                if (attack == true)
                {
                    TransitionToState(BossState.ATTACK);
                }
                break;
            case BossState.WALK:

                if (bossDir == Vector2.zero)
                {
                    TransitionToState(BossState.IDLE);
                }

                if (attack == true)
                {
                    TransitionToState(BossState.ATTACK);
                }
                break;
            case BossState.ATTACK:

                if (bossDir != Vector2.zero)
                {
                    TransitionToState(BossState.WALK);
                }

                if (bossDir == Vector2.zero)
                {
                    TransitionToState(BossState.IDLE);
                }
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            attack = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            attack = false;
        }
    }
}

