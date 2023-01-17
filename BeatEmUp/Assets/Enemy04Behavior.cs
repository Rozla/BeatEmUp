using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy04Behavior : MonoBehaviour
{
    [SerializeField] EnemyState currentState;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] SpriteRenderer srEnemy;
    [SerializeField] float enemySpeed = 3f;
    [SerializeField] Animator enemyAnimator;
    [SerializeField] float overlapRadius = 2.5f;
    [SerializeField] LayerMask overlapLayerMask;
    [SerializeField] GameObject enemyGraphics;


    Vector2 dirEnemy;

    [SerializeField] bool playerDetected;
    [SerializeField] bool playerOnRange;

    bool isRight;


    public enum EnemyState { 
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
        srEnemy = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (dirEnemy != Vector2.zero)
        {
            isRight = dirEnemy.x > 0;

            enemyGraphics.transform.rotation = isRight ? Quaternion.identity : Quaternion.Euler(0, 180f, 0);
        }

        OnStateUpdate();

        PlayerDetection();



    }

    IEnumerator Attack()
    {
        
        TransitionToState(EnemyState.IDLE);
        yield return new WaitForSeconds(1f);
        TransitionToState(EnemyState.ATTACK);
        playerOnRange = false;
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
                break;
            case EnemyState.ATTACK:
                enemyAnimator.SetTrigger("ATTACK");
                break;
            case EnemyState.DEAD:
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

                if (playerDetected && !playerOnRange)
                {
                    TransitionToState(EnemyState.WALK);
                }

                break;
            case EnemyState.WALK:
                rb2d.velocity = dirEnemy.normalized * enemySpeed;

                if (!playerDetected)
                {
                    TransitionToState(EnemyState.IDLE);
                }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerOnRange = true;
            StartCoroutine(Attack());
        }
    }
}
