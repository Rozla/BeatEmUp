using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy04Behavior : MonoBehaviour
{
    [SerializeField] EnemyState currentState;
    Rigidbody2D rb2d;
    [SerializeField] float enemySpeed = 3f;
    [SerializeField] Animator enemyAnimator;
    [SerializeField] float overlapRadius = 2.5f;
    [SerializeField] LayerMask overlapLayerMask;
    [SerializeField] GameObject enemyGraphics;
    [SerializeField] Transform playerTransform;
    [SerializeField] SpriteRenderer enemySR;
    CapsuleCollider2D cc2d;

    float playerDist = 1f;
    float currentDist;

    bool isDying;

    int attackCount = 0;

    Vector2 dirEnemy;

    [SerializeField] bool playerDetected;
    [SerializeField] bool playerOnRange;

    bool isRight;
    bool isAttacking;

    public bool isDead;
    public bool isHurt;

    bool canDetect;

    Coroutine attackCor;

    public enum EnemyState
    {
        IDLE,
        WALK,
        HURT,
        ATTACK,
        DEAD
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.IDLE;
        rb2d = GetComponent<Rigidbody2D>();
        cc2d = GetComponent<CapsuleCollider2D>();
        attackCor = StartCoroutine(Attack());
        canDetect = true;
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(canDetect);


        if (dirEnemy != Vector2.zero)
        {
            isRight = dirEnemy.x > 0;

            enemyGraphics.transform.rotation = isRight ? Quaternion.identity : Quaternion.Euler(0, 180f, 0);
        }

        OnStateUpdate();

        if (canDetect)
        {
            PlayerDetection();
        }


        currentDist = Vector2.Distance(transform.position, playerTransform.position);
        playerOnRange = currentDist <= playerDist;
        enemyAnimator.SetInteger("ATTACKCOUNT", attackCount);


        if (isHurt)
        {
            canDetect = false;
            StartCoroutine(IsHurt());
        }

        if (isDead)
        {
            canDetect = false;
            StartCoroutine(IsDead());
        }
    }

    IEnumerator Attack()
    {
        canDetect = false;
        isAttacking = false;
        yield return new WaitForSeconds(.75f);
        enemyAnimator.SetTrigger("ATTACK");
        yield return new WaitForSeconds(2f);

        Collider2D collision = Physics2D.OverlapCircle(transform.position, 1f, overlapLayerMask);

        if(collision.gameObject != null && collision.gameObject.GetComponent<PlayerMovement>().isInvincible == false)
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage();

        }


        if (attackCount == 0)
        {
            attackCount = 1;
        }
        else
        {
            attackCount = 0;
        }

        Debug.Log("Coup de poing");
        
        canDetect = true;
        TransitionToState(EnemyState.IDLE);

    }

    IEnumerator IsHurt()
    {
        StopCoroutine(attackCor);
        rb2d.velocity = Vector3.zero;
        isHurt = false;
        TransitionToState(EnemyState.HURT);
        enemySR.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(.5f);
        enemySR.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        canDetect = true;
        TransitionToState(EnemyState.IDLE);
    }

    IEnumerator IsDead()
    {
        StopCoroutine(attackCor);
        isDying = true;
        rb2d.velocity = Vector2.zero;
        isDead = false;
        canDetect = false;
        TransitionToState(EnemyState.DEAD);
        yield return new WaitForSeconds(.2f);
        enemySR.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
        yield return new WaitForSeconds(.2f);
        enemySR.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(.2f);
        enemySR.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
        yield return new WaitForSeconds(.2f);
        enemySR.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }
    private void PlayerDetection()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, overlapRadius, overlapLayerMask);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Player")
            {
                dirEnemy = new Vector2(collider.transform.position.x - transform.position.x, collider.transform.position.y - transform.position.y);
                playerDetected = true;
            }

        }

        if (!Physics2D.OverlapCircle(transform.position, overlapRadius, overlapLayerMask))
        {
            playerDetected = false;
        }
    }

    void OnStateEnter()
    {
        switch (currentState)
        {
            case EnemyState.IDLE:
                rb2d.velocity = Vector2.zero;
                enemyAnimator.SetBool("ISWALKING", false);
                break;
            case EnemyState.WALK:
                enemyAnimator.SetBool("ISWALKING", true);
                break;
            case EnemyState.HURT:
                enemyAnimator.SetTrigger("HURT");
                break;
            case EnemyState.ATTACK:
                rb2d.velocity = Vector2.zero;
                isAttacking = true;
                StartCoroutine(Attack());
                break;
            case EnemyState.DEAD:
                enemyAnimator.SetTrigger("DEAD");
                break;
            default:
                break;
        }
    }

    void OnStateUpdate()
    {
        switch (currentState)
        {
            case EnemyState.IDLE:

                if (playerDetected && !playerOnRange && !isAttacking && !isHurt && !isDead &&!isDying)
                {
                    TransitionToState(EnemyState.WALK);
                }

                if (playerDetected && playerOnRange && !isDying)
                {
                    TransitionToState(EnemyState.ATTACK);
                }


                break;
            case EnemyState.WALK:
                rb2d.velocity = dirEnemy.normalized * enemySpeed;

                if (!playerDetected || playerOnRange)
                {
                    TransitionToState(EnemyState.IDLE);
                }

                if (isAttacking && !isDying)
                {
                    TransitionToState(EnemyState.ATTACK);
                }


                break;
            case EnemyState.HURT:
                rb2d.velocity = Vector2.zero;
                break;
            case EnemyState.ATTACK:
                rb2d.velocity = Vector2.zero;
                break;
            case EnemyState.DEAD:
                rb2d.velocity = Vector2.zero;
                cc2d.isTrigger = true;
                
                break;
            default:
                break;
        }
    }

    void OnStateExit()
    {
        switch (currentState)
        {
            case EnemyState.IDLE:
                break;
            case EnemyState.WALK:
                break;
            case EnemyState.HURT:
                break;
            case EnemyState.ATTACK:
                break;
            case EnemyState.DEAD:
                break;
            default:
                break;
        }
    }

    void TransitionToState(EnemyState nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, overlapRadius);
    }


}
