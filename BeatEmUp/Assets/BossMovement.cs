using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMovement : MonoBehaviour
{
    [SerializeField] float bossSpeed = 5f;
    [SerializeField] Animator animator;
    [SerializeField] BossState currentState;
    [SerializeField] GameObject player;

    [SerializeField] GameObject bossGraphics;
    [SerializeField] SpriteRenderer bossSR;
    [SerializeField] LayerMask layerMask;

    [SerializeField] AnimationCurve jumpBoss;

    public float throwForce = 10;

    //float playerDist = 1f;

    //[Header("Jump Settings")]
    //[SerializeField] AnimationCurve jumpCurve;
    //[SerializeField] float jumpHeight = 3f;
    //[SerializeField] float jumpDuration = 3f;
    //float jumpTimer;

    //bool isJumping;

    public int NombreDeSec = 2;

    Rigidbody2D rb2d;

    public bool isDead;
    public bool pickUpObjectDetected;
    

    [SerializeField] float overlapRadius = 2.5f;

    bool attack;

    Vector2 posPlayer;


    float jumpTimer = 3f;
    [SerializeField] float jumpDuration = 1f;
    bool _isJumping;


    bool playerDetected;
    [SerializeField] bool playerOnRange;
    bool bossRight;
    Coroutine attackCor;
    int attackCount;
    float attackCoolDown;
    Collider2D col;

    public float jumpForce = 6f;
    public enum BossState
    {
        IDLE,
        WALK,
        ATTACK,
        SLAM,
        DEATH,
        HURT,
        TAUNT

    }

    Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentState = BossState.IDLE;
        rb2d = GetComponent<Rigidbody2D>();
        //attackCor = StartCoroutine(BossAttack());
    }


    void Update()
    {

        //transform.LookAt(Boss.transform.position);


        OnStateUpdate();

        posPlayer = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);

        if (rb2d.velocity.x != 0)
        {
            bossRight = rb2d.velocity.x > 0;

            bossGraphics.transform.rotation = bossRight ? Quaternion.identity : Quaternion.Euler(0, 180f, 0);

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpTimer = 0f;
        }

        Jump();

        Physics2D.OverlapCircle(transform.position, overlapRadius, layerMask);


        Debug.Log(attackCount);

        attackCoolDown += Time.deltaTime;

        if (isDead)
        {
            BossIsDead();
        }

    }

    private void Jump()
    {
        if (jumpTimer < jumpDuration)
        {
            jumpTimer += Time.deltaTime;
            float y = jumpBoss.Evaluate(jumpTimer / jumpDuration);
            bossGraphics.transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
        }
    }

    void OnStateEnter()
    {
        switch (currentState)
        {
            case BossState.IDLE:
                rb2d.velocity = Vector2.zero;
                animator.SetBool("WALK", false);

                break;
            case BossState.WALK:
                animator.SetBool("WALK", true);

                break;
            case BossState.ATTACK:
                animator.SetTrigger("ATTACK");
                break;
            case BossState.SLAM:
                break;
            case BossState.HURT:
                //animator.SetTrigger("HURT");
                break;
            case BossState.DEATH:
                animator.SetTrigger("DEATH");
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

                if (playerDetected && !playerOnRange)
                {
                    TransitionToState(BossState.WALK);
                }

                if (playerOnRange)
                {
                    attackCoolDown = 0f;
                    attackCount = attackCount == 0 ? 1 : 0;
                    StartCoroutine(BossAttack());
                }

                if (GetComponent<BossHealth>().isHurt == true)
                {
                    TransitionToState(BossState.HURT);
                }

                break;
            case BossState.WALK:



                if (!playerDetected || playerOnRange)
                {
                    TransitionToState(BossState.IDLE);
                }

                if (GetComponent<BossHealth>().isHurt == true)
                {
                    TransitionToState(BossState.HURT);
                }

                break;
            case BossState.ATTACK:

                if (attackCount == 0 && attackCoolDown > 1.5f)
                {
                    animator.SetInteger("ATTACKCOUNT", 1);
                    TransitionToState(BossState.IDLE);
                }

                if (attackCount == 1 && attackCoolDown > 1.5f)
                {
                    animator.SetInteger("ATTACKCOUNT", 0);
                    TransitionToState(BossState.IDLE);
                }


                if (GetComponent<BossHealth>().isHurt == true)
                {
                    TransitionToState(BossState.HURT);
                }

                break;
            case BossState.SLAM:
                break;
            case BossState.HURT:
                rb2d.velocity = Vector2.zero;



                break;
            case BossState.DEATH:
                break;
            case BossState.TAUNT:

                if (GetComponent<BossHealth>().isHurt == true)
                {
                    TransitionToState(BossState.HURT);
                }

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
                break;
            case BossState.ATTACK:
                break;
            case BossState.SLAM:
                break;
            case BossState.HURT:
                break;
            case BossState.DEATH:
                break;
            case BossState.TAUNT:
                break;
            default:
                break;
        }
    }




    void TransitionToState(BossState nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();

    }

    private void FixedUpdate()
    {
        if (playerDetected)
        {
            rb2d.velocity = posPlayer.normalized * bossSpeed;

        }

       
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerDetected = true;
        }

        if (collision.gameObject.tag == "PICKUP")
        {
            pickUpObjectDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerDetected = false;
        }

        if (collision.gameObject.tag == "PICKUP")
        {
            pickUpObjectDetected = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerOnRange = true;
            TransitionToState(BossState.IDLE);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerOnRange = false;
        }
    }





    IEnumerator BossAttack()
    {

        TransitionToState(BossState.ATTACK);
        yield return new WaitForSeconds(.75f);
    }


    public void BossIsDead()
    {
        TransitionToState(BossState.DEATH);
    }

}

