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

    Coroutine attackCoroutine;

    EnnemyHealth enemyHealthScript;

    public Transform player;

    public bool right, isOnRange;

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

        currentState = EnemyState.IDLE;
        OnStateEnter();
        rb2d = GetComponent<Rigidbody2D>();
        enemyHealthScript = GetComponent<EnnemyHealth>();

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

        float enemyCurrentHealth = enemyHealthScript.currentHealth;

        if (!isOnRange)
        {

        }
    }


    void OnStateEnter()
    {
        switch (currentState)
        {
            case EnemyState.IDLE:
                animator.SetBool("IsIdle", true);
                animator.SetBool("WALK_ENEMY01", false);
                rb2d.velocity = Vector2.zero;

                break;
            case EnemyState.WALK:
                if (enemyDir.magnitude != 0)
                {
                    animator.SetBool("WALK_ENEMY01", true);
                    animator.SetBool("IsIdle", false);
                }

                break;
            case EnemyState.ATTACK01:
                animator.SetTrigger("IsAttacking");
                animator.SetBool("IsIdle", false);
                animator.SetBool("WALK_ENEMY01", false);
                break;

            case EnemyState.ATTACK02:
                animator.SetTrigger("IsAttacking02");
                animator.SetBool("IsIdle", false);
                animator.SetBool("WALK_ENEMY01", false);
                break;

            case EnemyState.HURT:
                animator.SetTrigger("HURT");
                animator.SetBool("IsIdle", false);
                animator.SetBool("WALK_ENEMY01", false);
                break;
            case EnemyState.DEAD:
                animator.SetTrigger("IsDead");
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
                // CHANGEMENT
                if (Vector2.Distance(transform.position, player.position) <= playerDist)
                {
                    isOnRange = true;                  
                    
                    attackCoroutine = StartCoroutine(Attack());
                }
                if (Vector2.Distance(transform.position, player.position) > playerDist)
                {
                    isOnRange = false;
                    TransitionToState(EnemyState.WALK);
                    StopCoroutine(Attack());
                }
                // if la vie de l'ennemie descend 
                // TransitionToState(EnemyState.HURT);
                // SI LA VIE DE MON ENNEMIE EST A ZERO 
               //TransitionToState(EnemyState.DEAD);

                break;
            case EnemyState.WALK:

                rb2d.velocity = enemyDir.normalized * walkEnemySpeed;
                if (Vector2.Distance(transform.position, player.position) <= playerDist)
                {
                    isOnRange = true;
                    TransitionToState(EnemyState.IDLE);
                    attackCoroutine = StartCoroutine(Attack());
                }

                if (enemyDir == Vector2.zero)
                {
                    TransitionToState(EnemyState.IDLE);
                }
                // if la vie de l'ennemie descend 
                //TransitionToState(EnemyState.HURT);
                // SI LA VIE DE MON ENNEMIE EST A ZERO 
                //TransitionToState(EnemyState.DEAD);

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
            case EnemyState.ATTACK02:
                if (enemyDir != Vector2.zero)
                {
                    TransitionToState(EnemyState.WALK);
                }
                if (enemyDir == Vector2.zero)
                {
                    TransitionToState(EnemyState.IDLE);
                }
                break;
            // if la vie de l'ennemie descend 
            // TransitionToState(EnemyState.HURT);
            // SI LA VIE DE MON ENNEMIE EST A ZERO 
            //TransitionToState(EnemyState.DEAD);

            case EnemyState.HURT:
                if (enemyDir != Vector2.zero)
                {
                    TransitionToState(EnemyState.WALK);
                }
                if (enemyDir == Vector2.zero)
                {
                    TransitionToState(EnemyState.IDLE);
                }
                break;
            // SI LA VIE DE MON ENNEMIE EST A ZERO 
            //TransitionToState(EnemyState.DEAD);

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
                //if (enemyDir.magnitude != 0)
                //{
                //    animator.SetBool("WALK_ENEMY01", false);
                //}
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
           
            enemyDir = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y) * walkEnemySpeed;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           
            enemyDir = Vector2.zero;
        }

    }
    private IEnumerator Attack()
    {
        while (isOnRange)
        {
            animator.SetBool("IsIdle", true);
            yield return new WaitForSeconds(1);
            animator.SetBool("IsIdle", false);
            animator.SetTrigger("IsAttacking");
            animator.SetTrigger("IsAttacking02");
            yield return new WaitForSeconds(1);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
           
        }
    }

}

