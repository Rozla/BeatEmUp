using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float walkEnemySpeed = 5f;

    [SerializeField] EnemyState currentState;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] GameObject graphics;
    [SerializeField] float jumpEnemyDuration = 3f;
    [SerializeField] float playerDist = 1f;

    Coroutine attackCoroutine;


    public Transform player;

    public bool right, isOnRange;


    Vector2 enemyDir;
    public enum EnemyState
    {
        IDLE,
        WALK,
        JUMPUP,
        ATTACK01,
        DEAD

    }





    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.IDLE;
        OnStateEnter();
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
            case EnemyState.JUMPUP:

                break;
            case EnemyState.DEAD:

            default:
                break;
        }
    }
    void OnStateUpdate()
    {
        switch (currentState)
        {
            case EnemyState.IDLE:
                // TO WALK
                if (Vector2.Distance(transform.position, player.position) > playerDist)
                {
                    isOnRange = false;
                    TransitionToState(EnemyState.WALK);
                    StopCoroutine(Attack());
                }

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
            case EnemyState.JUMPUP:
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
                if (enemyDir.magnitude != 0)
                {
                    animator.SetBool("WALK_ENEMY01", false);
                }
                break;
            case EnemyState.ATTACK01:
                animator.SetTrigger("IsAttacking");
                break;

            case EnemyState.DEAD:
                break;
            case EnemyState.JUMPUP:
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

        // UTILISER LA COROUTINE POUR COMMENCER L'ANIMATION


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
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
            yield return new WaitForSeconds(1);
        }
    }

}

