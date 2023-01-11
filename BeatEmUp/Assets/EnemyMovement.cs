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

    


    public Transform player;

    public bool right;


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
                animator.SetBool("WALK_ENEMY01", false);
                rb2d.velocity = Vector2.zero;

                break;
            case EnemyState.WALK:
                if (enemyDir.magnitude != 0)
                {
                    animator.SetBool("WALK_ENEMY01", true);
                }

                break;
            case EnemyState.ATTACK01:
                animator.SetBool("IsAttacking", true);
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
                if (enemyDir != Vector2.zero)
                {
                    TransitionToState(EnemyState.WALK);
                }
                //SI LE PLAYER EST A PORTE DE L'ENEMY JE JOUE MON ANIMATION D'ATTAQUE

                break;
            case EnemyState.WALK:
                rb2d.velocity = enemyDir.normalized * walkEnemySpeed;

                // TO IDLE
                if (enemyDir == Vector2.zero)
                {
                    TransitionToState(EnemyState.IDLE);
                }
                //SI LE PLAYER EST A PORTE DE L'ENEMY JE JOUE MON ANIMATION D'ATTAQUE
                // UTILISER LE RAYCAST POUR DETECTER LE PLAYER
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
                animator.SetBool("IsAttacking", false);
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
            // UTILISER LA COROUTINE POUR COMMENCER L'ANIMATION
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemyDir = Vector2.zero;
            // UTILISER LA COROUTINE POUR SORTIR DE L'ETAT

        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ;
        }
    }
}

