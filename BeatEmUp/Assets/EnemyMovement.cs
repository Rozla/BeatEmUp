using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement: MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float walkEnemySpeed = 5f;  

    [SerializeField] EnemyState currentState;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] GameObject graphics;
    [SerializeField] float jumpEnemyDuration = 3f;
    public Transform player;

    [SerializeField] float stopDistance = 10f;
    public bool right;

    Vector2 enemyDir;
    public enum EnemyState
    {
        IDLE,
        WALK,
        JUMPUP,
        JUMPMAX,
        JUMPDOWN,
        ATTACK01,
        ATTACK02
        
        
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

        // SEULEMENT SI J'APPUI SUR UNE DIRECTION
        if (enemyDir.magnitude != 0)
        {
            animator.SetBool("WALK_ENEMY01", true);

        }

        if (enemyDir.x != 0)
        {
            right = enemyDir.x > 0;
            graphics.transform.rotation = right ? Quaternion.identity : Quaternion.Euler(0, 180f, 0);
        }

        enemyDir = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y) * walkEnemySpeed * Time.deltaTime;

        // Calcul de la distance entre l'ennemi et le joueur
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance == stopDistance)
        {
            return;
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
                // TO WALK
                if (enemyDir != Vector2.zero )
                {
                    TransitionToState(EnemyState.WALK);
                }
               
                break;
            case EnemyState.WALK:
                rb2d.velocity = enemyDir.normalized * walkEnemySpeed;
               
                // TO IDLE
                if (enemyDir == Vector2.zero)
                {
                    TransitionToState(EnemyState.IDLE);
                }
                
                break;
            case EnemyState.JUMPUP:

                TransitionToState(EnemyState.JUMPMAX);

                break;
            case EnemyState.JUMPMAX:
                TransitionToState(EnemyState.JUMPDOWN);

                break;
            case EnemyState.JUMPDOWN:

                
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
           
               
        }
    }
    void TransitionToState(EnemyState nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();
    }
}

