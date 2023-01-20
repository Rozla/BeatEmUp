using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float walkEnemySpeed = 5f;

    [SerializeField] EnemyState currentState;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] GameObject graphics;
    [SerializeField] float playerDist = 1f;
    float currentHealth, maxHealth;
    int attackCount = 0;

    Coroutine attackCoroutine;

    //EnnemyHealth enemyHealthScript;

    public Transform player;

    public bool right, isOnRange, playerDetected;

    Vector2 enemyDir;



    public enum EnemyState
    {
        IDLE,
        WALK,
        JUMPUP,
        ATTACK01,
        ATTACK02,
        HURT,
        DEAD

    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = GetComponent<EnnemyHealth>().currentHealth;
        maxHealth = GetComponent<EnnemyHealth>().maxHealth;

        currentState = EnemyState.IDLE;
        OnStateEnter();
        rb2d = GetComponent<Rigidbody2D>();

       // attackCoroutine = StartCoroutine(Attack());

    }

    // Update is called once per frame
    void Update()
    {
        OnStateUpdate();

        if (enemyDir.x != 0)
        {
            right = enemyDir.x > 0;
            graphics.transform.rotation = right ? Quaternion.identity : Quaternion.Euler(0, 180f, 0);
        }
        if (!isOnRange && !playerDetected)
        {
            StopCoroutine(Attack());
        }

       

    }


    void OnStateEnter()
    {
        switch (currentState)
        {
            case EnemyState.IDLE:
                animator.SetBool("ISWALKING", false);
                rb2d.velocity = Vector2.zero;
                break;
            case EnemyState.WALK:
                animator.SetBool("ISWALKING", true);
                break;
            case EnemyState.ATTACK01:
                animator.SetTrigger("ATTACK");
                break;
            case EnemyState.HURT:
                animator.SetTrigger("HURT");
                break;
            case EnemyState.DEAD:
                animator.SetTrigger("DEAD");
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

                if (Vector2.Distance(transform.position, player.position) <= playerDist)
                {
                    isOnRange = true;
                    StartCoroutine(Attack());
                }
                if (Vector2.Distance(transform.position, player.position) > playerDist)
                {
                    isOnRange = false;

                }
                if (enemyDir != Vector2.zero)
                {
                    TransitionToState(EnemyState.WALK);
                }
                if (currentHealth <= 0)
                    TransitionToState(EnemyState.DEAD);

                break;
            case EnemyState.WALK:

                rb2d.velocity = enemyDir.normalized * walkEnemySpeed;

                if (Vector2.Distance(transform.position, player.position) <= playerDist)
                {
                    isOnRange = true;
                    TransitionToState(EnemyState.IDLE);
                }

                if (enemyDir == Vector2.zero)
                {
                    TransitionToState(EnemyState.IDLE);
                }
                if (currentHealth <= 0)
                    TransitionToState(EnemyState.DEAD);


                break;
            case EnemyState.ATTACK01:

                if (enemyDir != Vector2.zero)
                {
                    TransitionToState(EnemyState.WALK);
                }
                if (enemyDir == Vector2.zero)
                {
                    TransitionToState(EnemyState.IDLE);
                }

                break;
           
            case EnemyState.HURT:
                if (!isOnRange && enemyDir != Vector2.zero)
                {
                    TransitionToState(EnemyState.WALK);
                }
                if (enemyDir == Vector2.zero && !playerDetected && !isOnRange)
                {
                    TransitionToState(EnemyState.IDLE);
                }
                if (currentHealth <= 0)
                    TransitionToState(EnemyState.DEAD);
                break;
            case EnemyState.DEAD:

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
            case EnemyState.ATTACK01:
                break;
            case EnemyState.ATTACK02:
                break;
            case EnemyState.DEAD:
                break;
            case EnemyState.HURT:
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

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerDetected = true;
            enemyDir = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y) * walkEnemySpeed;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerDetected = false;
            enemyDir = Vector2.zero;
        }

    }
    IEnumerator Attack()
    {

        rb2d.velocity = Vector2.zero;
        yield return new WaitForSeconds(.75f);      
        animator.SetTrigger("ATTACK");
        yield return new WaitForSeconds(.25f);
        if (attackCount == 0)
        {
            attackCount = 1;
        }
        else
        {
            attackCount = 0;
        }
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
    }

}

